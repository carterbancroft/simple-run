using System;
using Xamarin.Forms;

namespace SimpleRun.Views.Custom
{
	public class RightImageCell : ViewCell
	{
		Image image;
		Label title;

		public string Source { 
			set {
				image.Source = FileImageSource.FromFile(value); 
			}
		}

		public string Title { 
			get {
				return title.Text;
			}
			set {
				title.Text = value;
			}
		}

		public bool ImageIsVisible {
			get {
				return image.IsVisible;
			}
			set {
				image.IsVisible = value;
			}
		}

		public Command Command { get; set; } 

		public RightImageCell(string imageSource = null)
		{
			title = new Label {
				YAlign = TextAlignment.Center
			};

			image = new Image();
			if (!string.IsNullOrEmpty(imageSource)) {
				image.Source = FileImageSource.FromFile(imageSource);
			}

			var layout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Children = { 
					new StackLayout {
						Padding = new Thickness(20, 0, 0, 0),
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.StartAndExpand,
						Children = {
							title,
						}
					}, 
					new StackLayout {
						Padding = new Thickness(0, 0, 20, 0),
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.EndAndExpand,
						Children = {
							image,
						}
					},
				}
			};
			View = layout;
		}

		protected override void OnTapped()
		{
			base.OnTapped();
			if (Command != null) {
				Command.Execute(null);
			}
		}

		protected override void OnBindingContextChanged()
		{
			// Fixme : this is happening because the View.Parent is getting 
			// set after the Cell gets the binding context set on it. Then it is inheriting
			// the parents binding context.
			View.BindingContext = BindingContext;
			base.OnBindingContextChanged();
		}
	}
}

