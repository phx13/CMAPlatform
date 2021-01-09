using CMAPlatform.Models.AlarmDetailModel;
using Digihail.AVE.Launcher.Controls;

namespace CMAPlatform.TimeLine.Window
{
    /// <summary>
    ///     AlarmDetail.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmDetail : PopWindow
    {
        public AlarmDetail()
        {
            InitializeComponent();
        }

        public AlarmDetail(AlarmDetailModel alarmDetailModel)
        {
            InitializeComponent();
            DataContext = alarmDetailModel;
        }
    }
}