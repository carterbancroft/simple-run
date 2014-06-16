using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using SimpleRun.Models;
using SimpleRun.Extensions;

namespace SimpleRun
{
	public class RunPage : ContentPage
	{
		Run run;

		Label distanceLabel;
		Label averagePaceLabel;
		Label durationLabel;

		// Right now I'm not including the map view in the Android version.
		Map map;

		double minLat;
		double maxLat;

		double minLon;
		double maxLon;

		public string Name { get; private set; }

		public RunPage(Run _run)
		{
			run = _run;
			Title = run.RunDate.ToShortDateString();

			ToolbarItems.Add(new ToolbarItem(
				string.Empty,
				"bin2x.png",
				async () =>
				{
					var page = new ContentPage();
					var result = await page.DisplayAlert("Delete", "Are you sure?", "Accept", "Cancel");
					if (result) {
						run.Delete();
						await Navigation.PopAsync();
					}
				}//,
				//ToolbarItemOrder.Secondary
			));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var font = Font.SystemFontOfSize(20);

			distanceLabel = new Label {
				Text = string.Format("Distance: {0}", run.FriendlyDistance),
				Font = font,
			};

			averagePaceLabel = new Label {
				Text = string.Format("Average Pace: {0}", run.FriendlyPace),
				Font = font,
			};

			durationLabel = new Label {
				Text = string.Format("Duration: {0}", run.FriendlyDuration),
				Font = font,
			};

			var layout = new StackLayout {
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
				}
			};

#if __IOS__
			map = new Map();

			var positions = run.Positions;

			var first = positions.FirstOrDefault();
			if (first != null) {
				minLat = maxLat = first.Latitude;
				minLon = maxLon = first.Longitude;
			}

			int pointReducer = (int)(0.04m * positions.Count);

			var count = 0;
			foreach (var pos in positions) {
				if (count % pointReducer != 0) {
					count += 1;
					continue;
				}

				CheckForMinMaxLatLon(pos.Latitude, pos.Longitude);
				Position position = new Position(pos.Latitude, pos.Longitude);

				map.Pins.Add(new Pin {
					Label = string.Format("{0} - {1} per km", pos.PositionCaptureTime, pos.Speed),
					Position = position,
					Type = PinType.Place
				});

				count += 1;
			}

			var center = new Position((minLat + maxLat) / 2, (minLon + maxLon) / 2);
			var latSpan = (maxLat - minLat) * 1.3;
			var lonSpan = (maxLon - minLon) * 1.3;

			map.MoveToRegion(new MapSpan(center, latSpan, lonSpan));

			layout.Children.Add(map);
#endif
			Content = layout;
		}

		void CheckForMinMaxLatLon(double lat, double lon) 
		{
			if (lat < minLat) minLat = lat;
			else if (lat > maxLat) maxLat = lat;

			if (lon < minLon) minLon = lon;
			else if (lon > maxLon) maxLon = lon;
		}
	}
}

