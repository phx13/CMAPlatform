using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CMAPlatform.DataClient;
using CMAPlatform.Models.TimeBarModel;
using CMAPlatform.Models.TimeLineModel;
using CMAPlatform.TimeLine.Window;
using Digihail.CCP.Utilities;
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;

namespace CMAPlatform.TimeLine.Controls
{
    /// <summary>
    ///     RealtimeRainControl.xaml 的交互逻辑
    /// </summary>
    public partial class RealtimeRainControl : UserControl
    {
        private TimeLineChartController m_Controller;

        public RealtimeRainControl()
        {
            InitializeComponent();

            m_Controller = DataContext as TimeLineChartController;
        }

        // TimeBar柱图点击事件
        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            if (DataContext == null) return;

            var controller = DataContext as TimeLineChartController;
            var s = ((sender as Border).DataContext as DataPoint).DataItem as CountInfo;
            LoadData(controller, s);

            var p = Mouse.GetPosition(Application.Current.MainWindow);

            var stationPopWindow = new StationPopWindow(controller)
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.Manual,
                Top = p.Y,
                Left = p.X,
                SnapsToDevicePixels = true
            };

            stationPopWindow.ShowDialog();
        }

        private void LoadData(TimeLineChartController controller, CountInfo countInfo)
        {
            controller.StationInfos.Clear();
            var color = "";
            if (DataManager.Instance.IsTestData)
            {
                var res = DataManager.getTimelineWarningStationPop("11B05", "", "");
                color = res.Key;
                controller.TimeLineStationDatas = res.Value;
            }
            else
            {
                var res = DataManager.getTimelineWarningStationPop(countInfo.EventType, countInfo.Stations,
                    countInfo.CountDateTime.ToString("yyyy-MM-dd HH"));
                color = res.Key;
                controller.TimeLineStationDatas = res.Value;
            }

            foreach (var timeLineStationData in controller.TimeLineStationDatas)
            {
                var station = new StationInfo
                {
                    Value = timeLineStationData.Value,
                    StationName = timeLineStationData.StationName,
                    StationId = timeLineStationData.StationId,
                    EventType = countInfo.EventType,
                    EventColor = color
                };
                controller.StationInfos.Add(station);
            }
        }

        /// <summary>
        ///     切换Y轴刻度
        /// </summary>
        public void ChangeYAxis()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += (sender, args) =>
            {
                dispatcherTimer.Stop();

                var range = (stackBarChart.VerticalAxis as LinearAxis).ActualRange;

                var tbmin = this.GetChildByName<TextBlock>("mintb");
                var tbmax = this.GetChildByName<TextBlock>("maxtb");

                if (tbmin != null && tbmax != null)
                {
                    tbmin.Text = string.Format("{0}站", range.Minimum);
                    tbmax.Text = string.Format("{0}站", range.Maximum);
                }
            };
            dispatcherTimer.Start();
        }
    }
}