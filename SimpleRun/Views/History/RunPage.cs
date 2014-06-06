using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
//using Xamarin.Geolocation;
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

			var lastPosition = new Position(37.79762, -122.40181);

			foreach (var pos in run.Positions) {
				Position position = new Position(pos.Latitude, pos.Longitude);

				map.Pins.Add(new Pin {
					Label = string.Format("{0} - {1} per km", pos.PositionCaptureTime, pos.Speed),
					Position = position
				});

				lastPosition = position;
			}

			map.MoveToRegion(new MapSpan(lastPosition, 0.01, 0.01));

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

