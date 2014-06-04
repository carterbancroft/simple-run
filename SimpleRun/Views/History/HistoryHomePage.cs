using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SimpleRun.Views.History
{
	public class HistoryHomePage : ContentPage
	{
		ListView listView;

		public HistoryHomePage()
		{
			Title = "History";
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var cellTemplate = new DataTemplate (typeof (TextCell));
			cellTemplate.SetBinding (TextCell.TextProperty, new Binding("Title"));

			var runPages = new List<RunPage>();
			foreach (var run in App.GetRuns()) {
				runPages.Add(new RunPage(run));
			}

			listView = new ListView {
				RowHeight = 40,
				//IsGroupingEnabled = true,
				ItemTemplate = cellTemplate,
				ItemsSource = runPages,
			};

			listView.ItemSelected += (sender, arg) => {
				if (listView.SelectedItem != null) {
					Navigation.PushAsync((ContentPage)listView.SelectedItem);
					listView.SelectedItem = null;
				}
			};

			Content = listView;
		}
	}
}

