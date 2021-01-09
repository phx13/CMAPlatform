using System;
using System.Globalization;
using System.Windows.Data;

namespace CMAPlatform.Chart.Converter
{
    public class TyphoonIntensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var label = value.ToString();
            var labelChinese = "";

            switch (label)
            {
                case "TD":
                    labelChinese = "热带低气压";
                    break;
                case "TS":
                    labelChinese = "热带风暴";
                    break;
                case "STS":
                    labelChinese = "强烈热带风暴";
                    break;
                case "TY":
                    labelChinese = "台风";
                    break;
                case "STY":
                    labelChinese = "强台风";
                    break;
                case "SuperTY":
                    labelChinese = "超强台风";
                    break;
            }

            return labelChinese + "（" + label + "）";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}