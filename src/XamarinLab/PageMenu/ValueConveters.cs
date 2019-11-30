using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinLab.PageList
{
    public class StringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new NotSupportedException("StringConverter only supports converting to strings.");
            }

            if (value == null)
            {
                return String.Empty;
            }
            if (value is Guid guid)
            {
                return guid.ToString(parameter as string);
            }
            else
            {
                return value.ToString();
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (value == null) return null;
                if (targetType == typeof(Guid))
                {
                    return new Guid(stringValue);
                }
            }

            throw new NotSupportedException("StringConverter only supports converting from strings.");
        }
    }
}
