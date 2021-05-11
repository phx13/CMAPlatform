using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     信息员、灾情上报情况
    /// </summary>
    public class XXYGeneralModel : NotificationObject
    {
        private List<xxyGeneral> m_data;

        /// <summary>
        ///     返回数据集合
        /// </summary>
        public List<xxyGeneral> data
        {
            get { return m_data; }
            set
            {
                m_data = value;
                RaisePropertyChanged(() => data);
            }
        }

        /// <summary>
        ///     接口调用错误信息
        /// </summary>
        public string errmgs { get; set; }

        /// <summary>
        ///     接口是否成功返回
        /// </summary>
        public string errcode { get; set; }
    }

    public class xxyGeneral
    {
        public int xxy { get; set; }

        public int zq { get; set; }

        public double jhl { get; set; }

        public double hyd { get; set; }
    }


    /// <summary>
    ///     单条预警的信息员发布情况
    /// </summary>
    public class xxyAboutEarlyWarning
    {
        public xxyAboutEarlyWarningDatum[] data { get; set; }
        public string errmgs { get; set; }
        public string errcode { get; set; }
    }

    /// <summary>
    ///     单条预警的信息员发布情况 - 数据
    /// </summary>
    public class xxyAboutEarlyWarningDatum
    {
        public string read { get; set; }
        public string send { get; set; }
        public string org { get; set; }
        public string jhl { get; set; }
        public string hyd { get; set; }
    }
}