using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.Essentials
{
    [DisplayName("Carousel")]
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EssentialsCarouselPage : ContentPage
	{
		public EssentialsCarouselPage ()
		{
            try
            {
			    InitializeComponent ();
                BindingContext = new CollectionViewPageModel();
            }
            catch (Exception error)
            {
                Content = GetErrorView(error);
            }

		}

        private View GetErrorView(Exception error)
        {
            var stack = new StackLayout() { Orientation = StackOrientation.Vertical };
            stack.Children.Add(new Label() { Text = error.GetType().Name, FontSize = 15, TextColor = Color.White, FontAttributes=FontAttributes.Bold, HorizontalOptions = LayoutOptions.FillAndExpand });
            stack.Children.Add(new Label() { Text = error.Message, FontSize = 14, FontFamily = "monospace", TextColor = Color.DarkRed, BackgroundColor=Color.Gainsboro });
            stack.Children.Add(new Label() { Text = error.StackTrace, FontSize = 12, FontFamily = "monospace", TextColor = Color.Silver });
            var frame = new Frame() { HasShadow = true, BorderColor = Color.Red, CornerRadius=10, Padding=10, BackgroundColor=Color.Red, HorizontalOptions=LayoutOptions.Center, VerticalOptions=LayoutOptions.CenterAndExpand };
            frame.Content = stack;

            return frame;
        }
	}
}