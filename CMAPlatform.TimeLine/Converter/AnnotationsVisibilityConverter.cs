using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CMAPlatform.TimeLine.Converter
{
    public class AnnotationsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value.ToString();

            if (s == "1" || s == "3")
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}