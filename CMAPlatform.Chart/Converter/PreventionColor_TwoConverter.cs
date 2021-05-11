using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class PreventionColor_TwoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strColor = "../Resouces/Images/PreventImage_Blue_Two.png";
            if (value.ToString() == null)
            {
                return strColor;
            }
            if (value.ToString() == "true" || value.ToString() == "True" || value.ToString() == "TRUE")
            {
                strColor = "../Resouces/Images/PreventImage_Blue_Two.png";
            }
            else if (value.ToString() == "false" || value.ToString() == "False" || value.ToString() == "FALSE")
            {
                strColor = "../Resouces/Images/PreventImage_Yellow_Two.png";
            }
            return strColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}