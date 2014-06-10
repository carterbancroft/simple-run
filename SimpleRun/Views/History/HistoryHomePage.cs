using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using SimpleRun.Models;

namespace SimpleRun.Views.History
{
	public class HistoryHomePage : ContentPage
	{
		TableView tableView;

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
			foreach (var run in Run.All()) {
				runPages.Add(new RunPage(run));
			}

			tableView = new TableView {
				Intent = TableIntent.Form,
			};

			var root = new TableRoot();
			var groupedRunsByDate = Run.All().OrderByDescending(r => r.RunDate).GroupBy(r => r.RunDate.Date).ToDictionary(r => r.Key, r => r.ToList());

			foreach (var key in groupedRunsByDate.Keys) {
				var newSection = new TableSection(key.ToShortDateString());
				foreach (var run in groupedRunsByDate[key]) {
					newSection.Add(new TextCell {
						Text = run.FriendlyDistance,
						Detail = run.FriendlyDuration,
						Command = new Command(async () => await Navigation.PushAsync(new RunPage(run))),
					});
				}
				root.Add(newSection);
			}

			tableView.Root = root;
			Content = tableView;
		}
	}
}

