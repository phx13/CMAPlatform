using CMAPlatform.Chart.DVM;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    public class EventInfoDetailsController : ChartControllerBase
    {
        private string m_BrowseCount;
        private EventInfoDetailsDataViewModel m_Dvm;
        private string m_PredictionTime;
        private string m_TakeEffectTime;
        private string m_TitleAddress;
        private string m_TitleName;
        private string m_TitleType;
        private string m_TransmitCount;

        public EventInfoDetailsController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_Dvm = DataViewModel as EventInfoDetailsDataViewModel;
        }

        public IMessageAggregator MessageAggregator
        {
            get { return new MessageAggregator(); } 
        }

        /// <summary>
        ///     DVM
        /// </summary>
        public EventInfoDetailsDataViewModel DVM
        {
            get { return m_Dvm; }
            set
            {
                m_Dvm = value;
                OnPropertyChanged("DVM");
            }
        }

        /// <summary>
        ///     事件详情标题名
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

        /// <summary>
        ///     标题类型
        /// </summary>
        public string TitleType
        {
            get { return m_TitleType; }
            set
            {
                m_TitleType = value;
                OnPropertyChanged("TitleType");
            }
        }

        /// <summary>
        ///     标题地址
        /// </summary>
        public string TitleAddress
        {
            get { return m_TitleAddress; }
            set
            {
                m_TitleAddress = value;
                OnPropertyChanged("TitleAddress");
            }
        }

        /// <summary>
        ///     预警时间
        /// </summary>
        public string PredictionTime
        {
            get { return m_PredictionTime; }
            set
            {
                m_PredictionTime = value;
                OnPropertyChanged("PredictionTime");
            }
        }

        /// <summary>
        ///     生效时间
        /// </summary>
        public string TakeEffectTime
        {
            get { return m_TakeEffectTime; }
            set
            {
                m_TakeEffectTime = value;
                OnPropertyChanged("TakeEffectTime");
            }
        }

        /// <summary>
        ///     浏览人数
        /// </summary>
        public string BrowseCount
        {
            get { return m_BrowseCount; }
            set
            {
                m_BrowseCount = value;
                OnPropertyChanged("BrowseCount");
            }
        }

        /// <summary>
        ///     转发人数
        /// </summary>
        public string TransmitCount
        {
            get { return m_TransmitCount; }
            set
            {
                m_TransmitCount = value;
                OnPropertyChanged("TransmitCount");
            }
        }

        public override void ReceiveData(AdapterDataTable adt)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }
    }
}