using System.Collections.Generic;
using System.ComponentModel;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     突发事件实体
    /// </summary>
    public class EventListModel : INotifyPropertyChanged
    {
        private List<Events> m_Events;

        public List<Events> Events
        {
            get { return m_Events; }
            set
            {
                m_Events = value;
                OnPropertyChanged("Events");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Events : INotifyPropertyChanged
    {
        private string m_Detail;

        private string m_EventType;

        private string m_ID;

        private bool m_IsHistory;

        private bool m_IsMouseOver;
        private bool m_IsPressed;

        private double m_Latitude;

        private string m_Location;

        private double m_Longitude;

        private string m_Name;

        private string m_ShowTitle;

        private string m_Source;

        private string m_Time;

        public bool IsPressed
        {
            get { return m_IsPressed; }
            set
            {
                m_IsPressed = value;
                OnPropertyChanged("IsPressed");
            }
        }

        public bool IsMouseOver
        {
            get { return m_IsMouseOver; }
            set
            {
                m_IsMouseOver = value;
                OnPropertyChanged("IsMouseOver");
            }
        }

        /// <summary>
        ///     突发事件唯一标识
        /// </summary>
        public string ID
        {
            get { return m_ID; }
            set
            {
                m_ID = value;
                OnPropertyChanged("ID");
            }
        }

        /// <summary>
        ///     事件名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        ///     事件类型
        /// </summary>
        public string EventType
        {
            get { return m_EventType; }
            set
            {
                m_EventType = value;
                OnPropertyChanged("EventType");
            }
        }

        /// <summary>
        ///     事件地点
        /// </summary>
        public string Location
        {
            get { return m_Location; }
            set
            {
                m_Location = value;
                OnPropertyChanged("Location");
            }
        }

        /// <summary>
        ///     经度
        /// </summary>
        public double Longitude
        {
            get { return m_Longitude; }
            set
            {
                m_Longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

        /// <summary>
        ///     纬度
        /// </summary>
        public double Latitude
        {
            get { return m_Latitude; }
            set
            {
                m_Latitude = value;
                OnPropertyChanged("Latitude");
            }
        }

        /// <summary>
        ///     标牌显示
        /// </summary>
        public string ShowTitle
        {
            get { return m_ShowTitle; }
            set
            {
                m_ShowTitle = value;
                OnPropertyChanged("ShowTitle");
            }
        }

        /// <summary>
        ///     发生时间
        /// </summary>
        public string Time
        {
            get { return m_Time; }
            set
            {
                m_Time = value;
                OnPropertyChanged("Time");
            }
        }

        /// <summary>
        ///     事件详情
        /// </summary>
        public string Detail
        {
            get { return m_Detail; }
            set
            {
                m_Detail = value;
                OnPropertyChanged("Detail");
            }
        }

        /// <summary>
        ///     信息来源
        /// </summary>
        public string Source
        {
            get { return m_Source; }
            set
            {
                m_Source = value;
                OnPropertyChanged("Source");
            }
        }

        /// <summary>
        ///     是否是历史事件
        /// </summary>
        public bool IsHistory
        {
            get { return m_IsHistory; }
            set
            {
                m_IsHistory = value;
                OnPropertyChanged("IsHistory");
            }
        }

        /// <summary>
        ///     结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        ///     省份区域代码
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        ///     市区域代码
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///     县区域代码
        /// </summary>
        public string Country { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


    /// <summary>
    ///     突发事件接口类
    /// </summary>
    public class RecEmergencise
    {
        /// <summary>
        ///     结果编码，0：成功，1：失败
        /// </summary>
        public string code { get; set; }

        public RecEmergenciseValue[] value { get; set; }
    }

    /// <summary>
    ///     突发事件接口类
    /// </summary>
    public class RecEmergenciseValue
    {
        /// <summary>
        ///     事件状态
        /// </summary>
        public string eventStatus { get; set; }

        /// <summary>
        ///     事件发生详情
        /// </summary>
        public string eventDescription { get; set; }

        /// <summary>
        ///     经度
        /// </summary>
        public string lon { get; set; }

        /// <summary>
        ///     事件来源
        /// </summary>
        public string from { get; set; }

        /// <summary>
        ///     事件发生地点
        /// </summary>
        public string place { get; set; }

        /// <summary>
        ///     事件发生时间
        /// </summary>
        public string time { get; set; }

        /// <summary>
        ///     事件主题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///     纬度
        /// </summary>
        public string lat { get; set; }

        /// <summary>
        ///     结束时间
        /// </summary>
        public string ctime { get; set; }

        /// <summary>
        ///     省份区域代码
        /// </summary>
        public string province { get; set; }

        /// <summary>
        ///     市区域代码
        /// </summary>
        public string city { get; set; }

        /// <summary>
        ///     县区域代码
        /// </summary>
        public string country { get; set; }
    }

    /// <summary>
    ///     突发事件入参
    /// </summary>
    public class EventListEnter
    {
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string Begintime { get; set; }
        public string endtime { get; set; }
        public string eventType { get; set; }
    }
    
    public class ClosestStation
    {
        public string stationId { get; set; }
        public string lon { get; set; }
        public string lat { get; set; }
        public string alti { get; set; }
        public string stationNa { get; set; }
    }
}