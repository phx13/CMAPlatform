using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CMAPlatform.Chart.InfoPanel
{
    /// <summary>
    ///     EmptyInfoPanel.xaml 的交互逻辑
    /// </summary>
    [Export("IAVE3DInfoWindowContent_test", typeof (UserControl))]
    public partial class EmptyInfoPanel : UserControl
    {
        public EmptyInfoPanel()
        {
            InitializeComponent();
        }
    }
}