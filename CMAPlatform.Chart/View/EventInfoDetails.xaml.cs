using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using CMAPlatform.Chart.Controller;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
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
    ///     EventInfoDetails.xaml 的交互逻辑
    /// </summary>
    public partial class EventInfoDetails : ChartViewBase, INotifyPropertyChanged
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public EventInfoDetails(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            //Loaded += EventInfoTitle_Loaded;
            m_Controller = Controllers[0] as EventInfoDetailsController;

            DataContext = m_Controller;

            m_Controller.MessageAggregator.GetMessage<CMAEventPageIntoMessage>()
                .Subscribe(ReceiveCMAEventMessage, ThreadOption.UIThread);
            m_Controller.MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>()
                .Subscribe(ReceiveCMAScutcheonSelectGroupMessage, ThreadOption.UIThread);
            EventDictionary = DataManager.Instance.EventDictiinaryType();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly EventInfoDetailsController m_Controller;

        /// <summary>
        ///     消息聚合器
        /// </summary>
        private IMessageAggregator m_MessageAggregator = new MessageAggregator();

        private Page3DModel m_Page3dModel;

        private Guid m_PageId = Guid.Empty;

        private Guid m_PageInstanceId = Guid.NewGuid();

        private ObservableCollection<EventDictionary> m_EventDictionary = new ObservableCollection<EventDictionary>();
        ///// <summary>
        ///// 突发事件_灾害类型
        ///// </summary>
        public ObservableCollection<EventDictionary> EventDictionary
        {
            get { return m_EventDictionary; }
            set
            {
                m_EventDictionary = value;
                OnPropertyChanged("EventDictionary");
            }
        }

        #endregion

        #region 私有方法

        // 控件加载事件
        private void EventInfoTitle_Loaded(object sender, RoutedEventArgs e)
        {
            m_Controller.TitleName = "山东省暴雨橙色预警";
            m_Controller.TitleType = "暴雨";
            m_Controller.TitleAddress = "山东省-烟台市-龙口市  N37.65°  E119.12°";
            m_Controller.PredictionTime = "2019-04-01  12:00:00";
            m_Controller.TakeEffectTime = "4月2日至4月5日";
            m_Controller.BrowseCount = "11";
            m_Controller.TransmitCount = "9";
        }

        // 接收页面传来的告警消息
        private void ReceiveCMAScutcheonSelectGroupMessage(ScutcheonSelectGroupMessage obj)
        {
            if (obj != null)
            {
                var id = obj.id;
                var lon = obj.lon;
                var lat = obj.lat;
                var typeName = obj.typeName;
                var colour = obj.colour;
                var data = new WarningDetail();
                data = DataManager.GetWarningByIdentifier(id);
                if (data != null)
                {
                    //m_Controller.TitleName = data.headline;
                    try
                    {
                        m_Controller.TitleType = EventDictionary.FirstOrDefault(t => t.Code == data.eventType).Name;
                    }
                    catch (Exception)
                    {
                        m_Controller.TitleType = "";
                    }
                    m_Controller.TitleAddress = data.sender;
                    m_Controller.PredictionTime = data.sendTime;
                    m_Controller.TakeEffectTime = data.effective;
                }
            }
        }

        // 接收页面传来的突发事件
        private void ReceiveCMAEventMessage(EventPageInto obj)
        {
            if (obj != null)
            {
                m_Controller.TitleType = "突发事件";
                m_Controller.TitleAddress = obj.EventPlace;
                m_Controller.PredictionTime = obj.EventBeginTime;
                m_Controller.TakeEffectTime = "";
            }
        }

        #endregion

        #region 实现基类方法

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

        public override void Dispose()
        {
            m_Controller.MessageAggregator.GetMessage<CMAEventPageIntoMessage>().Unsubscribe(ReceiveCMAEventMessage);
            m_Controller.MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>()
                .Unsubscribe(ReceiveCMAScutcheonSelectGroupMessage);

            base.Dispose();
        }

        #endregion
    }
}