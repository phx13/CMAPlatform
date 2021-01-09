using System.Windows;
using System.Windows.Controls;
using CMAPlatform.TestWindow.Controls;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Controls.TimeBar;

namespace CMAPlatform.TestWindow
{
    /// <summary>
    ///     TimeLineChart.xaml 的交互逻辑
    /// </summary>
    public partial class TimeLineChart : UserControl
    {
        #region 成员变量

        private TimeLineChartController m_Controller;

        #endregion

        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        public TimeLineChart()
        {
            InitializeComponent();
            Loaded += TimeLineChart_Loaded;
        }

        #endregion

        #region 私有函数

        private void ChangeToSingleBar(StationInfo station)
        {
            grid.Children.Clear();
            grid.Children.Add(new SingleRealtimeRain());

            returnButton.Visibility = Visibility.Visible;
            m_Controller.TimeBarTitle = string.Format("实况（{0}站降水）", station.StationName);
        }

        #endregion

        #region 控件事件

        // 窗体加载事件
        private void TimeLineChart_Loaded(object sender, RoutedEventArgs e)
        {
            var timeLineChartController = new TimeLineChartController();
            timeLineChartController.InitTestData();
            DataContext = timeLineChartController;

            m_Controller = timeLineChartController;

            //this.grid.Children.Add(new RealtimeRain());
            //this.grid.Children.Add(new WindLine());
            this.grid.Children.Add(new EmergencyMultiControl());

            //grid.Children.Add(new WindLineControl());

            m_Controller.TimeBarChangeCommand = new DelegateCommand<StationInfo>(ChangeToSingleBar);
        }

        // 返回按钮
        private void ReturnButton_OnClick(object sender, RoutedEventArgs e)
        {
            returnButton.Visibility = Visibility.Collapsed;
            m_Controller.TimeBarTitle = "实况（杭州市多站降水）";

            grid.Children.Clear();
            grid.Children.Add(new RealtimeRain());
        }

        // TimeLine控件时间间隔改变事件
        private void Timeline_OnItemIntervalChanged(object sender, DrillEventArgs e)
        {
        }

        #endregion
    }
}