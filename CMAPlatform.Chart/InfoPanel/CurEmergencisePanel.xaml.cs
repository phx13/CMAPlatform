using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCP.Models;
using Digihail.CCP.Models.LauncherMessage;

namespace CMAPlatform.Chart.InfoPanel
{
    /// <summary>
    ///     CurEmergencisePanel.xaml 的交互逻辑
    /// </summary>
    [Export("IAVE3DInfoWindowContent_突发事件_标签", typeof (UserControl))]
    public partial class CurEmergencisePanel : UserControl
    {
        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        public CurEmergencisePanel()
        {
            InitializeComponent();
            DataContextChanged += InfoWindowPanel_DataContextChanged;
        }

        private void InfoWindowPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panelModel = DataContext as InfoPanelModel;
            if (panelModel != null)
            {
                //EventPageInto eventPageInto = new EventPageInto();
                //eventPageInto.EventPageType = 0;
                //eventPageInto.id = panelModel.Key;
                //m_MessageAggregator.GetMessage<CMAEventPageIntoMessage>().Publish(eventPageInto);

                var stateChangedModel = new StateChangedModel
                {
                    PageNameList = new List<string>(),
                    StateName = "事件页"
                };
                //切换状态
                m_MessageAggregator.GetMessage<StateChangedMessage>().Publish(stateChangedModel);
            }
        }
    }
}