using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using CMAPlatform.Chart.Controller;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.CCPSOE.Models.PageModel;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Microsoft.Practices.Prism.Events;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     EventInfoTitle.xaml 的交互逻辑
    /// </summary>
    public partial class EventInfoTitle : ChartViewBase, INotifyPropertyChanged
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public EventInfoTitle(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();

            //Loaded += EventInfoTitle_Loaded;
            m_Controller = Controllers[0] as EventInfoTitleController;

            DataContext = m_Controller;

            m_Controller.MessageAggregator.GetMessage<CMAEventPageIntoMessage>()
                .Subscribe(ReceiveCMAEventMessage, ThreadOption.UIThread);
            m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>()
                .Subscribe(ReceiveCMAScutcheonSelectGroupMessage);
        }

        #endregion

        #region 释放

        /// <summary>
        ///     释放资源（取消订阅）
        /// </summary>
        public override void Dispose()
        {
            m_Controller.MessageAggregator.GetMessage<CMAEventPageIntoMessage>().Unsubscribe(ReceiveCMAEventMessage);

            base.Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly EventInfoTitleController m_Controller;

        /// <summary>
        ///     消息聚合器
        /// </summary>
        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        private Page3DModel m_Page3dModel;


        private Guid m_PageId = Guid.Empty;

        private Guid m_PageInstanceId = Guid.NewGuid();

        #endregion

        #region 私有方法

        // 接收页面传来的告警消息
        private void ReceiveCMAScutcheonSelectGroupMessage(ScutcheonSelectGroupMessage obj)
        {
            if (obj == null) return;
            if (obj.id.Length == 4) return;

            var id = obj.id;
            var lon = obj.lon;
            var lat = obj.lat;
            var typeName = obj.typeName;
            var colour = obj.colour;
            var data = new WarningDetail();
            data = DataManager.GetWarningByIdentifier(id);
            if (!string.IsNullOrEmpty(data.headline))
            {
                m_Controller.TitleName = data.headline;
            }
            else
            {
                if (!string.IsNullOrEmpty(data.eventType))
                {
                    m_Controller.TitleName = data.eventType;
                }
                else
                {
                    m_Controller.TitleName = "";
                }
            }
            m_Controller.TitleInfo = data.description;
        }

        // 控件加载
        private void EventInfoTitle_Loaded(object sender, RoutedEventArgs e)
        {
            m_Controller.TitleName = "";
            m_Controller.TitleInfo = "";
        }

        // 返回按钮
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            var stateChangedModel = new StateChangedModel
            {
                PageNameList = new List<string>(),
                StateName = "首页默认"
            };
            //切换状态
            m_MessageAggregator.GetMessage<StateChangedMessage>().Publish(stateChangedModel);
        }

        // 接收页面传来的突发事件
        private void ReceiveCMAEventMessage(EventPageInto obj)
        {
            if (obj != null)
            {
                m_Controller.TitleName = obj.EventTitle;
                m_Controller.TitleInfo = obj.EventDescription;
            }
        }

        #endregion

        #region override

        public override void RefreshStyle()
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }

        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        public override void ExportChart(ExportType type)
        {
        }

        #endregion
    }
}