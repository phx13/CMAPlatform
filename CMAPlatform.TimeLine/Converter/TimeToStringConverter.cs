using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.TimeLine.Converter
{
    public class TimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = DateTime.Parse(value.ToString());

            return time.ToString("MM月dd日");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}