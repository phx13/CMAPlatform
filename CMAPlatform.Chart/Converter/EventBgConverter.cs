using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class EventBgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strColor = "../Resouces/Images/Eventbg_ImageWhite.png";
            if (value != null)
            {
                if (value.ToString() != "false" || value.ToString() != "false")
                {
                    strColor = "../Resouces/Images/Eventbg_Image.png";
                }
            }
            return strColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}