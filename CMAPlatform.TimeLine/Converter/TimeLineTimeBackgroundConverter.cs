using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CMAPlatform.TimeLine.Converter
{
    /// <summary>
    /// 泳道图顶部时间背景色转换器
    /// </summary>
    public class TimeLineTimeBackgroundConverter : IMultiValueConverter
    {
        /// <summary>
        /// 正向转换
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return null;
            if (values[0] == null) return null;
            if (values[1] == null) return null;

            SolidColorBrush solidColorBrush = null;

            DateTime actualTime;

            bool flag = DateTime.TryParse(values[0].ToString(), out actualTime);
            if (flag == false) return null;

            DateTime typhoonTime = DateTime.Parse(values[1].ToString());

            if (actualTime != typhoonTime || typhoonTime == DateTime.Today.AddDays(1))
            {
                solidColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#044A6C"))
                {
                    Opacity = 0.8
                };
            }
            else
            {
                solidColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4497E3"))
                {
                    Opacity = 1
                };
            }

            return solidColorBrush;
        }

        /// <summary>
        /// 反向转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}