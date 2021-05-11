using System.Collections.Generic;
using System.Windows;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CMAPlatform.TimeLine
{
    /// <summary>
    ///     UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class MainChartView : ChartViewBase
    {
        private TimeLineChartController m_Controller;

        private bool m_Init;

        public MainChartView(ChartViewBaseModel model) : base(model)
        {
            InitializeComponent();
            Loaded += MainChartView_Loaded;
        }

        private void MainChartView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!m_Init)
            {
                m_Controller = Controllers[0] as TimeLineChartController;
                DataContext = m_Controller;
                OnDadChartLoaded();
                m_Init = true;
            }
        }

        #region override

        public override void RefreshStyle()
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }

        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        public override void ExportChart(ExportType type)
        {
        }

        #endregion
    }
}