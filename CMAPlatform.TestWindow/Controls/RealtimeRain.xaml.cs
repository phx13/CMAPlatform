using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CMAPlatform.TestWindow.泳道图弹窗;

namespace CMAPlatform.TestWindow.Controls
{
    /// <summary>
    ///     RealtimeRain.xaml 的交互逻辑
    /// </summary>
    public partial class RealtimeRain : UserControl
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public RealtimeRain()
        {
            InitializeComponent();
        }

        // TimeBar柱图点击事件
        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            if (DataContext == null) return;

            var controller = DataContext as TimeLineChartController;

            var stationPopWindow = new StationPopWindow(controller)
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.Manual
            };
            stationPopWindow.ShowDialog();
        }
    }
}