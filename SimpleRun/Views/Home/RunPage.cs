using System;
using System.Net;
using System.Threading;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace SimpleRun.Views.Home
{
	public class RunPage : ContentPage
	{
		Button runButton;
		Label distanceLabel;
		Label paceLabel;
		Label durationLabel;
		DateTimeOffset startTime;
		GeoLocationTracker tracker;

		public RunPage()
		{
			tracker = new GeoLocationTracker();

			distanceLabel = new Label {
				Text = "0.00 km",
				TextColor = Color.White,
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize(50),
			};

			paceLabel = new Label {
				Text = "00:00 per km",
				TextColor = Color.White,
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize(20),
			};

			durationLabel = new Label {
				Text = "00:00:00",
				TextColor = Color.White,
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize(20),
			};

			runButton = new Button {
				TextColor = Color.White,
				Font = Font.SystemFontOfSize(40),
			};

			var distanceTimer = Observable.Interval(TimeSpan.FromSeconds(2)).DistinctUntilChanged();
			var durationTimer = Observable.Interval(TimeSpan.FromSeconds(1)).DistinctUntilChanged();

			IDisposable distanceAndSpeedSubscription = null;
			IDisposable durationSubscription = null;

			runButton.Clicked += (sender, e) => {
				App.UserIsRunning = !App.UserIsRunning;

				SetUIState();

				if (App.UserIsRunning) {
					tracker.BeginTrackingLocation();
					startTime = DateTimeOffset.Now;
					distanceAndSpeedSubscription = distanceTimer.ObserveOn(SynchronizationContext.Current).Subscribe(DistanceAndSpeedTimerTick);
					durationSubscription = durationTimer.ObserveOn(SynchronizationContext.Current).Subscribe(DurationTimerTick);
				} else {
					tracker.StopTrackingLocation();
					distanceAndSpeedSubscription.Dispose();
					durationSubscription.Dispose();
				}
			};

			Content = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Children = {
					new StackLayout {
						VerticalOptions = LayoutOptions.Start,
						Padding = new Thickness(0, 20, 0, 0),
						Children = {
							runButton
						}
					},
					new StackLayout {
						VerticalOptions = LayoutOptions.CenterAndExpand,
						Children = {
							distanceLabel,
							paceLabel,
							durationLabel,
						}
					},
				}
			};

			Icon = "pin@2x.png";
			Title = "Run";

			SetUIState();
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

		void SetUIState()
		{
			BackgroundColor = App.UserIsRunning ? App.RunTint : App.StationaryTint;
			runButton.Text = App.UserIsRunning ? "Stop" : "Run";
		}
	}
}
