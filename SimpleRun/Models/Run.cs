using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Geolocation;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using SimpleRun.DataAccess;

namespace SimpleRun.Models
{
	public class Run
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int DurationInSeconds { get; set; }
		public double DistanceInMeters { get; set; }
		public double AveragePaceInMetersPerSecond { get; set; }
		public DateTime RunDate { get; set; }

		[OneToMany]
		public List<RunPosition> RunPositions { get; set; }

		[Ignore]
		public string FriendlyDuration {
			get {
				var timeSpan = new TimeSpan(0, 0, DurationInSeconds);
				if (timeSpan.Hours == 0)
					return timeSpan.ToString(@"hh\:mm\:ss");

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

		public static List<Run> All() {
			var runs = new List<Run>();
			lock (Database.Main) {
				runs = Database.Main.Table<Run>().ToList();
			}
			return runs;
		}

		[Ignore]
		public List<RunPosition> Positions {
			get {
				var positions = new List<RunPosition>();
				lock (Database.Main) {
					positions = Database.Main.Table<RunPosition>().Where(r => r.RunID == ID).ToList();
				}
				return positions;
			}
		}
	}
}

