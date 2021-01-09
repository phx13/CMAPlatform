using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Resource.Converter
{
    /// <summary>
    /// 数值到可见性转换器
    /// </summary>
    public class DoubleToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 正向转换（后台值转为UI显示）
        /// </summary>
        /// <param name="value">传入值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">传入参数</param>
        /// <param name="culture">格式信息</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double number;

            bool result = double.TryParse(value.ToString(), out number);

            return (result && !double.IsNaN(number) && Math.Abs(number) > 0) ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// 反向转换（UI显示转为后台值）
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