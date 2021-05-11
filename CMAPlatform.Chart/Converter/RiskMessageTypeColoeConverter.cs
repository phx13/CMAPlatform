using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class RiskMessageTypeColoeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var FontColor = "#D8464B";
            if (value == null)
            {
                return FontColor;
            }
            if (value.ToString() == "red")
            {
                FontColor = "#D8464B";
            }
            else if (value.ToString() == "orange")
            {
                FontColor = "#FF7F00";
            }
            else if (value.ToString() == "blue")
            {
                FontColor = "#00A3D8";
            }
            else if (value.ToString() == "yellow")
            {
                FontColor = "Yellow";
            }
            return FontColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}