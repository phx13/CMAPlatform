using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CMAPlatform.Models.Enum;
using CMAPlatform.Models.MessageModel;

namespace CMAPlatform.TimeLine.Controls
{
    /// <summary>
    /// TyphoonChart.xaml 的交互逻辑
    /// </summary>
    public partial class TyphoonChart : UserControl
    {
        #region 成员变量

        // 是否第一次加载
        private bool m_IsInit = true;

        // Controller
        private TimeLineChartController m_Controller;

        #endregion

        public TyphoonChart()
        {
            InitializeComponent();

            this.Loaded += TyphoonChart_Loaded;
        }

        private void TyphoonChart_Loaded(object sender, RoutedEventArgs e)
        {
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

            switch (chartSettingParameter.ChartName)
            {
                case ChartNameEnum.TyphoonWindChart:
                    this.windLinearAxis.Minimum = chartSettingParameter.MinimumValue - temp * 0.3;
                    this.windLinearAxis.Maximum = chartSettingParameter.MaximumValue + temp * 0.3;
                    break;
                case ChartNameEnum.TyphoonAirPressureChart:
                    this.pressLinearAxis.Maximum = chartSettingParameter.MaximumValue + temp * 0.05;
                    break;
            }
        }
    }
}
