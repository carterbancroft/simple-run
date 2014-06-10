using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using SimpleRun.Models;
using SimpleRun.Views.Custom;

namespace SimpleRun.Views.Settings
{
	public class SettingsListPage : ContentPage
	{
		TableView tableView;
		RightImageCell milesImageCell;
		RightImageCell kmImageCell;

		public SettingsListPage()
		{
			Title = "Settings";
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			tableView = new TableView {
				Intent = TableIntent.Form,
			};

			milesImageCell = new RightImageCell {
				Title = "Miles",
				Source = "check",
				Command = new Command(() => {
					milesImageCell.ImageIsVisible = !milesImageCell.ImageIsVisible;
					if (kmImageCell.ImageIsVisible)
						kmImageCell.ImageIsVisible = false;

					SimpleRun.Models.Settings.MeasurementType = DistanceUnit.Miles;
				}),
				ImageIsVisible = SimpleRun.Models.Settings.MeasurementType == DistanceUnit.Miles
			};

			kmImageCell = new RightImageCell {
				Title = "Kilometers",
				Source = "check",
				Command = new Command(() => {
					kmImageCell.ImageIsVisible = !kmImageCell.ImageIsVisible;
					if (milesImageCell.ImageIsVisible)
						milesImageCell.ImageIsVisible = false;

					SimpleRun.Models.Settings.MeasurementType = DistanceUnit.Kilometers;
				}),
				ImageIsVisible = SimpleRun.Models.Settings.MeasurementType == DistanceUnit.Kilometers
			};

			var root = new TableRoot();
			var section = new TableSection("Distance Unit");

			section.Add(kmImageCell);
			section.Add(milesImageCell);
			root.Add(section);

			tableView.Root = root;
			Content = tableView;
		}
	}
}

