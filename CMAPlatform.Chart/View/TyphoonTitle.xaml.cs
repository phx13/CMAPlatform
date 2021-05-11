using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CMAPlatform.Chart.Controller;
using CMAPlatform.Chart.Window;
using CMAPlatform.DataClient;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCP.Helper;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Microsoft.Practices.Prism.Events;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     TyphoonTitle.xaml 的交互逻辑
    /// </summary>
    public partial class TyphoonTitle : ChartViewBase, INotifyPropertyChanged
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public TyphoonTitle(ChartViewBaseModel model) : base(model)
        {
            InitializeComponent();

            //Loaded += TyphoonTitle_Loaded;

            m_Controller = Controllers[0] as TyphoonTitleController;

            DataContext = m_Controller;

            m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>()
                .Subscribe(ReceiveCMAScutcheonSelectGroupMessage, ThreadOption.UIThread);
        }

        #endregion

        #region 成员变量

        /// <summary>
        ///     消息聚合器
        /// </summary>
        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly TyphoonTitleController m_Controller;

        #endregion

        #region 私有方法

        // 接收页面传来的告警消息
        private void ReceiveCMAScutcheonSelectGroupMessage(ScutcheonSelectGroupMessage obj)
        {
            // 判断
            if (obj == null) return;
            if (obj.typeName != "11B01") return;

            var id = obj.id;
            if (id.Length == 4)
            {
                id = "520200";
            }
            var data = DataManager.GetWarningByIdentifier(id);
            if (data == null) return;
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
            m_Controller.Time = data.sendTime;
            m_Controller.Severity = data.severity;
        }

        // 控件加载
        private void TyphoonTitle_Loaded(object sender, RoutedEventArgs e)
        {
            //buttonName.Text = "台风详情";
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

            // 取消选中
            var data = new SelectedFocusData
            {
                From = CCPHelper.Instance.GetCurrentCCPPageModel().ID,
                PageInstanceId = Guid.NewGuid(),
                LstGoeObjectNames = new List<string>(),
                LstObjects = new List<Guid>(),
                SelectedInfoList = new List<string>()
            };
            m_MessageAggregator.GetMessage<SelectedFocusMessage>().Publish(data);
        }

        // 弹出台风详情按钮事件
        private void OpenTyphoonDetail(object sender, MouseButtonEventArgs e)
        {
            var typhoonDetailWindow = new TyphoonDetailWindow(m_Controller.TitleName, m_Controller.Time,
                m_Controller.TitleInfo, m_Controller.Severity)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            typhoonDetailWindow.ShowDialog();
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

        #region 释放

        /// <summary>
        ///     释放资源（取消订阅）
        /// </summary>
        public override void Dispose()
        {
            m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>()
                .Unsubscribe(ReceiveCMAScutcheonSelectGroupMessage);

            base.Dispose();
        }

        #endregion

        #endregion
    }
}