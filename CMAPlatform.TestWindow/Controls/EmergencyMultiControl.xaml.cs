using System.Windows.Controls;

namespace CMAPlatform.TestWindow.Controls
{
    /// <summary>
    ///     EmergencyMultiControl.xaml 的交互逻辑
    /// </summary>
    public partial class EmergencyMultiControl : UserControl
    {
        public EmergencyMultiControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 切换Y轴刻度
        /// </summary>
        //public void ChangeYAxis()
        //{
        //    DispatcherTimer dispatcherTimer = new DispatcherTimer();
        //    dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        //    dispatcherTimer.Tick += (sender, args) =>
        //    {
        //        dispatcherTimer.Stop();

        //        var rangeTemp = (TempChart.VerticalAxis as LinearAxis).ActualRange;

        //        var tbminTemp = VisualTreeHelperExtension.GetChildByName<TextBlock>(this, "tempMin");
        //        var tbmaxTemp = VisualTreeHelperExtension.GetChildByName<TextBlock>(this, "tempMax");

        //        if (tbminTemp != null && tbmaxTemp != null)
        //        {
        //            tbminTemp.Text = string.Format("{0}℃", rangeTemp.Minimum);
        //            tbmaxTemp.Text = string.Format("{0}℃", rangeTemp.Maximum);
        //        }

        //        var rangeRain = (TempChart.VerticalAxis as LinearAxis).ActualRange;

        //        var tbminRain = VisualTreeHelperExtension.GetChildByName<TextBlock>(this, "rainMin");
        //        var tbmaxRain = VisualTreeHelperExtension.GetChildByName<TextBlock>(this, "rainMax");

        //        if (tbminRain != null && tbmaxRain != null)
        //        {
        //            tbminRain.Text = string.Format("{0}mm", rangeRain.Minimum);
        //            tbmaxRain.Text = string.Format("{0}mm", rangeRain.Maximum);
        //        }

        //        var rangeWind = (TempChart.VerticalAxis as LinearAxis).ActualRange;

        //        var tbminWind = VisualTreeHelperExtension.GetChildByName<TextBlock>(this, "windMin");
        //        var tbmaxWind = VisualTreeHelperExtension.GetChildByName<TextBlock>(this, "windMax");

        //        if (tbminWind != null && tbmaxWind != null)
        //        {
        //            tbminWind.Text = string.Format("{0}级", rangeWind.Minimum);
        //            tbmaxWind.Text = string.Format("{0}级", rangeWind.Maximum);
        //        }

        //    };
        //    dispatcherTimer.Start();
        //}
    }
}