using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class RiskHeadBgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imagePath = "../Resouces/Images/RiskHeadBg_Red.png";
            if (value == null)
            {
                return imagePath;
            }
            if (value.ToString() == "red")
            {
                imagePath = "../Resouces/Images/RiskHeadBg_Red.png";
            }
            else if (value.ToString() == "yellow")
            {
                imagePath = "../Resouces/Images/RiskHeadBg_Yellow.png";
            }
            else if (value.ToString() == "blue")
            {
                imagePath = "../Resouces/Images/RiskHeadBgImage_Blue.png";
            }
            else if (value.ToString() == "orange")
            {
                imagePath = "../Resouces/Images/RiskHeadBg_Orange.png";
            }
            return imagePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}