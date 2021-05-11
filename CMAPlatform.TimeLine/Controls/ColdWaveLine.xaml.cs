using System;
using System.Windows;
using System.Windows.Controls;
using CMAPlatform.Models.Enum;
using CMAPlatform.Models.MessageModel;
using Microsoft.Practices.Prism.Commands;

namespace CMAPlatform.TimeLine.Controls
{
    /// <summary>
    ///     WindLine.xaml 的交互逻辑
    /// </summary>
    public partial class ColdWaveLine : UserControl, IDisposable
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
        public ColdWaveLine()
        {
            InitializeComponent();

            this.Loaded += ColdWaveLine_Loaded;
        }

        #endregion

        #region 私有方法

        // 控件加载事件
        private void ColdWaveLine_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsInit)
            {
                m_IsInit = false;
                m_Controller = this.DataContext as TimeLineChartController;
                if (m_Controller != null)
                    m_Controller.MessageAggregator.GetMessage<CMAChartSettingMessage>().Subscribe(SetLinearAxisPeakValue);
                m_Controller.ColdWaveLineChangeCommand =
                    new DelegateCommand<string>(SelectButton);
            }
        }

        private void SelectButton(string tag)
        {
            switch (tag)
            {
                case "48":
                    button48.IsChecked = false;
                    button48.IsChecked = true;
                    break;
                case "24":
                    button24.IsChecked = false;
                    button24.IsChecked = true;
                    break;
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
                case ChartNameEnum.ColdWaveTemperatureChart:
                    this.temperatureLinearAxis.Minimum = chartSettingParameter.MinimumValue - temp * 0.3;
                    this.temperatureLinearAxis.Maximum = chartSettingParameter.MaximumValue + temp * 0.3;
                    break;
                case ChartNameEnum.ColdWaveWindChart:
                    this.windLinearAxis.Minimum = chartSettingParameter.MinimumValue - temp * 0.2;
                    this.windLinearAxis.Maximum = chartSettingParameter.MaximumValue + temp * 0.3;
                    break;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            m_Controller.MessageAggregator.GetMessage<CMAChartSettingMessage>().Unsubscribe(SetLinearAxisPeakValue);
        }

        #endregion

        private void TimeSpanChanged(object sender, RoutedEventArgs e)
        {
            var tag = (sender as RadioButton).Tag.ToString();
            m_Controller.LoadColdWaveLineData(m_Controller.CurrentStationInfo, tag);
        }
    }
}