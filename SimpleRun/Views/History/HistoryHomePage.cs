using System;
using Xamarin.Forms;

namespace SimpleRun.Views.History
{
	public class HistoryHomePage : ContentPage
	{
		ListView listView;

		public HistoryHomePage()
		{
			Title = "History";

			var cellTemplate = new DataTemplate (typeof (TextCell));
			cellTemplate.SetBinding (TextCell.TextProperty, new Binding("Title"));

			listView = new ListView {
				RowHeight = 40,
				//IsGroupingEnabled = true,
				ItemTemplate = cellTemplate,
				ItemsSource = new [] {
					new TestPage(),
				},
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

