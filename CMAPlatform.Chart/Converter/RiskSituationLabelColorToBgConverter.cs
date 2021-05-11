using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CMAPlatform.Chart.Converter
{
    /// <summary>
    ///     风险态势标签控件(落区标签) 颜色到背景图片的转换器
    /// </summary>
    public class RiskSituationLabelColorToBgConverter : IValueConverter
    {
        /// <summary>
        ///     正向转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorString = value.ToString().ToLower();
            var imagePath = @"pack://application:,,,/CMAPlatform.Chart;component/Images/RiskRedBg.png";
            if (colorString == "red")
            {
                imagePath = @"pack://application:,,,/CMAPlatform.Chart;component/Images/RiskRedBg.png";
            }
            else if (colorString == "orange")
            {
                imagePath = @"pack://application:,,,/CMAPlatform.Chart;component/Images/RiskOrangeBg.png";
            }
            else if (colorString == "yellow")
            {
                imagePath = @"pack://application:,,,/CMAPlatform.Chart;component/Images/RiskYellowBg.png";
            }
            else if (colorString == "blue")
            {
                imagePath = @"pack://application:,,,/CMAPlatform.Chart;component/Images/RiskBlueBg.png";
            }
            var imageUri = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            var imageBitmap = new BitmapImage(imageUri);
            return imageBitmap;
        }

        /// <summary>
        ///     逆向转化（未实现）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}