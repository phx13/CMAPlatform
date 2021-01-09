using System.Collections.ObjectModel;
using CMAPlatform.Chart.DVM;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    public class EventListController : ChartControllerBase
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public EventListController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DataViewModel = dvm as EventListDataViewModel;
            RiskCountCollection = DataManager.Instance.RiskSituationModels;
        }

        #endregion

        #region 公共属性

        private EventListDataViewModel m_DataViewModel;

        /// <summary>
        ///     DVM
        /// </summary>
        public EventListDataViewModel DataViewModel
        {
            get { return m_DataViewModel; }
            set
            {
                m_DataViewModel = value;
                OnPropertyChanged("DataViewModel");
            }
        }

        private ObservableCollection<RiskSituationModel> m_RiskCountCollection =
            new ObservableCollection<RiskSituationModel>();

        /// <summary>
        ///     风险态势按类型统计集合
        /// </summary>
        public ObservableCollection<RiskSituationModel> RiskCountCollection
        {
            get { return m_RiskCountCollection; }
            set
            {
                m_RiskCountCollection = value;
                OnPropertyChanged("RiskCountCollection");
            }
        }

        private RiskSituationModel m_CurrentSelectedRiskType = new RiskSituationModel();

        /// <summary>
        ///     当前选中的预警类型
        /// </summary>
        public RiskSituationModel CurrentSelectedRiskType
        {
            get { return m_CurrentSelectedRiskType; }
            set
            {
                m_CurrentSelectedRiskType = value;
                OnPropertyChanged("CurrentSelectedRiskType");
            }
        }

        #endregion

        #region 实现基类方法

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        public override void ReceiveData(AdapterDataTable adt)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        #endregion
    }
}