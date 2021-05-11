using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class EventFontColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fontColor = "#ACE5C5";
            if (value != null)
            {
                if (value.ToString() != "false" || value.ToString() != "false")
                {
                    fontColor = "#777777";
                }
            }
            return fontColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}