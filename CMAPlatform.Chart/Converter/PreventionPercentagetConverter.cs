using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class PreventionPercentagetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strNum = "0%";
            if (value == strNum)
            {
                return strNum;
            }
            var num = System.Convert.ToDouble(value)*100;
            strNum = num + "%";
            return strNum;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}