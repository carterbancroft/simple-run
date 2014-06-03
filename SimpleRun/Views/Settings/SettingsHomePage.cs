using System;
using Xamarin.Forms;

namespace SimpleRun.Views.Settings
{
	public class SettingsHomePage : ContentPage
	{
		Label testLabel;

		public SettingsHomePage()
		{
			Title = "Settings";

			testLabel = new Label {
				Text = "Settings... yeah...",
				Font = Font.SystemFontOfSize(20),
			};

			Content = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness(10),
				Children = {
					testLabel
				}
			};
		}
	}
}

