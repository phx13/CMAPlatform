using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Digihail.CCP.Utilities;
using Telerik.Windows.Controls.ChartView;
using CMAPlatform.Models.MessageModel;
using CMAPlatform.Models.Enum;

namespace CMAPlatform.TimeLine.Controls
{
    /// <summary>
    ///     WindLine.xaml 的交互逻辑
    /// </summary>
    public partial class WindLine : UserControl
    {

        // 是否第一次加载
        private bool m_IsInit = true;

        // Controller
        private TimeLineChartController m_Controller;
        public WindLine()
        {
            InitializeComponent();

            Loaded += WindLine_Loaded;
        }

        private void WindLine_Loaded(object sender, RoutedEventArgs e)
        {
            //ChangeYAxis();
            if (m_IsInit)
            {
                m_IsInit = false;
                m_Controller = this.DataContext as TimeLineChartController;
                if (m_Controller != null)
                    m_Controller.MessageAggregator.GetMessage<CMAChartSettingMessage>().Subscribe(SetLinearAxisPeakValue);
            }
        }


        /// <summary>
        /// 设置刻度极值
        /// </summary>
        /// <param name="chartSettingParameter">设置参数</param>
        private void SetLinearAxisPeakValue(ChartSettingParameter chartSettingParameter)
        {
            var temp = chartSettingParameter.MaximumValue - chartSettingParameter.MinimumValue;
            
                    this.windLinearAxis.Minimum = chartSettingParameter.MinimumValue - temp * 0.3;
                    this.windLinearAxis.Maximum = chartSettingParameter.MaximumValue + temp * 0.3;
        }
        public void Dispose()
        {
            m_Controller.MessageAggregator.GetMessage<CMAChartSettingMessage>().Unsubscribe(SetLinearAxisPeakValue);
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

                var range = (WindChart.VerticalAxis as LinearAxis).ActualRange;

                var tbmin = this.GetChildByName<TextBlock>("mintb");
                var tbmax = this.GetChildByName<TextBlock>("maxtb");

                if (tbmin != null && tbmax != null)
                {
                    tbmin.Text = string.Format("{0}级", range.Minimum);
                    tbmax.Text = string.Format("{0}级", range.Maximum);
                }
            };
            dispatcherTimer.Start();
        }
    }
}