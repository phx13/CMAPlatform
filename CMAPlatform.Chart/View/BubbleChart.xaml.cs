using System.Collections.Generic;
using System.Windows;
using CMAPlatform.Chart.Controller;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     ItemsChart.xaml 的交互逻辑
    /// </summary>
    public partial class BubbleChart : ChartViewBase
    {
        public BubbleChart(ChartViewBaseModel chartViewBaseModel)
            : base(chartViewBaseModel)
        {
            InitializeComponent();
            Loaded += NameChart_Loaded;
        }

        private BubbleController m_NameController { get; set; }

        private void NameChart_Loaded(object sender, RoutedEventArgs e)
        {
            m_NameController = Controllers[0] as BubbleController;
            DataContext = m_NameController;
            OnDadChartLoaded();
            IsLoadingData = false;
        }


        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        public override void ExportChart(ExportType type)
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
            IsLoadingData = false;
        }

        public override void RefreshStyle()
        {
            IsLoadingData = false;
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }
    }
}