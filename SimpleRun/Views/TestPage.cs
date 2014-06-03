using System;
using Xamarin.Forms;

namespace SimpleRun.Views
{
	public class TestPage : ContentPage
	{
		Label testLabel;

		public TestPage()
		{
			Title = "Testing";

			testLabel = new Label {
				Text = "Testing... 1... 2...",
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

