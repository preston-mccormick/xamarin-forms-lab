using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.Colors
{
    [DisplayName("Colors")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorsPage : ContentPage
    {
        public ColorsPage()
        {
            InitializeComponent();
            //BindingContext = ColorListModel.GetSystemDrawingColors();
            var colors = ColorListModel.GetXamarinFormsColors();
            colors.SortByRBG();
            BindingContext = colors;
        }
    }
}