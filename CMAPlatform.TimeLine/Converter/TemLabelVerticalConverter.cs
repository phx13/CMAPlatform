using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CMAPlatform.TimeLine.Converter
{
    public class TemLabelVerticalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double tem = (double)value;
            return (tem >= 0) ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}