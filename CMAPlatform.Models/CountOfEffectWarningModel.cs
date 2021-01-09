using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     全国预警生效总数
    /// </summary>
    public class CountOfEffectWarningModel : NotificationObject
    {
        private city m_city;

        private county m_county;

        private nation m_nation;
        private nationwide m_nationwide;

        private province m_province;

        /// <summary>
        ///     全国预警生效总数
        /// </summary>
        public nationwide nationwide
        {
            get { return m_nationwide; }
            set
            {
                m_nationwide = value;
                RaisePropertyChanged(() => nationwide);
            }
        }

        /// <summary>
        ///     国家级预警信息
        /// </summary>
        public nation nation
        {
            get { return m_nation; }
            set
            {
                m_nation = value;
                RaisePropertyChanged(() => nation);
            }
        }

        /// <summary>
        ///     省级
        /// </summary>
        public province province
        {
            get { return m_province; }
            set
            {
                m_province = value;
                RaisePropertyChanged(() => province);
            }
        }

        /// <summary>
        ///     市级
        /// </summary>
        public city city
        {
            get { return m_city; }
            set
            {
                m_city = value;
                RaisePropertyChanged(() => city);
            }
        }

        /// <summary>
        ///     县级
        /// </summary>
        public county county
        {
            get { return m_county; }
            set
            {
                m_county = value;
                RaisePropertyChanged(() => county);
            }
        }
    }

    /// <summary>
    ///     全国
    /// </summary>
    public class nationwide
    {
        /// <summary>
        ///     全国预警生效总数
        /// </summary>
        public string total { get; set; }
    }

    /// <summary>
    ///     国家级
    /// </summary>
    public class nation
    {
        /// <summary>
        ///     国家级生效预警总数
        /// </summary>
        public string total { get; set; }

        /// <summary>
        ///     国家级红色生效预警总数
        /// </summary>
        public string red { get; set; }

        /// <summary>
        ///     国家级橙色生效预警总数
        /// </summary>
        public string orange { get; set; }
    }

    /// <summary>
    ///     省级
    /// </summary>
    public class province
    {
        /// <summary>
        ///     国家级生效预警总数
        /// </summary>
        public string total { get; set; }

        /// <summary>
        ///     省级红色生效预警总数
        /// </summary>
        public string red { get; set; }

        /// <summary>
        ///     省级橙色生效预警总数
        /// </summary>
        public string orange { get; set; }
    }

    /// <summary>
    ///     市级
    /// </summary>
    public class city
    {
        /// <summary>
        ///     国家级生效预警总数
        /// </summary>
        public string total { get; set; }

        /// <summary>
        ///     省级红色生效预警总数
        /// </summary>
        public string red { get; set; }

        /// <summary>
        ///     省级橙色生效预警总数
        /// </summary>
        public string orange { get; set; }
    }

    /// <summary>
    ///     县级
    /// </summary>
    public class county
    {
        /// <summary>
        ///     国家级生效预警总数
        /// </summary>
        public string total { get; set; }

        /// <summary>
        ///     省级红色生效预警总数
        /// </summary>
        public string red { get; set; }

        /// <summary>
        ///     省级橙色生效预警总数
        /// </summary>
        public string orange { get; set; }
    }
}