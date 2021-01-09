using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CMAPlatform.Chart.Controller;
using CMAPlatform.Chart.Window;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     Prevention.xaml 的交互逻辑
    /// </summary>
    public partial class Prevention : ChartViewBase
    {
        private IMessageAggregator m_MessageAggregator = new MessageAggregator();
        private Storyboard m_storyboard;

        public Prevention(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            if (Controllers.FirstOrDefault() != null && Controllers.Count > 0)
            {
                if (Controllers.FirstOrDefault() is PreventionController)
                {
                    PreventionController = Controllers.FirstOrDefault() as PreventionController;
                    DataContext = PreventionController;
                }
            }

            Loaded += Prevention_Loaded;
        }

        public PreventionController PreventionController { get; set; }

        private void Prevention_Loaded(object sender, RoutedEventArgs e)
        {
            OnDadChartLoaded();
        }

        /// <summary>
        ///     刷新界面
        /// </summary>
        /// <param name="propertyDescription"></param>
        public override void RefreshStyle()
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        /// <summary>
        ///     接收数据（已废弃）
        /// </summary>
        /// <param name="adtList"></param>
        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        /// <summary>
        ///     接收联动消息选中
        /// </summary>
        /// <param name="selectedModel"></param>
        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }

        /// <summary>
        ///     接收联动消息取消
        /// </summary>
        /// <param name="clearModel"></param>
        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        /// <summary>
        ///     导出图表
        /// </summary>
        /// <param name="type"></param>
        public override void ExportChart(ExportType type)
        {
        }

        /// <summary>
        ///     点击打开详情窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreventionItem_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe != null)
            {
                var preventionMessage = new PreventionMessageWindow(fe.Tag.ToString())
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                preventionMessage.ShowDialog();
            }
        }
    }
}