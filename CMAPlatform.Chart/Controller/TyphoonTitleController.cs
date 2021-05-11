using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    public class TyphoonTitleController : ChartControllerBase
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public TyphoonTitleController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
        }

        #endregion

        #region 公共属性

        private string m_TitleName;

        /// <summary>
        ///     标题名称
        /// </summary>
        public string TitleName
        {
            get { return m_TitleName; }
            set
            {
                m_TitleName = value;
                OnPropertyChanged("TitleName");
            }
        }

        private string m_TitleInfo;

        /// <summary>
        ///     标题详细
        /// </summary>
        public string TitleInfo
        {
            get { return m_TitleInfo; }
            set
            {
                m_TitleInfo = value;
                OnPropertyChanged("TitleInfo");
            }
        }

        private string m_Time;

        public string Time
        {
            get { return m_Time; }
            set
            {
                m_Time = value;
                OnPropertyChanged("Time");
            }
        }

        private string m_Severity;

        public string Severity
        {
            get { return m_Severity; }
            set
            {
                m_Severity = value;
                OnPropertyChanged("Severity");
            }
        }

        #endregion

        #region 重写基类方法

        public override void ReceiveData(AdapterDataTable adt)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        #endregion
    }
}