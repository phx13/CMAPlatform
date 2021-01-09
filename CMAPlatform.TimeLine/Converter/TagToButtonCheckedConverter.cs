using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.TimeLine.Converter
{
    public class TagToButtonCheckedConverter : IValueConverter
    {
        private string timespan = "";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timespan = value.ToString();
            var tag = parameter.ToString();
            if (timespan == tag)
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = bool.Parse(value.ToString());
            if (flag)
            {
                timespan = parameter.ToString();
            }
            return timespan;
        }
    }
}