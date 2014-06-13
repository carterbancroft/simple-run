using System;
using System.Net;
using System.Threading;
using System.Reactive.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

#if __ANDROID__
using Android.Content;
#endif

namespace SimpleRun.Views.Home
{
	public class HomePage : ContentPage
	{
		Button runButton;
		Label distanceLabel;
		Label paceLabel;
		Label durationLabel;
		Label referenceLabel;
		DateTimeOffset startTime;
		GeoLocationTracker tracker;
		RelativeLayout layout;

		IDisposable distanceAndSpeedSubscription;
		IDisposable durationSubscription;

		const int runButtonOffset = 125;

#if __ANDROID__
		Context context;

		public HomePage(Context _context)
		{
			context = _context;
#else
		public HomePage()
		{
#endif
			tracker = new GeoLocationTracker();
			layout = new RelativeLayout();
			BackgroundColor = App.StationaryTint;

			runButton = new Button {
				Text = "Run",
				TextColor = Color.White,
				Font = Font.SystemFontOfSize(50),
				BackgroundColor = App.RunTint,
				BorderRadius = 80,
			};

			// A Label whose upper-left is centered vertically and is simply used as a reference for layouts.
			referenceLabel = new Label
			{
				Text = "Run",
				Opacity = 0,
				Font = Font.SystemFontOfSize(50)
			};

			distanceLabel = new Label {
				TextColor = Color.White,
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize(50),
			};

			paceLabel = new Label {
				TextColor = Color.White,
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize(20),
			};

			durationLabel = new Label {
				TextColor = Color.White,
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize(20),
			};

			InitLabels();

			distanceAndSpeedSubscription = null;
			durationSubscription = null;

			runButton.Clicked += async (sender, e) => {
				if (!App.UserIsRunning)
					await StartRunAsync();
				else
					await StopRunAsync();
			};

			layout.Children.Add(
				referenceLabel,
				Constraint.RelativeToParent(parent => {
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent => {
					return parent.Height / 2;
				})
			);

			layout.Children.Add(
				runButton,
				Constraint.RelativeToParent(parent => {
					return (parent.Width / 2) / 2;
				}),
				Constraint.RelativeToParent(parent => {
					if (App.UserIsRunning)
						return (parent.Width / 2) - runButtonOffset;
					
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent => {
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent(parent => {
					return parent.Width / 2;
				})
			);

			var padding = 5;

			layout.Children.Add(
				distanceLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(referenceLabel, (parent, sibling) => {
					return sibling.Y - sibling.Height / 2;
				}),
				Constraint.RelativeToParent(parent => {
					return parent.Width;
				})
			);

			layout.Children.Add(
				paceLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(distanceLabel, (parent, sibling) => {
					return sibling.Y + referenceLabel.Height;
				}),
				Constraint.RelativeToParent(parent => {
					return parent.Width;
				})
			);

			layout.Children.Add(
				durationLabel,
				Constraint.Constant(0),
				Constraint.RelativeToView(paceLabel, (parent, sibling) => {
					return sibling.Y + paceLabel.Height + padding;
				}),
				Constraint.RelativeToParent(parent => {
					return parent.Width;
				})
			);

			Content = layout;

			Icon = "pin@2x.png";
			Title = "Run";
		}

		void DistanceAndSpeedTimerTick(long l)
		{
			distanceLabel.Text = tracker.CurrentDistanceString;
			paceLabel.Text = tracker.CurrentPaceString;
		}

		void DurationTimerTick(long l)
		{
			TimeSpan currentTimespan = DateTimeOffset.Now - startTime;
			durationLabel.Text = currentTimespan.ToString(@"hh\:mm\:ss");
		}

		async Task StartRunAsync()
		{
			App.UserIsRunning = true;

			await runButton.LayoutTo(new Rectangle(runButton.X, runButton.Y - runButtonOffset, runButton.Width, runButton.Height), 150, Easing.SpringOut);
		
			var distanceTimer = Observable.Interval(TimeSpan.FromSeconds(2)).DistinctUntilChanged();
			var durationTimer = Observable.Interval(TimeSpan.FromSeconds(1)).DistinctUntilChanged();

			runButton.Text = "Stop";
			BackgroundColor = App.RunTint;
			distanceLabel.Opacity = 1;
			paceLabel.Opacity = 1;
			durationLabel.Opacity = 1;

#if __ANDROID__
			tracker.BeginTrackingLocation(context);
#else
			tracker.BeginTrackingLocation();
#endif

			startTime = DateTimeOffset.Now;
			distanceAndSpeedSubscription = distanceTimer.ObserveOn(SynchronizationContext.Current).Subscribe(DistanceAndSpeedTimerTick);
			durationSubscription = durationTimer.ObserveOn(SynchronizationContext.Current).Subscribe(DurationTimerTick);
		}

		async Task StopRunAsync()
		{
			InitLabels();

			tracker.StopTrackingLocation();
			distanceAndSpeedSubscription.Dispose();
			durationSubscription.Dispose();

			await runButton.LayoutTo(new Rectangle(runButton.X, runButton.Y + runButtonOffset, runButton.Width, runButton.Height), 150, Easing.SpringOut);
			App.UserIsRunning = false;
			BackgroundColor = App.StationaryTint;
			runButton.Text = "Run";
		}

		void InitLabels()
		{
			distanceLabel.Text = "0.00 " + UnitUtility.DistanceUnitString;
			paceLabel.Text = "00:00 per " + UnitUtility.DistanceUnitString;
			durationLabel.Text = "00:00:00";

			distanceLabel.Opacity = 0;
			paceLabel.Opacity = 0;
			durationLabel.Opacity = 0;
		}
	}
}
