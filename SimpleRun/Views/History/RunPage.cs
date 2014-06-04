using System;
using Xamarin.Forms;
using SimpleRun.Models;

namespace SimpleRun
{
	public class RunPage : ContentPage
	{
		Run run;

		Label distanceLabel;
		Label averagePaceLabel;
		Label durationLabel;

		public RunPage(Run _run)
		{
			run = _run;

			Title = run.RunDate.ToShortDateString();

			var font = Font.SystemFontOfSize(20);

			distanceLabel = new Label {
				Text = string.Format("Distance: {0} km", run.DistanceInKm.ToString("F")),
				Font = font,
			};

			averagePaceLabel = new Label {
				Text = string.Format("Average Pace: {0} per km", run.FriendlyPaceInKm),
				Font = font,
			};

			durationLabel = new Label {
				Text = string.Format("Duration: {0}", run.FriendlyDuration),
				Font = font,
			};

			Content = new StackLayout {
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.Start,
				Padding = new Thickness(20),
				Children = {
					durationLabel,
					distanceLabel,
					averagePaceLabel,
				}
			};
		}
	}
}

