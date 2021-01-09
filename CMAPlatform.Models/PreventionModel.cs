using System;
using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models
{
    public class PreventionModel : NotificationObject
    {
        private int m_id;

        private string m_percent;


        private string m_province;

        private string m_ProvinceName;

        private string m_ShortName;

        private DateTime m_Time;

        private double m_Value;

        //省份ID
        public int id
        {
            get { return m_id; }
            set
            {
                m_id = value;
                RaisePropertyChanged(() => id);
            }
        }

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
        ///     短名称
        /// </summary>
        public string ShortName
        {
            get { return m_ShortName; }
            set
            {
                m_ShortName = value;
                RaisePropertyChanged(() => ShortName);
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
                ProvinceName = value;
                RaisePropertyChanged(() => province);
            }
        }

        /// <summary>
        ///     接口省份数值
        /// </summary>
        public string percent
        {
            get { return m_percent; }
            set
            {
                m_percent = value;
                Value = Convert.ToDouble(value);
                RaisePropertyChanged(() => percent);
            }
        }

        /// <summary>
        ///     省份值
        /// </summary>
        public double Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        /// <summary>
        ///     时间
        /// </summary>
        public DateTime Time
        {
            get { return m_Time; }
            set
            {
                m_Time = value;
                RaisePropertyChanged(() => Time);
            }
        }
    }
}