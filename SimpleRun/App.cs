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

			if (Settings.MeasurementType == MeasurementType.None)
				InitSettings();

			return new RootTabbedPage();
		}

		public static Color StationaryTint {
			get {
				return Color.White;
			}
		}

		public static Color RunTint {
			get {
				return Color.FromHex("4CD964");
			}
		}

		public static Color HeaderTint {
			get {
				return Color.FromHex("3498DB");;
			}
		}

		public static bool UserIsRunning { get; set; }

		static void InitSettings() {
			Settings.CreateOrUpdateKeyValue("MeasurementType", MeasurementType.Customary.ToString());
		}
	}
}
