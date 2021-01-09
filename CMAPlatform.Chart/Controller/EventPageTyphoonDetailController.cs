using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    /// <summary>
    ///     事件页台风详情按钮Controller
    /// </summary>
    public class EventPageTyphoonDetailController : ChartControllerBase
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public EventPageTyphoonDetailController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
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