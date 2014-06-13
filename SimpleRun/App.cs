using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using SimpleRun.Views;
using SimpleRun.Models;
using SimpleRun.DataAccess;

#if __ANDROID__
using Android.Content;
#endif

namespace SimpleRun
{
	public class App
	{
#if __ANDROID__
		public static TabbedPage GetMainPage(Context context)
#else
		public static TabbedPage GetMainPage()
#endif
		{
			UserIsRunning = false;

			if (Settings.MeasurementType == DistanceUnit.None)
				InitSettings();
#if __ANDROID__
			return new RootTabbedPage(context);
#else
			return new RootTabbedPage();
#endif
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
			Settings.CreateOrUpdateKeyValue("MeasurementType", DistanceUnit.Miles.ToString());
		}
	}
}
