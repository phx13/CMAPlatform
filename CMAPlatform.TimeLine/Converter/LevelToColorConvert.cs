using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CMAPlatform.TimeLine.Converter
{
    public class LevelToColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color) ColorConverter.ConvertFromString("#dddddd");

            var colorString = value.ToString();
            switch (colorString.ToLower())
            {
                case "red":
                    color = (Color) ColorConverter.ConvertFromString("#C5364B");
                    break;
                case "orange":
                    color = (Color) ColorConverter.ConvertFromString("#DB6D00");
                    break;
                case "yellow":
                    color = (Color) ColorConverter.ConvertFromString("#D3AD14");
                    break;
                case "blue":
                    color = (Color) ColorConverter.ConvertFromString("#019ADC");
                    break;
            }
            Brush brush = new SolidColorBrush(color);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}