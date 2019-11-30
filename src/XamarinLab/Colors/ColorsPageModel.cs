using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms;
using XamarinLab.Mvvm;

namespace XamarinLab.Colors
{
    public class ColorsPageModel : BaseModel
    {
        private ColorModel _selectedBackgroundColor;
        private ColorSortOption _selectedSwatchSort;

        public ColorsPageModel(IEnumerable<ColorModel> colorSwatches = null)
        {
            if (colorSwatches == null) Swatches = new ObservableList<ColorModel>();
            else Swatches = new ObservableList<ColorModel>(colorSwatches);

            SwatchSortChoices = new List<ColorSortOption>(ColorSortOption.GetColorSortOptions());

            SortSwatches(ColorSortType.Name);
            _selectedSwatchSort = SwatchSortChoices.Find(sso => sso.SortType == ColorSortType.Name && sso.Direction == ListSortDirection.Ascending);

            BackgroundColorChoices = GetNamedHtmlColorsColors();
            _selectedBackgroundColor = BackgroundColorChoices.Find(bc => bc.Name == "Gainsboro");
        }

        public List<ColorModel> BackgroundColorChoices { get; }

        public ColorModel SelectedBackgroundColor
        {
            get
            {
                return _selectedBackgroundColor;
            }
            set
            {
                SetAndNotify(ref _selectedBackgroundColor, value);
            }
        }

        public ColorSortOption SelectedSwatchSort
        {
            get
            {
                return _selectedSwatchSort;
            }
            set
            {
                if (Changed(_selectedSwatchSort, value))
                {
                    SortSwatches(value);

                    _selectedSwatchSort = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableList<ColorModel> Swatches { get; }

        public List<ColorSortOption> SwatchSortChoices { get; }

        public string Title { get; set; }

        public static List<ColorModel> GetNamedHtmlColorsColors()
        {
            var colors = new List<ColorModel>();
            Type colorType = typeof(Xamarin.Forms.Color);
            var fields = colorType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                if (field.FieldType == colorType)
                {
                    Color c = (Color)field.GetValue(null);
                    var cm = new ColorModel { Name = field.Name, Color = c };
                    colors.Add(cm);
                }
            }

            colors.Sort();

            return colors;
        }

        private void SortSwatches(ColorSortOption sortOption)
        {
            if (sortOption == null) return;
            SortSwatches(sortOption.SortType, sortOption.Direction);
        }

        private void SortSwatches(ColorSortType sortType, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            int sd = sortDirection.Equals(ListSortDirection.Ascending) ? 1 : -1;

            switch (sortType)
            {
                case ColorSortType.Luminocity:
                    Swatches.Sort((a, b) =>
                    {
                        var aa = a.Color.Luminosity;
                        var bb = b.Color.Luminosity;
                        return sd * aa.CompareTo(bb);
                    });
                    break;

                case ColorSortType.Saturation:
                    Swatches.Sort((a, b) => sd * a.Color.Saturation.CompareTo(b.Color.Saturation));
                    break;

                case ColorSortType.Hue:
                    Swatches.Sort((a, b) =>
                    {
                        var aa = a.Color.Hue + a.Color.Luminosity;
                        var bb = b.Color.Hue + b.Color.Luminosity;
                        return sd * aa.CompareTo(bb);
                    });
                    Swatches.Sort((a, b) => sd * a.Color.Hue.CompareTo(b.Color.Hue));
                    break;

                case ColorSortType.Name:
                    Swatches.Sort((a, b) => sd * a.Name.CompareTo(b.Name));
                    break;
            }
        }
    }
}