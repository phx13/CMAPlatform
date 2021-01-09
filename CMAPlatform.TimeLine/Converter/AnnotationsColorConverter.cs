using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CMAPlatform.TimeLine.Converter
{
    public class AnnotationsColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value.ToString();
            if (s == "1" || s == "3")
            {
                return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFA64D"));
            }
            return new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFFF00"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}