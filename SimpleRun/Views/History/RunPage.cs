using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using SimpleRun.Models;

namespace SimpleRun
{
	public class RunPage : ContentPage
	{
		Run run;

		Label distanceLabel;
		Label averagePaceLabel;
		Label durationLabel;
		Map map;

		public string Name { get; private set; }

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

			map = new Map();

			Position position = new Position(37.79762, -122.40181);
			map.MoveToRegion(new MapSpan(position, 0.01, 0.01));
			map.Pins.Add(new Pin {
				Label = "Xamarin",
				Position = position
			});

			Content = new StackLayout {
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					new StackLayout {
						VerticalOptions = LayoutOptions.Start,
						Padding = new Thickness(10),
						Children = {
							durationLabel,
							distanceLabel,
							averagePaceLabel,
						}
					},
					map,
				}
			};
		}
	}
}

