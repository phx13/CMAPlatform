using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class LayerBtnStyleShowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object strVisibility = Visibility.Visible;
            if (value.ToString() == "true" || value.ToString() == "True")
            {
                strVisibility = Visibility.Collapsed;
            }
            return strVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}