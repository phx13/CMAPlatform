using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class RiskDictionaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var values = "台风事件";
            if (value != null)
            {
                switch (value.ToString())
                {
                    case "11B03":
                        values = "暴雨事件";
                        break;
                    case "11B04":
                        values = "暴雪事件";
                        break;
                    case "11B05":
                        values = "寒潮事件";
                        break;
                    case "11B06":
                        values = "大风事件";
                        break;
                    case "11B07":
                        values = "沙尘暴";
                        break;
                    case "11B09":
                        values = "高温事件";
                        break;
                    case "11B14":
                        values = "雷电事件";
                        break;
                    case "11B15":
                        values = "冰雹事件";
                        break;
                    case "11B16":
                        values = "霜冻事件";
                        break;
                    case "11B17":
                        values = "大雾事件";
                        break;
                    case "11B19":
                        values = "霾事件";
                        break;
                    case "11B21":
                        values = "道路结冰";
                        break;
                    case "11B22":
                        values = "干旱事件";
                        break;
                }
            }
            return values;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}