using System;
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
	}
}

