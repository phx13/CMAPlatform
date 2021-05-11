using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Digihail.AVE.Launcher.Infrastructure;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCP.Helper;
using Digihail.CCP.UserControls;
using Digihail.CCP.Utilities;
using Microsoft.Practices.Prism.Commands;

namespace ACMAPlatform.Header
{
    /// <summary>
    ///     页头的视图模型
    /// </summary>
    [Export(typeof (HeaderViewModel))]
    internal class HeaderViewModel : IPartImportsSatisfiedNotification
    {
        #region Fields

        /// <summary>
        ///     进程间通信的消息聚合器对象
        /// </summary>
        public IMessageAggregator m_MessageAggregator = new MessageAggregator();

        #endregion Fields

        #region Commands

        /// <summary>
        ///     退出系统命令
        /// </summary>
        public ICommand ExitCommand
        {
            get { return new DelegateCommand(OnExit); }
        }

        #endregion

        #region Interface IPartImportsSatisfiedNotification

        /// <summary>
        ///     当前对象中全部Import执行完成，会触发此方法
        /// </summary>
        public void OnImportsSatisfied()
        {
        }

        #endregion

        /// <summary>
        ///     退出程序
        /// </summary>
        private void OnExit()
        {
            var tip = ImageHelper.GetString("s_Tip");
            var exitTip1 = ImageHelper.GetString("s_ExitProgramTip1");
            var exitTip2 = ImageHelper.GetString("s_ExitProgramTip2");

            if (MessageBoxWindow.Show(tip, exitTip1, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                CommonManager.KillAllLauncherByVCS();
                FrameworkCommands.ExitCommand.Execute();
            }
        }
    }
}