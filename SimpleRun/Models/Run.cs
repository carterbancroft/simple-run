using System;
using SQLite;

namespace SimpleRun.Models
{
	public class Run
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public DateTime RunDate { get; set; }
		public double DistanceInMeters { get; set; }
		public double AveragePaceInMetersPerSecond { get; set; }
		public int DurationInSeconds { get; set; }

		[Ignore]
		public string FriendlyDuration {
			get {
				var timeSpan = new TimeSpan(0, 0, DurationInSeconds);
				return timeSpan.ToString(@"hh\:mm\:ss");
			}
		}

		[Ignore]
		public double DistanceInKm {
			get {
				return Math.Round(DistanceInMeters / 1000, 2);
			}
		}

		[Ignore]
		public string FriendlyPaceInKm {
			get {
				if (AveragePaceInMetersPerSecond == 0)
					return "00:00";

				var minutesPerKm = Math.Round(1 / (AveragePaceInMetersPerSecond * 0.001 * 60.0), 2);
				var timeSpan = TimeSpan.FromMinutes(minutesPerKm);
				return timeSpan.ToString(@"mm\:ss");
			}
		}
	}
}

