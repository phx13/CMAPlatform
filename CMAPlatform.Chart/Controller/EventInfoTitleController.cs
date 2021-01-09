using CMAPlatform.Chart.DVM;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    public class EventInfoTitleController : ChartControllerBase
    {
        private EventInfoTitleDataViewModel m_Dvm;
        private string m_TitleInfo;
        private string m_TitleName;

        public EventInfoTitleController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_Dvm = DataViewModel as EventInfoTitleDataViewModel;
        }

        public IMessageAggregator MessageAggregator
        {
            get { return new MessageAggregator(); } 
        } 

        /// <summary>
        ///     DVM
        /// </summary>
        public EventInfoTitleDataViewModel DVM
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
        ///     事件详情标题信息
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