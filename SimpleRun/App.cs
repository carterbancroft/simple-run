using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using SimpleRun.Views;

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
	}
}
