using System;
using System.Net;
using System.Threading;
using System.Reactive.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

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

		public HomePage()
		{
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

			runButton.Clicked += (sender, e) => {
				if (!App.UserIsRunning)
					StartRun();
				else
					StopRun();
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
						return (parent.Width / 2) - 125;
					
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
			distanceLabel.Text = Math.Round(tracker.CurrentDistance / 1000, 2).ToString("F") + " km";

			var pace = tracker.CurrentPace;
			if (pace == 0) return;

			var minutesPerKm = Math.Round(1 / (pace * 0.001 * 60.0), 2);
			var timeSpan = TimeSpan.FromMinutes(minutesPerKm);
			paceLabel.Text = timeSpan.ToString(@"mm\:ss") + " per km";
			//paceLabel.Text = pace.ToString();
		}

		void DurationTimerTick(long l)
		{
			TimeSpan currentTimespan = DateTimeOffset.Now - startTime;
			durationLabel.Text = currentTimespan.ToString(@"hh\:mm\:ss");
		}

		async Task StartRun()
		{
			App.UserIsRunning = true;

			await runButton.LayoutTo(new Rectangle(runButton.X, runButton.Y - 125, runButton.Width, runButton.Height), 150, Easing.SpringOut);
		
			var distanceTimer = Observable.Interval(TimeSpan.FromSeconds(2)).DistinctUntilChanged();
			var durationTimer = Observable.Interval(TimeSpan.FromSeconds(1)).DistinctUntilChanged();

			tracker.BeginTrackingLocation();

			runButton.Text = "Stop";
			BackgroundColor = App.RunTint;
			distanceLabel.Opacity = 1;
			paceLabel.Opacity = 1;
			durationLabel.Opacity = 1;

			startTime = DateTimeOffset.Now;
			distanceAndSpeedSubscription = distanceTimer.ObserveOn(SynchronizationContext.Current).Subscribe(DistanceAndSpeedTimerTick);
			durationSubscription = durationTimer.ObserveOn(SynchronizationContext.Current).Subscribe(DurationTimerTick);
		}

		async Task StopRun()
		{
			InitLabels();

			tracker.StopTrackingLocation();
			distanceAndSpeedSubscription.Dispose();
			durationSubscription.Dispose();

			await runButton.LayoutTo(new Rectangle(runButton.X, runButton.Y + 125, runButton.Width, runButton.Height), 150, Easing.SpringOut);
			App.UserIsRunning = false;
			BackgroundColor = App.StationaryTint;
			runButton.Text = "Run";
		}

		void InitLabels()
		{
			distanceLabel.Text = "0.00 km";
			paceLabel.Text = "00:00 per km";
			durationLabel.Text = "00:00:00";

			distanceLabel.Opacity = 0;
			paceLabel.Opacity = 0;
			durationLabel.Opacity = 0;
		}
	}
}
