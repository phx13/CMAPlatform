using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CMAPlatform.TimeLine.Converter
{
    public class TimeLineTimeBackgroundConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (parameter == null) return null;

            SolidColorBrush solidColorBrush = null;

            DateTime actualTime = DateTime.Parse(parameter.ToString());
            DateTime typhoonTime = DateTime.Parse(value.ToString());

            if (actualTime != typhoonTime || typhoonTime == DateTime.Today.AddDays(1))
            {
                solidColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#044A6C"))
                {
                    Opacity = 0.8
                };
            }
            else
            {
                solidColorBrush = new SolidColorBrush(Colors.OrangeRed)
                {
                    Opacity = 1
                };
            }

            return solidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}