using System.ComponentModel;
using Xamarin.Forms;

namespace XamarinLab.Components
{
    public class BlockView : ContentView
    {
        public BlockView()
        {
            Content = new StackLayout
            {
                Children = {
                    new StackLayout { HeightRequest=100,
                        Spacing = 0, Padding=new Thickness(5), BackgroundColor=Color.White,
                        Children = {
                                new BoxView {VerticalOptions=LayoutOptions.FillAndExpand, BackgroundColor=Color.Blue },
                                new BoxView {VerticalOptions=LayoutOptions.FillAndExpand, BackgroundColor=Color.Red }
                        }
                    }
                }
            };
        }
    }
}