using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms;

namespace XamarinLab.Colors
{
    public class ColorListModel
    {
        public ColorListModel(IEnumerable<ColorModel> colors = null)
        {
            if (colors == null) Colors = new ObservableList<ColorModel>();
            else Colors = new ObservableList<ColorModel>(colors);
        }

        public ObservableList<ColorModel> Colors { get; }
        public string DisplayName { get; set; }

        public static ColorListModel GetSystemDrawingColors()
        {
            var colors = new List<ColorModel>();
            Type colorType = typeof(System.Drawing.Color);
            var props = colorType.GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var prop in props)
            {
                if (prop.PropertyType == colorType)
                {
                    var sdc = (System.Drawing.Color)prop.GetValue(null);
                    var xfc = Color.FromRgb(sdc.R, sdc.G, sdc.B);
                    var cm = new ColorModel { DisplayName = prop.Name, Color = xfc };
                    colors.Add(cm);
                }
            }

            colors.Sort((ColorModel left, ColorModel right) =>
                left.Color.Luminosity.CompareTo(right.Color.Luminosity)
            );

            ColorListModel systemColors = new ColorListModel(colors) { DisplayName = "System.Drawing Colors" };
            return systemColors;
        }

        public static ColorListModel GetXamarinFormsColors()
        {
            var colors = new List<ColorModel>();
            Type colorType = typeof(Xamarin.Forms.Color);
            var fields = colorType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                if (field.FieldType == colorType)
                {
                    Color c = (Color)field.GetValue(null);
                    var cm = new ColorModel { DisplayName = field.Name, Color = c };
                    colors.Add(cm);
                }
            }

            // Sort light to dark
            colors.Sort((ColorModel left, ColorModel right) =>
                -1 * left.Color.Luminosity.CompareTo(right.Color.Luminosity)
            );

            colors.Sort((ColorModel left, ColorModel right) =>
                -1 * left.Color.Saturation.CompareTo(right.Color.Saturation)
            );

            ColorListModel systemColors = new ColorListModel(colors) { DisplayName = "System.Drawing Colors" };
            return systemColors;
        }

        public void SortByLuminocity(ListSortDirection direction = ListSortDirection.Ascending)
        {
            int order = direction.Equals(ListSortDirection.Ascending) ? 1 : -1;
            Colors.Sort((a, b) => order * a.Color.Luminosity.CompareTo(b.Color.Luminosity));
        }

        public void SortByRBG(ListSortDirection direction = ListSortDirection.Ascending)
        {
            int order = direction.Equals(ListSortDirection.Ascending) ? 1 : -1;
            Colors.Sort((a, b) => order * a.Color.R.CompareTo(b.Color.R));
            Colors.Sort((a, b) => order * a.Color.G.CompareTo(b.Color.G));
            Colors.Sort((a, b) => order * a.Color.B.CompareTo(b.Color.B));
        }

        public void SortBySaturation(ListSortDirection direction = ListSortDirection.Ascending)
        {
            int order = direction.Equals(ListSortDirection.Ascending) ? 1 : -1;
            Colors.Sort((a, b) => order * a.Color.Saturation.CompareTo(b.Color.Saturation));
        }
    }
}