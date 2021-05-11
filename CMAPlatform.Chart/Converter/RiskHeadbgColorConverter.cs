using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class RiskHeadbgColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imagePath = "../Resouces/Images/RiskMessageBtnbg_Red.png";
            if (value == null)
            {
                return imagePath;
            }
            if (value.ToString() == "red")
            {
                imagePath = "../Resouces/Images/RiskMessageBtnbg_Red.png";
            }
            else if (value.ToString() == "orange")
            {
                imagePath = "../Resouces/Images/RiskMessage_Btnbg_Orange.png";
            }
            else if (value.ToString() == "yellow")
            {
                imagePath = "../Resouces/Images/RiskMessage_Btnbg_Yellow.png";
            }
            else if (value.ToString() == "blue")
            {
                imagePath = "../Resouces/Images/RiskMessage_Btnbg_Blue.png";
            }
            return imagePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }


    public class RiskHeadColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = "#D8464B";
            if (value == null)
            {
                return color;
            }
            if (value.ToString() == "red")
            {
                color = "#D8464B";
            }
            else if (value.ToString() == "orange")
            {
                color = "#FF7F00";
            }
            else if (value.ToString() == "yellow")
            {
                color = "#FFDC72";
            }
            else if (value.ToString() == "blue")
            {
                color = "#26C9FF";
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}