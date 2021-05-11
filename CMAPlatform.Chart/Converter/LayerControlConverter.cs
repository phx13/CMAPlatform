using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    /// <summary>
    ///     图层显隐控转换器
    /// </summary>
    public class LayerControlConverter : IMultiValueConverter
    {
        /// <summary>
        ///     正向转换
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var currentLayer = values[0].ToString();
            var senderLayer = values[1].ToString();

            if (currentLayer == senderLayer)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     反向转换
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