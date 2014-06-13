using System;
using Xamarin.Forms;

using SimpleRun.Views.Home;
using SimpleRun.Views.History;
using SimpleRun.Views.Settings;

#if __ANDROID__
using Android.Content;
#endif

namespace SimpleRun.Views
{
	public class RootTabbedPage : TabbedPage
	{
		HomePage runPage;
		HistoryNavigationPage historyPage;
		SettingsNavigationPage settingsPage;

#if __ANDROID__
		public RootTabbedPage(Context context)
		{
			runPage = new HomePage(context);
#else
		public RootTabbedPage()
		{
			runPage = new HomePage();
#endif
			historyPage = new HistoryNavigationPage();
			settingsPage = new SettingsNavigationPage();

			Children.Add(runPage);
			Children.Add(historyPage);
			Children.Add(settingsPage);
		}
	}
}
