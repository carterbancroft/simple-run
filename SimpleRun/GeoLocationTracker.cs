using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Geolocation;
using SimpleRun.Models;

#if __IOS__
using MonoTouch.CoreLocation;
#endif

namespace SimpleRun
{
	public class GeoLocationTracker
	{
		const int positionHistorySize = 5;
		const int minPositionsNeededToUpdateStats = 3;
		const double maxHorizontalAccuracy = 40.0f;

		TimeSpan statsCalculationInterval;
		TimeSpan validLocationHistoryDeltaInterval;

		DateTimeOffset previousDistanceTime;
		DateTimeOffset startTime;

		Geolocator geolocator;

		Position previousPosition;
		List<Position> positionHistory;
		List<Position> routePositions;
		List<double> totalAveragePaceHistory;

		public double CurrentDistance { get; set; }
		public double CurrentPace { get; set; }
		public double AveragePace { 
			get { 
				if (totalAveragePaceHistory.Count == 0)
					return 0;

				return totalAveragePaceHistory.Average();
			}
		}

		void Init()
		{
			CurrentDistance = 0;
			CurrentPace = 0;
			positionHistory = new List<Position>();
			routePositions = new List<Position>();
			totalAveragePaceHistory = new List<double>();
			previousPosition = null;
			statsCalculationInterval = new TimeSpan(0, 0, 1);
			validLocationHistoryDeltaInterval = new TimeSpan(0, 0, 1);
		}

		public void BeginTrackingLocation()
		{
			Init();

			geolocator = new Geolocator() { DesiredAccuracy = 1 };

			startTime = DateTime.Now;

			geolocator.PositionChanged += delegate(object sender, PositionEventArgs e) {
				if (e.Position.Accuracy == 0.0f || e.Position.Accuracy > maxHorizontalAccuracy)
					return;

				positionHistory.Add(e.Position);

				if (previousPosition == null)
					previousDistanceTime = startTime;

				if (positionHistory.Count > positionHistorySize)
					positionHistory.RemoveAt(0);

				var canUpdateStats = positionHistory.Count >= minPositionsNeededToUpdateStats;

				if (e.Position.Timestamp - previousDistanceTime <= statsCalculationInterval)
					return;

				previousDistanceTime = e.Position.Timestamp;

				Position bestPosition = null;
				var bestAccuracy = maxHorizontalAccuracy;

				foreach (var position in positionHistory) {
					if (DateTimeOffset.Now - position.Timestamp <= validLocationHistoryDeltaInterval && position.Accuracy < bestAccuracy && position != previousPosition) {
						bestAccuracy = position.Accuracy;
						bestPosition = position;
					}
				}

				if (bestPosition == null) bestPosition = e.Position;

				if (canUpdateStats) {
#if __IOS__
					var bestLocation = new CLLocation(bestPosition.Latitude, bestPosition.Longitude);
#endif

					if (previousPosition != null) {
#if __IOS__
						var prevLocation = new CLLocation(previousPosition.Latitude, previousPosition.Longitude);
						var test = bestLocation.DistanceFrom(prevLocation);
#endif
						CurrentDistance += DistanceInMeters(bestPosition, previousPosition);

						totalAveragePaceHistory.Add(bestPosition.Speed);
						CurrentPace = bestPosition.Speed;
					}
				}

				routePositions.Add(bestPosition);
				previousPosition = bestPosition;
			};

			geolocator.PositionError += delegate(object sender, PositionErrorEventArgs e) {
				Console.WriteLine("Location Manager Failed: " + e.Error);
			};

			geolocator.StartListening(minTime: 1000, minDistance: 1, includeHeading: false);
		}

		public void StopTrackingLocation()
		{
			geolocator.StopListening();

			TimeSpan totalRunTime = DateTimeOffset.Now - startTime;

			var newRun = new Run {
				RunDate = DateTime.Now,
				DistanceInMeters = CurrentDistance,
				AveragePaceInMetersPerSecond = AveragePace,
				DurationInSeconds = Convert.ToInt32(Math.Round(totalRunTime.TotalSeconds, 0)),
			};

			App.SaveRun(newRun);

			Init();
		}

		/// <summary>
		/// Used for calculating distance in meters between two points. 
		/// Code pulled from here: http://slodge.blogspot.com/2012/04/calculating-distance-between-latlng.html
		/// Attributed originally to here: http://www.movable-type.co.uk/scripts/latlong.html 
		/// </summary>
		/// <returns>The distance in meters.</returns>
		/// <param name="position1">The furthest along position</param>
		/// <param name="position2">The position to determine distance from.</param>
		public double DistanceInMeters(Position recentPosition, Position previousPosition)
		{
			var lat1 = recentPosition.Latitude;
			var lon1 = recentPosition.Longitude;

			var lat2 = previousPosition.Latitude;
			var lon2 = previousPosition.Longitude;

			if (lat1 == lat2 && lon1 == lon2)
				return 0.0;

			var theta = lon1 - lon2;

			var distance = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) +
				Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
				Math.Cos(deg2rad(theta));

			distance = Math.Acos(distance);
			if (double.IsNaN(distance))
				return 0.0;

			distance = rad2deg(distance);
			distance = distance * 60.0 * 1.1515 * 1609.344;

			return distance;
		}

		double deg2rad(double deg)
		{
			return (deg * Math.PI / 180.0);
		}

		double rad2deg(double rad)
		{
			return (rad / Math.PI * 180.0);
		}
	}
}
