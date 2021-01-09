using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models
{
    public class ProventionMessageModel : NotificationObject
    {
        private List<Indicators> m_Indicators;

        private List<Indicators> m_message;

        private double m_percent;

        private double m_Percent;

        private string m_province;
        private string m_ProvinceName;

        /// <summary>
        ///     省份名称
        /// </summary>
        public string ProvinceName
        {
            get { return m_ProvinceName; }
            set
            {
                m_ProvinceName = value;
                RaisePropertyChanged(() => ProvinceName);
            }
        }

        /// <summary>
        ///     接口省份名称
        /// </summary>
        public string province
        {
            get { return m_province; }
            set
            {
                m_province = value;
                RaisePropertyChanged(() => province);
                ProvinceName = province;
            }
        }

        /// <summary>
        ///     省份占比值
        /// </summary>
        public double Percent
        {
            get { return m_Percent; }
            set
            {
                m_Percent = value;
                RaisePropertyChanged(() => Percent);
            }
        }

        /// <summary>
        ///     接口省份占比值
        /// </summary>
        public double percent
        {
            get { return m_percent; }
            set
            {
                m_percent = value;
                Percent = value;
                RaisePropertyChanged(() => percent);
            }
        }

        /// <summary>
        ///     省份指标
        /// </summary>
        public List<Indicators> Indicators
        {
            get { return m_Indicators; }
            set
            {
                m_Indicators = value;
                RaisePropertyChanged(() => Indicators);
            }
        }

        public List<Indicators> message
        {
            get { return m_message; }
            set
            {
                m_message = value;
                Indicators = value;
                RaisePropertyChanged(() => message);
            }
        }

        private string m_ProvenceUrl;
        /// <summary>
        /// 省份网址
        /// </summary>
        public string ProvenceUrl
        {
            get { return m_ProvenceUrl; }
            set
            {
                m_ProvenceUrl = value;
                RaisePropertyChanged("ProvenceUrl");
            }
        }
    }

    /// <summary>
    ///     省份指标
    /// </summary>
    public class Indicators : NotificationObject
    {
        private List<SubIndicators> m_childMessageList;

        private string m_disasterName;

        private bool m_exist;

        private bool m_HaveOrNot;
        private string m_Name;

        private List<SubIndicators> m_SubIndicators;

        /// <summary>
        ///     指标名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /// <summary>
        ///     接口指标名称
        /// </summary>
        public string disasterName
        {
            get { return m_disasterName; }
            set
            {
                m_disasterName = value;
                Name = value;
                RaisePropertyChanged(() => disasterName);
            }
        }

        /// <summary>
        ///     是否具有
        /// </summary>
        public bool HaveOrNot
        {
            get { return m_HaveOrNot; }
            set
            {
                m_HaveOrNot = value;
                RaisePropertyChanged(() => HaveOrNot);
            }
        }

        /// <summary>
        ///     接口是否具有
        /// </summary>
        public bool exist
        {
            get { return m_exist; }
            set
            {
                m_exist = value;
                HaveOrNot = value;
                RaisePropertyChanged(() => exist);
            }
        }

        /// <summary>
        ///     二级指标
        /// </summary>
        public List<SubIndicators> SubIndicators
        {
            get { return m_SubIndicators; }
            set
            {
                m_SubIndicators = value;
                RaisePropertyChanged(() => SubIndicators);
            }
        }

        /// <summary>
        ///     接口二级指标
        /// </summary>
        public List<SubIndicators> childMessageList
        {
            get { return m_childMessageList; }
            set
            {
                m_childMessageList = value;
                SubIndicators = value;
                RaisePropertyChanged(() => childMessageList);
            }
        }
    }

    /// <summary>
    ///     二级指标
    /// </summary>
    public class SubIndicators : NotificationObject
    {
        private string m_disasterName;

        private bool m_exist;

        private bool m_HaveOrNot;
        private string m_Name;

        /// <summary>
        ///     指标名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /// <summary>
        ///     接口指标名称
        /// </summary>
        public string disasterName
        {
            get { return m_disasterName; }
            set
            {
                m_disasterName = value;
                Name = value;
                RaisePropertyChanged(() => disasterName);
            }
        }

        /// <summary>
        ///     是否具有
        /// </summary>
        public bool HaveOrNot
        {
            get { return m_HaveOrNot; }
            set
            {
                m_HaveOrNot = value;
                RaisePropertyChanged(() => HaveOrNot);
            }
        }

        /// <summary>
        ///     接口是否具有
        /// </summary>
        public bool exist
        {
            get { return m_exist; }
            set
            {
                m_exist = value;
                HaveOrNot = value;
                RaisePropertyChanged(() => exist);
            }
        }
    }
}