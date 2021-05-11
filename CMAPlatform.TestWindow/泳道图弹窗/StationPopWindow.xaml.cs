using System.Windows;
using System.Windows.Controls;

namespace CMAPlatform.TestWindow.泳道图弹窗
{
    /// <summary>
    ///     StationPopWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StationPopWindow : Window
    {
        public StationPopWindow()
        {
            InitializeComponent();
        }

        public StationPopWindow(TimeLineChartController controller)
        {
            InitializeComponent();
            DataContext = controller;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as TimeLineChartController).TimeBarChangeCommand.Execute(e.AddedItems[0] as StationInfo);
            Close();
        }
    }
}