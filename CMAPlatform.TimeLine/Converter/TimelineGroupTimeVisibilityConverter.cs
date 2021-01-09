using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CMAPlatform.TimeLine.Converter
{
    /// <summary>
    /// 泳道图日期显示转换器
    /// </summary>
    public class TimelineGroupTimeVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 正向转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime time = DateTime.Parse(value.ToString());

            if (time.Hour == 0 && time.Minute == 0)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 反向转换
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