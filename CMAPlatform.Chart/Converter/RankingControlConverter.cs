using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CMAPlatform.Chart.Converter
{
    public class RankingControlConverter
    {
    }

    /// <summary>
    ///     排名
    /// </summary>
    public class RankingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ranking = "";
            try
            {
                if (value != null)
                {
                    var _value = int.Parse(value.ToString()) + 1;
                    if (_value < 10)
                    {
                        ranking = "0" + _value;
                    }
                    else
                    {
                        ranking = _value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ranking;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    #region 排名背景图片

    /// <summary>
    ///     排名背景图片
    /// </summary>
    public class BorderBgConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = new ImageBrush();
            try
            {
                if (value != null)
                {
                    var _value = int.Parse(value.ToString()) + 1;
                    switch (_value)
                    {
                        case 1:
                            image.ImageSource =
                                new BitmapImage(
                                    new Uri(
                                        @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/One.png",
                                        UriKind.RelativeOrAbsolute));
                            break;
                        case 2:
                            image.ImageSource =
                                new BitmapImage(
                                    new Uri(
                                        @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/Two.png",
                                        UriKind.RelativeOrAbsolute));

                            break;
                        default:
                            image.ImageSource =
                                new BitmapImage(
                                    new Uri(
                                        @"pack://application:,,,/CMAPlatform.Chart;component/Resouces/Images/Other.png",
                                        UriKind.RelativeOrAbsolute));

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    #endregion

    public class TextBgColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bgColor = "";
            try
            {
                var _color = value.ToString();
                var _tmp = _color.Substring(1, _color.Length - 1);
                if (_color != "#000000")
                {
                    bgColor = "#7F" + _tmp;
                }
                else
                {
                    bgColor = "#00" + _tmp;
                }
            }
            catch (Exception ex)
            {
            }

            return bgColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}