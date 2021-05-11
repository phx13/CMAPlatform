using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class ProvinceColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strColor = "Red";
            if (value == null)
            {
                return strColor;
            }
            var num = System.Convert.ToDouble(value.ToString());
            if (num <= 0.2)
            {
                strColor = "#1A42C1CC";
            }
            else if (num > 0.2 && num <= 0.4)
            {
                strColor = "#3342C1CC";
            }
            else if (num > 0.4 && num <= 0.6)
            {
                strColor = "#8042C1CC";
            }
            else if (num > 0.6 && num <= 0.8)
            {
                strColor = "#B242C1CC";
            }
            else if (num > 0.8 && num <= 1)
            {
                strColor = "#CC42C1CC";
            }
            else
            {
                strColor = "#CC42C1CC";
            }
            return strColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}