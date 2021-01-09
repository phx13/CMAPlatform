using CMAPlatform.Chart.DVM;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    public class TimerAutomaticController : ChartControllerBase
    {
        private TimerAutomaticDataViewModel m_DataViewModel;

        public TimerAutomaticController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DataViewModel = dvm as TimerAutomaticDataViewModel;
        }

        /// <summary>
        ///     DVM
        /// </summary>
        public TimerAutomaticDataViewModel DataViewModel
        {
            get { return m_DataViewModel; }
            set
            {
                m_DataViewModel = value;
                OnPropertyChanged("DataViewModel");
            }
        }


        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        public override void ReceiveData(AdapterDataTable adt)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }
    }
}