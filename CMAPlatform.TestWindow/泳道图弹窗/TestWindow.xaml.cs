using System.Windows;

namespace CMAPlatform.TestWindow
{
    /// <summary>
    ///     TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            var timeLineChartController = new TimeLineChartController();
            timeLineChartController.InitTestData();

            DataContext = timeLineChartController.CurrentAlarmDetail;
        }
    }
}