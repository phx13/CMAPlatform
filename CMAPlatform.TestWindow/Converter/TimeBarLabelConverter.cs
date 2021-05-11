using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CMAPlatform.TestWindow.Converter
{
    public class TimeBarLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = double.Parse(value.ToString());
            if (temp == 0)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}