using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models.TimeLineModel
{
    public class TimeLineItem : NotificationObject
    {
        private TimeSpan m_Duration;

        private string m_GroupName;

        private string m_Id;
        private string m_Severity;
        private string m_EventType;

        private ObservableCollection<TimeLineItem> m_InculdeItems = new ObservableCollection<TimeLineItem>();

        private bool m_IsSelected;

        private string m_Name;

        private string m_ServiceUrl;
        private DateTime m_StartTime;

        /// <summary>
        ///     开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return m_StartTime; }
            set
            {
                m_StartTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        /// <summary>
        ///     开始时间
        /// </summary>
        public TimeSpan Duration
        {
            get { return m_Duration; }
            set
            {
                m_Duration = value;
                OnPropertyChanged("Duration");
            }
        }

        /// <summary>
        ///     类型
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
        ///     预警强度
        /// </summary>
        public string Severity
        {
            get { return m_Severity; }
            set
            {
                m_Severity = value;
                OnPropertyChanged("Severity");
            }
        }

        /// <summary>
        ///     ID
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set
            {
                m_Id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        ///     名称
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
        ///     组名
        /// </summary>
        public string GroupName
        {
            get { return m_GroupName; }
            set
            {
                m_GroupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        /// <summary>
        ///     文件路径
        /// </summary>
        public string ServiceUrl
        {
            get { return m_ServiceUrl; }
            set
            {
                m_ServiceUrl = value;
                OnPropertyChanged("ServiceUrl");
            }
        }

        /// <summary>
        ///     是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return m_IsSelected; }
            set
            {
                m_IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        ///     聚组
        /// </summary>
        public ObservableCollection<TimeLineItem> InculdeItems
        {
            get { return m_InculdeItems; }
            set
            {
                m_InculdeItems = value;
                OnPropertyChanged("InculdeItems");
            }
        }

        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }


    /// <summary>
    ///     按类型统计信息
    /// </summary>
    public class CountTypeInfo
    {
        /// <summary>
        ///     统计类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     统计类型下的统计信息
        /// </summary>
        public ObservableCollection<CountInfo> CountInfos { get; set; }
    }

    /// <summary>
    ///     统计类
    /// </summary>
    public class CountInfo
    {
        /// <summary>
        ///     统计时间
        /// </summary>
        public DateTime CountDateTime { get; set; }

        /// <summary>
        ///     统计值
        /// </summary>
        public int CountValue { get; set; }

        /// <summary>
        ///     预警类型
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        ///     观测站集合
        /// </summary>
        public string Stations { get; set; }
    }

    /// <summary>
    ///     最大降雨量统计类
    /// </summary>
    public class MaxCountInfo
    {
        /// <summary>
        ///     统计时间
        /// </summary>
        public DateTime CountDateTime { get; set; }

        /// <summary>
        ///     单位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     降水值
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        ///     降水值
        /// </summary>
        public int Value
        {
            get { return 1; }
        }
    }
}