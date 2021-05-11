using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class RiskWarningBackGroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Colour = "Transparent";

            if (value != null)
            {
                if (value.ToString() == "true" || value.ToString() == "True")
                {
                    Colour = "#3300FFFF";
                }
            }
            return Colour;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}