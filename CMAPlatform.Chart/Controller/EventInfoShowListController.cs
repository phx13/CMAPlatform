using System.Collections.Generic;
using CMAPlatform.Chart.DVM;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    public class EventInfoShowListController : ChartControllerBase
    {
        private string m_CurEventInfo;
        private EventInfoDetailsDataViewModel m_Dvm;
        private List<string> m_EventInfoList = new List<string>();

        public EventInfoShowListController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_Dvm = DataViewModel as EventInfoDetailsDataViewModel;
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
        ///     轮播列表
        /// </summary>
        public List<string> EventInfoList
        {
            get { return m_EventInfoList; }
            set
            {
                m_EventInfoList = value;
                OnPropertyChanged("EventInfoList");
            }
        }

        /// <summary>
        ///     当前显示的轮播信息
        /// </summary>
        public string CurEventInfo
        {
            get { return m_CurEventInfo; }
            set
            {
                m_CurEventInfo = value;
                OnPropertyChanged("CurEventInfo");
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