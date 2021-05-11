using CMAPlatform.Models;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    /// <summary>
    ///     台风详细Controller
    /// </summary>
    public class TyphoonDetailController : ChartControllerBase
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public TyphoonDetailController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
        }

        #endregion

        #region 台风数据

        private TyphoonDataInfoModel m_TyphoonData = new TyphoonDataInfoModel();

        /// <summary>
        ///     台风数据
        /// </summary>
        public TyphoonDataInfoModel TyphoonData
        {
            get { return m_TyphoonData; }
            set
            {
                m_TyphoonData = value;
                OnPropertyChanged("TyphoonData");
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