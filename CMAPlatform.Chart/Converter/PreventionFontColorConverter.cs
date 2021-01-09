using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class PreventionFontColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strColor = "Red";
            if (value.ToString() == null)
            {
                return strColor;
            }
            if (value.ToString() == "true" || value.ToString() == "True" || value.ToString() == "TRUE")
            {
                strColor = "#00FFFF";
            }
            else if (value.ToString() == "false" || value.ToString() == "False" || value.ToString() == "FALSE")
            {
                strColor = "#FFA64D";
            }
            return strColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}