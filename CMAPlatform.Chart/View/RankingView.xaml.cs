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
    ///     RankingView.xaml 的交互逻辑
    /// </summary>
    public partial class RankingView : ChartViewBase
    {
        public RankingController controller;

        public RankingView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            controller = (RankingController) Controllers[0];
            DataContext = controller;
        }

        private void RankingView_OnLoaded(object sender, RoutedEventArgs e)
        {
            OnDadChartLoaded();
        }

        #region override Method

        public override void RefreshStyle()
        {
            //throw new NotImplementedException();
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
            //throw new NotImplementedException();
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
            //throw new NotImplementedException();
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
            //throw new NotImplementedException();
        }

        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
            //throw new NotImplementedException();
        }

        public override void ExportChart(ExportType type)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}