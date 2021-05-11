using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CMAPlatform.Chart.Controller;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCP.Helper;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     RiskControl
    ///     EventListView.xaml 的交互逻辑
    /// </summary>
    public partial class EventListView : ChartViewBase, INotifyPropertyChanged
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public EventListView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();

            m_Controller = Controllers[0] as EventListController;
            DataContext = m_Controller;

            Loaded += EventListView_Loaded;

            // 订阅消息
            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>()
                .Subscribe(ReceiveCMAMainPageSelectedTabMessage);
            m_MessageAggregator.GetMessage<CMAClickWarningBillboardMessage>()
                .Subscribe(ReceiveCMAClickWarningBillboardMessage);

            SelectData();
        }

        #endregion

        #region 成员变量

        private readonly Guid m_PageInstanceId = Guid.NewGuid();

        private readonly EventListController m_Controller;

        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        #endregion

        #region 控件事件

        // 加载完成事件
        private void EventListView_Loaded(object sender, RoutedEventArgs e)
        {
            OnDadChartLoaded();

            // 初始化当前选中类型
            m_Controller.CurrentSelectedRiskType = m_Controller.RiskCountCollection.FirstOrDefault();

            m_Controller.RiskCountCollection.CollectionChanged += RiskCountCollection_CollectionChanged;
        }

        // 集合改变事件
        private void RiskCountCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            m_Controller.CurrentSelectedRiskType = m_Controller.RiskCountCollection.FirstOrDefault();
        }

        // 与页签联动
        private void TabItem_Down(object sender, MouseButtonEventArgs e)
        {
            var tabItem = sender as TabItem;
            if (tabItem != null)
            {
                var data = new CMAMainPageSelectedTabData
                {
                    InstanceGuid = m_PageInstanceId,
                    TabName = tabItem.Header.ToString()
                };
                m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>().Publish(data);
            }
        }

        // 预警态势点击跳转事件页
        private void RiskSelect_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;

            //gc
            var warnings = border.DataContext as Warnings;

            var stateChangedModel = new StateChangedModel
            {
                PageNameList = new List<string>(),
                StateName = "事件页"
            };

            //切换状态
            m_MessageAggregator.GetMessage<StateChangedMessage>().Publish(stateChangedModel);

            var view = CommonHelper.Instance.FindEventPageViewModel(warnings.AreaCode);

            if (view != null)
            {
                //发送三维位置消息
                var globalData = new GlobeTranslocationData();
                globalData.PageId = CCPHelper.Instance.GetCurrentCCPPageModel().ID;
                globalData.PageInstanceId = Guid.NewGuid();
                globalData.IsFree = true;
                globalData.CurLongitude = warnings.Lon;
                globalData.CurLatitude = warnings.Lat;
                globalData.CurHeight = view.Height;
                globalData.CurSurroundAngle = view.Yaw;
                globalData.CurPitch = view.Pitch;

                m_MessageAggregator.GetMessage<GlobeTranslocationMessage>().Publish(globalData);
            }

            //gc
            ////发送进入事件页
            var data = new ScutcheonSelectGroupMessage();
            data.areaCode = warnings.AreaCode;
            data.id = warnings.ID;
            data.lon = warnings.Lon;
            data.lat = warnings.Lat;
            data.colour = warnings.WarningLevel;
            data.Type = warnings.WarningType;
            data.typeName = warnings.WarningTypeName;
            data.DateTime = warnings.Time;

            m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>().Publish(data);

            //var i = 0;
            //while (i < 2)
            //{
            //    m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>().Publish(data);
            //    Thread.Sleep(20);
            //    i++;
            //}

            //if (warnings != null && warnings.WarningType == "11B01") return;
        }

        // 突发事件点击跳转事件页
        private void EventItemsControlBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn.Content != null)
            {
                var events = btn.Content as Events;

                var stateChangedModel = new StateChangedModel
                {
                    PageNameList = new List<string>(),
                    StateName = "突发事件页"
                };
                //切换状态
                m_MessageAggregator.GetMessage<StateChangedMessage>().Publish(stateChangedModel);

                // 切换地图视角
                var view = CommonHelper.Instance.FindEventPageViewModel("XXXX00");
                if (view != null)
                {
                    //发送三维位置消息
                    var globalData = new GlobeTranslocationData();
                    globalData.PageId = CCPHelper.Instance.GetCurrentCCPPageModel().ID;
                    globalData.PageInstanceId = Guid.NewGuid();
                    globalData.IsFree = true;
                    globalData.CurLongitude = events.Longitude;
                    globalData.CurLatitude = events.Latitude;
                    //globalData.CurHeight = view.Height;
                    globalData.CurHeight = 30000;
                    globalData.CurSurroundAngle = view.Yaw;
                    globalData.CurPitch = view.Pitch;

                    m_MessageAggregator.GetMessage<GlobeTranslocationMessage>().Publish(globalData);
                }

                // 突发事件跳转事件页
                var eventPageInto = new EventPageInto
                {
                    CityCode = events.City,
                    CountryCode = events.Country,
                    ProvinceCode = events.Province,
                    EventBeginTime = events.Time,
                    EventDescription = events.Detail,
                    EventEndTIme = events.EndTime,
                    EventLat = events.Latitude,
                    EventLon = events.Longitude,
                    EventPlace = events.Location,
                    EventTitle = events.Name
                };

                m_MessageAggregator.GetMessage<CMAEventPageIntoMessage>().Publish(eventPageInto);

                //var i = 0;
                //while (i < 2)
                //{
                //    m_MessageAggregator.GetMessage<CMAEventPageIntoMessage>().Publish(eventPageInto);
                //    Thread.Sleep(20);
                //    i++;
                //}
            }
        }

        #endregion

        #region 私有方法

        // 初始化数据集合
        private void SelectData()
        {
            EventItemsControl.ItemsSource = DataManager.Instance.EventModels;

            EventItemsControl.Items.IsLiveSorting = true;
            EventItemsControl.Items.SortDescriptions.Add(new SortDescription("IsHistory", ListSortDirection.Ascending));
            EventItemsControl.Items.SortDescriptions.Add(new SortDescription("Time", ListSortDirection.Descending));

            ExemtremeControl.ItemsSource = DataManager.Instance.ExemtremeWeatherModel;
        }

        // 接收图层联动控制消息
        private void ReceiveCMAMainPageSelectedTabMessage(CMAMainPageSelectedTabData obj)
        {
            if (obj != null && obj.InstanceGuid != m_PageInstanceId)
            {
                switch (obj.TabName)
                {
                    case "突发事件":
                        tabRightPanel.SelectedIndex = 0;
                        break;
                    case "风险态势":
                        tabRightPanel.SelectedIndex = 1;
                        break;
                    case "极端天气":
                        tabRightPanel.SelectedIndex = 2;
                        break;
                    default:
                        break;
                }
            }
        }

        // 标牌点击响应
        private void ReceiveCMAClickWarningBillboardMessage(string obj)
        {
            if (obj != null)
            {
                foreach (var item in DataManager.Instance.RiskSituationModels)
                {
                    foreach (var data in item.GroupWarning)
                    {
                        foreach (var ids in data.id)
                        {
                            if (obj == ids)
                            {
                                tabRightPanel.SelectedIndex = 1;
                                m_Controller.CurrentSelectedRiskType = item;
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 重写基类方法

        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        public override void ExportChart(ExportType type)
        {
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        public override void RefreshStyle()
        {
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }

        public override void Dispose()
        {
            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>()
                .Unsubscribe(ReceiveCMAMainPageSelectedTabMessage);

            base.Dispose();
        }

        #endregion

        #region OnPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}