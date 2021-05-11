using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     极端天气
    /// </summary>
    public class ExemtremeWeatherModel : NotificationObject
    {
        private bool m_IsCheck;

        private string m_Time;

        private int m_WeatherCount;

        private string m_WeatherElement;

        private List<Weathers> m_Weathers;

        /// <summary>
        ///     是否选中
        /// </summary>
        public bool IsCheck
        {
            get { return m_IsCheck; }
            set
            {
                m_IsCheck = value;
                RaisePropertyChanged(() => IsCheck);
            }
        }

        /// <summary>
        ///     气象要素
        /// </summary>
        public string WeatherElement
        {
            get { return m_WeatherElement; }
            set
            {
                m_WeatherElement = value;
                RaisePropertyChanged(() => WeatherElement);
            }
        }

        /// <summary>
        ///     气象要素数量
        /// </summary>
        public int WeatherCount
        {
            get { return m_WeatherCount; }
            set
            {
                m_WeatherCount = value;
                RaisePropertyChanged(() => WeatherCount);
            }
        }

        /// <summary>
        ///     时间
        /// </summary>
        public string Time
        {
            get { return m_Time; }
            set
            {
                m_Time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        public List<Weathers> Weathers
        {
            get { return m_Weathers; }
            set
            {
                m_Weathers = value;
                RaisePropertyChanged(() => Weathers);
            }
        }
    }

    public class Weathers : NotificationObject
    {
        private string m_ID;

        private string m_ShowWeather;

        private string m_Station;

        private double m_StationHeight;

        private double m_StationLat;

        private double m_StationLon;

        private string m_Time;

        private string m_WeatherElement;

        /// <summary>
        ///     唯一标识
        /// </summary>
        public string ID
        {
            get { return m_ID; }
            set
            {
                m_ID = value;
                RaisePropertyChanged(() => ID);
            }
        }

        /// <summary>
        ///     极值站
        /// </summary>
        public string Station
        {
            get { return m_Station; }
            set
            {
                m_Station = value;
                RaisePropertyChanged(() => Station);
            }
        }

        /// <summary>
        ///     极值站经度
        /// </summary>
        public double StationLon
        {
            get { return m_StationLon; }
            set
            {
                m_StationLon = value;
                RaisePropertyChanged(() => StationLon);
            }
        }

        /// <summary>
        ///     极值站纬度
        /// </summary>
        public double StationLat
        {
            get { return m_StationLat; }
            set
            {
                m_StationLat = value;
                RaisePropertyChanged(() => StationLat);
            }
        }

        /// <summary>
        ///     极值站高度
        /// </summary>
        public double StationHeight
        {
            get { return m_StationHeight; }
            set
            {
                m_StationHeight = value;
                RaisePropertyChanged(() => StationHeight);
            }
        }

        /// <summary>
        ///     气象要素
        /// </summary>
        public string WeatherElement
        {
            get { return m_WeatherElement; }
            set
            {
                m_WeatherElement = value;
                RaisePropertyChanged(() => WeatherElement);
            }
        }

        /// <summary>
        ///     显示的气象数值
        /// </summary>
        public string ShowWeather
        {
            get { return m_ShowWeather; }
            set
            {
                m_ShowWeather = value;
                RaisePropertyChanged(() => ShowWeather);
            }
        }

        /// <summary>
        ///     时间
        /// </summary>
        public string Time
        {
            get { return m_Time; }
            set
            {
                m_Time = value;
                RaisePropertyChanged(() => Time);
            }
        }
    }

    public class ExemtremeWeather
    {
        /// <summary>
        ///     气象要素
        /// </summary>
        public string WeatherElement { get; set; }

        /// <summary>
        ///     气象要素数量
        /// </summary>
        public int WeatherCount { get; set; }

        /// <summary>
        ///     时间
        /// </summary>
        public string Time { get; set; }

        public Weathers[] Weathers { get; set; }
    }
}