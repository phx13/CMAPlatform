using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CMAPlatform.Chart.Converter
{
    /// <summary>
    ///     风险态势标签控件(落区标签) 颜色到背景图片的转换器
    /// </summary>
    public class EventPageRiskSituationLabelColorToBgConverter : IValueConverter
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
            var imagePath =
                @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/EventPageLabel/UnknownBackground.png";
            if (colorString == "red")
            {
                imagePath =
                    @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/EventPageLabel/RedBackground.png";
            }
            else if (colorString == "orange")
            {
                imagePath =
                    @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/EventPageLabel/OrangeBackground.png";
            }
            else if (colorString == "yellow")
            {
                imagePath =
                    @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/EventPageLabel/YellowBackground.png";
            }
            else if (colorString == "blue")
            {
                imagePath =
                    @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/EventPageLabel/BlueBackground.png";
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

    /// <summary>
    ///     风险态势标签控件(落区标签) 类型到图片的转换器
    /// </summary>
    public class EventPageRiskSituationLabelTypeToImageConverter : IMultiValueConverter
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
            var colorString = values[0].ToString().ToLower();
            var typeString = values[1] == null ? "000000" : values[1].ToString().ToUpper();

            var strArr = new List<string>
            {
                "11B01",
                "11B03",
                "11B03",
                "11B04",
                "11B05",
                "11B06",
                "11B07",
                "11B08",
                "11B14",
                "11B15",
                "11B16",
                "11B17"
            };
            //switch (typeString)
            //{
            //    case "台风":
            //        typeString = "11B01";
            //        break;
            //    case "暴雨":
            //        typeString = "11B03";
            //        break;
            //    case "暴雪":
            //        typeString = "c";
            //        break;
            //    case "寒潮":
            //        typeString = "11b05";
            //        break;
            //    case "大风":
            //        typeString = "11b06";
            //        break;
            //    case "沙尘暴":
            //        typeString = "11B07";
            //        break;
            //    case "高温":
            //        typeString = "11B09";
            //        break;
            //    case "雷电":
            //        typeString = "11B14";
            //        break;
            //    case "冰雹":
            //        typeString = "11B15";
            //        break;
            //    case "霜冻":
            //        typeString = "11B16";
            //        break;
            //    case "大雾":
            //        typeString = "11B17";
            //        break;
            //    default:
            //        typeString = "00000";
            //        break;
            //}
            if (!strArr.Contains(typeString))
            {
                typeString = "00000";
            }

            var imagePath = "";
            if (string.IsNullOrWhiteSpace(colorString))
            {
                colorString = "Unknown";
                imagePath =
                    string.Format(
                        @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/EventPageLabel/{0}_{1}.png",
                        typeString.ToUpper(), colorString);
            }
            else
            {
                imagePath =
                    string.Format(
                        @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/EventPageLabel/{0}.png",
                        typeString.ToUpper());
            }

            var imageUri = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            var imageBitmap = new BitmapImage(imageUri);
            return imageBitmap;
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