using System;
using System.Net;
using System.Threading;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace SimpleRun.Views.History
{
	public class HistoryNavigationPage : NavigationPage
	{
		public HistoryNavigationPage() : base(new HistoryHomePage())
		{
			Tint = App.StationaryTint;
			Icon = "book@2x.png";
			Title = "History";
		}
	}
}
