using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CMAPlatform.Models.TimeLineModel;
using Telerik.Windows.Controls.Timeline;

namespace CMAPlatform.TimeLine.Converter
{
    public class TineLineHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "当前时间")
            {
                return 0;
            }
            return 60;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TineLineMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as TimeLineItem;
            if (item.GroupName == "当前时间")
            {
                return new Thickness(0, -2000, 0, -2000);
            }
            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TineLineVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as TimeLineItem;
            if (parameter.ToString() == "border1")
            {
                if (item.GroupName == "当前时间")
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
            if (item.GroupName == "当前时间")
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TimeLineItemColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)ColorConverter.ConvertFromString("#F59338");

            double opacity = 1;
            double.TryParse(parameter.ToString(), out opacity);

            var dataitem = value as TimelineDataItem;
            if (dataitem != null)
            {
                if (dataitem.Group.ToString() == "灾情舆情指数")
                {
                    color = (Color)ColorConverter.ConvertFromString("#F59338");
                }
                else if (dataitem.Group.ToString().StartsWith("服务"))
                {
                    color = (Color)ColorConverter.ConvertFromString("#00FFFF");
                }
                else if (dataitem.Group.ToString().StartsWith("预警"))
                {
                    if (dataitem.ToolTip != null)
                    {
                        switch (dataitem.ToolTip.ToString())
                        {
                            case "red":
                                color = (Color)ColorConverter.ConvertFromString("#C5364B");
                                break;
                            case "yellow":
                                color = (Color)ColorConverter.ConvertFromString("#D3AD14");
                                break;
                            case "orange":
                                color = (Color)ColorConverter.ConvertFromString("#DB6D00");
                                break;
                            case "blue":
                                color = (Color)ColorConverter.ConvertFromString("#01A9F1");
                                break;
                        }
                    }
                    else
                    {
                        color = (Color)ColorConverter.ConvertFromString("#01A9F1");
                    }
                }
            }

            var brush = new SolidColorBrush(color);
            brush.Opacity = opacity;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TimeLineItemColorConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)ColorConverter.ConvertFromString("#F59338");

            double opacity = 1;
            double.TryParse(parameter.ToString(), out opacity);

            var item = value as TimeLineItem;
            if (item != null)
            {
                if (item.GroupName == "灾情舆情指数")
                {
                    color = (Color)ColorConverter.ConvertFromString("#F59338");
                }
                else if (item.GroupName.StartsWith("服务"))
                {
                    color = (Color)ColorConverter.ConvertFromString("#00FFFF");
                }
                else if (item.GroupName.StartsWith("预警"))
                {
                    switch (item.Severity)
                    {
                        case "red":
                            color = (Color)ColorConverter.ConvertFromString("#C5364B");
                            break;
                        case "yellow":
                            color = (Color)ColorConverter.ConvertFromString("#D3AD14");
                            break;
                        case "orange":
                            color = (Color)ColorConverter.ConvertFromString("#DB6D00");
                            break;
                        case "blue":
                            color = (Color)ColorConverter.ConvertFromString("#01A9F1");
                            break;
                    }
                }
            }

            var brush = new SolidColorBrush(color);
            brush.Opacity = opacity;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TimeLineItemBorderHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dataitem = value as TimelineDataItem;
            var res = 0;
            if (dataitem != null)
            {
                if (dataitem.Group.ToString() == "灾情舆情指数" || dataitem.Group.ToString() == "服务-国家级")
                {
                    res = 7;
                }
                else
                {
                    res = 40;
                }
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TimeLineItemBorderWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dataitem = value as TimelineDataItem;
            var res = 0;
            if (dataitem != null)
            {
                if (dataitem.Group.ToString() == "灾情舆情指数" || dataitem.Group.ToString() == "服务-国家级")
                {
                    res = 7;
                }
                else
                {
                    res = 23;
                }
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TimeLineItemBorderImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dataitem = value as TimelineDataItem;
            ImageSource imageSource = new BitmapImage();

            if (dataitem != null)
            {
                if (dataitem.DataItem is TimeLineItem)
                {
                    var item = dataitem.DataItem as TimeLineItem;
                    if (item != null)
                    {
                        if (item.EventType != null)
                        {
                            if (dataitem.Group.ToString() == "灾情舆情指数" || dataitem.Group.ToString() == "服务-国家级")
                            {

                            }
                            else
                            {
                                if (item.EventType != "region")
                                {
                                    try
                                    {
                                        imageSource = new BitmapImage(new Uri(string.Format(
                                            @"pack://application:,,,/CMAPlatform.TimeLine;component/Resources/Images/TypeIcon/{0}.png",
                                            item.EventType)));
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}