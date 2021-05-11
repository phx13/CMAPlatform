using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CMAPlatform.Chart.Converter
{
    public class LayerPicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = "";
            if (value != null)
            {
                path = "../Resouces/Images/LayerPic/" + value;
                //path = "D:\\ZH\\工作安排\\气象局\\CMAPlatform\\CMAPlatform.Chart\\Resouces\\Images\\LayerPic\\" + value;
            }
            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class LayerPicConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage imageSource = null;

            if (value != null)
            {
                var path =
                    string.Format(
                        @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/LayerPic/icon_{0}.png",
                        value);
                imageSource = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}