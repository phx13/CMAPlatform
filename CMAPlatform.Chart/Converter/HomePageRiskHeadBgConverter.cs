using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CMAPlatform.Chart.Converter
{
    public class HomePageRiskHeadBgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imagePath =
                @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/HomePageRiskHead_Red.png";
            if (value == null)
            {
                return imagePath;
            }
            if (value.ToString() == "red")
            {
                imagePath =
                    @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/HomePageRiskHead_Red.png";
            }
            else if (value.ToString() == "yellow")
            {
                imagePath =
                    @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/HomePageRiskHead_Yellows.png";
            }
            else if (value.ToString() == "blue")
            {
                imagePath =
                    @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/HomePageRiskHead_Blue.png";
            }
            else if (value.ToString() == "orange")
            {
                imagePath =
                    @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/HomePageRiskHead_Orange.png";
            }

            ImageSource imageSource = new BitmapImage(new Uri(imagePath));

            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}