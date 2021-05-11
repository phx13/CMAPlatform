using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class PreventionColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strColor = "../Resouces/Images/PreventImage_Blue.png";
            if (value.ToString() == null)
            {
                return strColor;
            }
            if (value.ToString() == "true" || value.ToString() == "True" || value.ToString() == "TRUE")
            {
                strColor = "../Resouces/Images/PreventImage_Blue.png";
            }
            else if (value.ToString() == "false" || value.ToString() == "False" || value.ToString() == "FALSE")
            {
                strColor = "../Resouces/Images/PreventImage_Yellow.png";
            }
            return strColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}