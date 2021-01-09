using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CMAPlatform.Models.Enum;
using CMAPlatform.Models.MessageModel;
using Digihail.CCP.Utilities;
using Microsoft.Practices.Prism.Events;

namespace CMAPlatform.TimeLine.Controls
{
    /// <summary>
    ///     EmergencyMultiControl.xaml 的交互逻辑
    /// </summary>
    public partial class EmergencyMultiControl : UserControl, IDisposable
    {
        #region 成员变量

        // 是否第一次加载
        private bool m_IsInit = true;

        // Controller
        private TimeLineChartController m_Controller;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public EmergencyMultiControl()
        {
            InitializeComponent();

            this.Loaded += EmergencyMultiControl_Loaded;
        }

        #endregion

        #region 私有方法

        // 控件加载事件
        private void EmergencyMultiControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsInit)
            {
                m_IsInit = false;
                m_Controller = this.DataContext as TimeLineChartController;
                if (m_Controller != null)
                    m_Controller.MessageAggregator.GetMessage<CMAChartSettingMessage>().Subscribe(SetLinearAxisPeakValue,ThreadOption.UIThread);
            }
        }

        /// <summary>
        /// 设置刻度极值
        /// </summary>
        /// <param name="chartSettingParameter">设置参数</param>
        private void SetLinearAxisPeakValue(ChartSettingParameter chartSettingParameter)
        {
            var temp = chartSettingParameter.MaximumValue - chartSettingParameter.MinimumValue;

            switch (chartSettingParameter.ChartName)
            {
                case ChartNameEnum.EmergencyRainChart:
                    this.rainLinearAxis.Maximum = chartSettingParameter.MaximumValue + 3;
                    break;
                case ChartNameEnum.EmergencyTemperatureChart:
                    this.temperatureLinearAxis.Minimum = chartSettingParameter.MinimumValue - temp * 0.3;
                    this.temperatureLinearAxis.Maximum = chartSettingParameter.MaximumValue + temp * 0.3;
                    break;
                case ChartNameEnum.EmergencyWindChart:
                    this.windLinearAxis.Minimum = chartSettingParameter.MinimumValue - temp * 0.2;
                    this.windLinearAxis.Maximum = chartSettingParameter.MaximumValue + temp * 0.3;
                    break;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        ///     切换Y轴刻度
        /// </summary>
        public void ChangeYAxis()
        {
            var dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            dispatcherTimer.Tick += (sender, args) =>
            {
                dispatcherTimer.Stop();

                var rangeTemp = this.temperatureLinearAxis.ActualRange;

                var tbminTemp = this.GetChildByName<TextBlock>("tempMin");
                var tbmaxTemp = this.GetChildByName<TextBlock>("tempMax");

                if (tbminTemp != null && tbmaxTemp != null)
                {
                    tbminTemp.Text = string.Format("{0}℃", rangeTemp.Minimum);
                    tbmaxTemp.Text = string.Format("{0}℃", rangeTemp.Maximum);
                }

                var rangeRain = this.rainLinearAxis.ActualRange;

                var tbminRain = this.GetChildByName<TextBlock>("rainMin");
                var tbmaxRain = this.GetChildByName<TextBlock>("rainMax");

                if (tbminRain != null && tbmaxRain != null)
                {
                    tbminRain.Text = string.Format("{0}mm", rangeRain.Minimum);
                    tbmaxRain.Text = string.Format("{0}mm", rangeRain.Maximum);
                }

                var rangeWind = this.windLinearAxis.ActualRange;

                var tbminWind = this.GetChildByName<TextBlock>("windMin");
                var tbmaxWind = this.GetChildByName<TextBlock>("windMax");

                if (tbminWind != null && tbmaxWind != null)
                {
                    tbminWind.Text = string.Format("{0}级", rangeWind.Minimum);
                    tbmaxWind.Text = string.Format("{0}级", rangeWind.Maximum);
                }
            };
            dispatcherTimer.Start();
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            m_Controller.MessageAggregator.GetMessage<CMAChartSettingMessage>().Unsubscribe(SetLinearAxisPeakValue);
        }

        #endregion
    }
}