using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TimeBar;

namespace IntervalSpecificItems
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadTimeline1_ItemIntervalChanged(object sender, DrillEventArgs e)
        {
            var timeline = sender as RadTimeline;

            (timeline.DataContext as TimelineViewModel).CurrentInterval = timeline.CurrentItemInterval;
        }
    }
}