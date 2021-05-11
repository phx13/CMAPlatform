using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CMAPlatform.TimeLine.Converter
{
    public class TimeBarLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var temp = double.Parse(value.ToString());
                if (temp == 0)
                {
                    return Visibility.Hidden;
                }
                return Visibility.Visible;
            }
            catch (Exception)
            {
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}