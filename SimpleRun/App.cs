using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using SimpleRun.Views;
using SimpleRun.Models;
using SimpleRun.DataAccess;

namespace SimpleRun
{
	public class App
	{
		public static TabbedPage GetMainPage()
		{
			UserIsRunning = false;

			return new RootTabbedPage();
		}

		public static Color StationaryTint {
			get {
				return Color.FromHex("3498DB");
			}
		}

		public static Color RunTint {
			get {
				return Color.FromHex("4CD964");
			}
		}

		public static Color StopTint {
			get {
				return Color.Red;
			}
		}

		public static bool UserIsRunning { get; set; }

		public static void SaveRun(Run newRun) {
			lock (Database.Main) {
				Database.Main.Insert(newRun);
			}
		}

		public static List<Run> GetRuns() {
			var runs = new List<Run>();
			lock (Database.Main) {
				runs = Database.Main.Table<Run>().ToList();
			}
			return runs;
		}
	}
}
