using System;
using Xamarin.Forms;

using SimpleRun.Views.Home;
using SimpleRun.Views.History;
using SimpleRun.Views.Settings;

namespace SimpleRun.Views
{
	public class RootTabbedPage : TabbedPage
	{
		HomePage runPage;
		HistoryNavigationPage historyPage;
		SettingsNavigationPage settingsPage;

		public RootTabbedPage()
		{
			runPage = new HomePage();
			historyPage = new HistoryNavigationPage();
			settingsPage = new SettingsNavigationPage();

			Children.Add(runPage);
			Children.Add(historyPage);
			Children.Add(settingsPage);
		}
	}
}
