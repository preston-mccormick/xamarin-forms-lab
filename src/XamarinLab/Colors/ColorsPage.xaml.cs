using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinLab.Colors
{
    [DisplayName("Colors")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorsPage : ContentPage
    {
        public ColorsPage() : this(null)
        { }

        public ColorsPage(ColorsPageModel model)
        {
            InitializeComponent();

            if (model == null)
            {
                var colors = ColorsPageModel.GetNamedHtmlColorsColors();
                model = new ColorsPageModel(colors) { Title = "Named HTML Colors" };
            }
            BindingContext = model;

            SpacingChanged(SpacingSlider, new ValueChangedEventArgs(0.1, SpacingSlider.Value));
        }

        private void ScrollSwatchesToTop(object sender, System.EventArgs e)
        {
            // Otherwise it stays in the old position but all the swatches change.
            Swatches.ScrollTo(0);
        }

        private void SpacingChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue > 0)
            {
                BackgroundColorPicker.IsEnabled = true;
                BackgroundColorPicker.TextColor = Color.Default;
            }
            else
            {
                BackgroundColorPicker.IsEnabled = false;
                BackgroundColorPicker.TextColor = Color.Default;
            }
        }

        private void SpanChanged(object sender, ValueChangedEventArgs e)
        {
            // Hack to fix glitches after changing span on-the-fly.

            SwatchLayout.RemoveBinding(GridItemsLayout.HorizontalItemSpacingProperty);
            SwatchLayout.HorizontalItemSpacing += 0.01;
            //SwatchLayout.HorizontalItemSpacing -= 0.01;
            SwatchLayout.SetBinding(GridItemsLayout.HorizontalItemSpacingProperty, new Binding("Value", source: SpacingSlider));
        }
    }
}