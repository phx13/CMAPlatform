using System.ComponentModel.Composition;
using System.Windows.Controls;
using Digihail.AVE.Launcher.Infrastructure;

namespace ACMAPlatform.Header
{
    /// <summary>
    ///     HeaderView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof (IHeaderView))]
    public partial class HeaderView : UserControl, IHeaderView, IPartImportsSatisfiedNotification
    {
        /// <summary>
        ///     当前视图的视图模型
        /// </summary>
        [Import(AllowRecomposition = false)] private HeaderViewModel m_HeaderViewModel;


        public HeaderView()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
        }

        public void OnImportsSatisfied()
        {
            // 绑定数据视图
            DataContext = m_HeaderViewModel;
        }
    }
}