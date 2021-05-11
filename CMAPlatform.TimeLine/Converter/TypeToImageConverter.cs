using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CMAPlatform.TimeLine.Converter
{
    public class TypeToImageConverter : IMultiValueConverter
    {
        /// <summary>
        ///     正向转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var colorString = values[0].ToString();
            var typeString = values[1].ToString();

            switch (typeString)
            {
                case "11B01":
                case "11B03":
                case "11B04":
                case "11B05":
                case "11B06":
                case "11B07":
                case "11B09":
                case "11B14":
                case "11B15":
                case "11B16":
                case "11B17":
                    break;
                default:
                    typeString = "00000";
                    break;
            }
            ImageSource imageSource = new BitmapImage();
            if (string.IsNullOrWhiteSpace(colorString))
            {
                colorString = "unknown";
            }
            imageSource =
                new BitmapImage(
                    new Uri(
                        string.Format(
                            @"pack://application:,,,/CMAPlatform.TimeLine;component/Resources/Images/预警图标/{0}_{1}.png",
                            typeString, colorString.ToLower())));
            return imageSource;
        }

        /// <summary>
        ///     反向转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}