using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using CMAPlatform.Models.AlarmDetailModel;
using CMAPlatform.Models.Enum;
using CMAPlatform.Models.MessageModel;
using CMAPlatform.Models.TimeBarModel;
using CMAPlatform.Models.TimeLineModel;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace CMAPlatform.TimeLine
{
    public class TimeLineChartController : ChartControllerBase
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public TimeLineChartController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>().Subscribe(ReceiveCMAScutcheonSelectGroupMessage, ThreadOption.UIThread);
            MessageAggregator.GetMessage<CMAEventPageIntoMessage>().Subscribe(ReceiveCMAEventPageIntoMessage, ThreadOption.UIThread);
            MessageAggregator.GetMessage<SelectedFocusMessage>().Subscribe(ReceiveSelectedFocusMessage, ThreadOption.UIThread);
        }

        #endregion

        #region Override

        public override void ReceiveData(AdapterDataTable adt)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        #endregion

        #region 成员变量

        // 当前选中的预警ID
        private string m_CurrentWarningId;

        private string m_EventType;

        private List<TyphoonDataInfo> m_TyphoonData = new List<TyphoonDataInfo>();
        private List<List<WarningTyphoonDataInfo>> m_WarningTyphoonData = new List<List<WarningTyphoonDataInfo>>();

        // 最邻近站ID-Name字典
        private Dictionary<string, string> m_ClosestStation = new Dictionary<string, string>();

        #endregion

        #region 公共属性

        /// <summary>
        ///     消息聚合器
        /// </summary>
        public IMessageAggregator MessageAggregator
        {
            get { return new MessageAggregator(); }
        }

        private string m_Header;

        /// <summary>
        ///     泳道图标题
        /// </summary>
        public string Header
        {
            get { return m_Header; }
            set
            {
                m_Header = value;
                OnPropertyChanged("Header");
            }
        }

        private DateTime m_StartTime;

        /// <summary>
        ///     开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return m_StartTime; }
            set
            {
                m_StartTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        private DateTime m_EndTime;

        /// <summary>
        ///     结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return m_EndTime; }
            set
            {
                m_EndTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        private DateTime m_VisibleStartTime;

        /// <summary>
        ///     开始时间
        /// </summary>
        public DateTime VisibleStartTime
        {
            get { return m_VisibleStartTime; }
            set
            {
                m_VisibleStartTime = value;
                OnPropertyChanged("VisibleStartTime");
                DateAssignment();
            }
        }

        private DateTime m_VisibleEndTime;

        /// <summary>
        ///     结束时间
        /// </summary>
        public DateTime VisibleEndTime
        {
            get { return m_VisibleEndTime; }
            set
            {
                m_VisibleEndTime = value;
                OnPropertyChanged("VisibleEndTime");
            }
        }

        private DateTime m_CurrentTyphoonTime = DateTime.Today.AddDays(1);
        /// <summary>
        /// 当前台风时间
        /// </summary>
        public DateTime CurrentTyphoonTime
        {
            get { return m_CurrentTyphoonTime; }
            set
            {
                m_CurrentTyphoonTime = value;
                OnPropertyChanged("CurrentTyphoonTime");
            }
        }

        private DateTime m_CurrentDate;
        /// <summary>
        /// 当前日期
        /// </summary>
        public DateTime CurrentDate
        {
            get { return m_CurrentDate; }
            set
            {
                m_CurrentDate = value;
                OnPropertyChanged("CurrentDate");
            }
        }

        private ObservableCollection<TimeLineItem> m_TimeLineItems = new ObservableCollection<TimeLineItem>();

        /// <summary>
        ///     Items
        /// </summary>
        public ObservableCollection<TimeLineItem> TimeLineItems
        {
            get { return m_TimeLineItems; }
            set
            {
                m_TimeLineItems = value;
                OnPropertyChanged("TimeLineItems");
            }
        }

        private ObservableCollection<TimeLineItem> m_OnViewTimeLineItems = new ObservableCollection<TimeLineItem>();

        /// <summary>
        ///     上图Items
        /// </summary>
        public ObservableCollection<TimeLineItem> OnViewTimeLineItems
        {
            get { return m_OnViewTimeLineItems; }
            set
            {
                m_OnViewTimeLineItems = value;
                OnPropertyChanged("OnViewTimeLineItems");
            }
        }

        private ObservableCollection<CountTypeInfo> m_OnViewCountInfos = new ObservableCollection<CountTypeInfo>();

        /// <summary>
        ///     上图的统计数量
        /// </summary>
        public ObservableCollection<CountTypeInfo> OnViewCountInfos
        {
            get { return m_OnViewCountInfos; }
            set
            {
                m_OnViewCountInfos = value;
                OnPropertyChanged("OnViewCountInfos");
            }
        }

        private ObservableCollection<MaxCountInfo> m_MaxCountInfos = new ObservableCollection<MaxCountInfo>();

        /// <summary>
        ///     最大降水量
        /// </summary>
        public ObservableCollection<MaxCountInfo> MaxCountInfos
        {
            get { return m_MaxCountInfos; }
            set
            {
                m_MaxCountInfos = value;
                OnPropertyChanged("MaxCountInfos");
            }
        }

        private List<PeakDataInfo> m_MaxRainInfos;

        /// <summary>
        ///     最大降水量
        /// </summary>
        public List<PeakDataInfo> MaxRainInfos
        {
            get { return m_MaxRainInfos; }
            set
            {
                m_MaxRainInfos = value;
                OnPropertyChanged("MaxRainInfos");
            }
        }

        private List<TimeLineWarningData> m_TimeLineWarnings = new List<TimeLineWarningData>();

        /// <summary>
        ///     时间轴上的告警
        /// </summary>
        public List<TimeLineWarningData> TimeLineWarnings
        {
            get { return m_TimeLineWarnings; }
            set
            {
                m_TimeLineWarnings = value;
                OnPropertyChanged("TimeLineWarnings");
            }
        }

        private List<TimeLineService> m_TimeLineServices = new List<TimeLineService>();

        /// <summary>
        ///     时间轴上的事件
        /// </summary>
        public List<TimeLineService> TimeLineServices
        {
            get { return m_TimeLineServices; }
            set
            {
                m_TimeLineServices = value;
                OnPropertyChanged("TimeLineServices");
            }
        }

        private List<TimeLineWarningCount> m_TimeLineWarningCounts = new List<TimeLineWarningCount>();

        /// <summary>
        ///     时间轴上的告警站数统计
        /// </summary>
        public List<TimeLineWarningCount> TimeLineWarningCounts
        {
            get { return m_TimeLineWarningCounts; }
            set
            {
                m_TimeLineWarningCounts = value;
                OnPropertyChanged("TimeLineWarningCounts");
            }
        }

        /// <summary>
        ///     灾情舆情事件集合
        /// </summary>
        private List<TimeLineItem> m_Disasters = new List<TimeLineItem>();

        public List<TimeLineItem> Disasters
        {
            get { return m_Disasters; }
            set
            {
                m_Disasters = value;
                OnPropertyChanged("Disaster");
            }
        }

        /// <summary>
        ///     国家服务事件集合
        /// </summary>
        private List<TimeLineItem> m_NationServices = new List<TimeLineItem>();

        public List<TimeLineItem> NationServices
        {
            get { return m_NationServices; }
            set
            {
                m_NationServices = value;
                OnPropertyChanged("NationServices");
            }
        }

        /// <summary>
        ///     省服务事件集合
        /// </summary>
        private List<TimeLineItem> m_ProvinceServices = new List<TimeLineItem>();

        public List<TimeLineItem> ProvinceServices
        {
            get { return m_ProvinceServices; }
            set
            {
                m_ProvinceServices = value;
                OnPropertyChanged("ProvinceServices");
            }
        }

        /// <summary>
        ///     国家事件集合
        /// </summary>
        private List<TimeLineItem> m_Nations = new List<TimeLineItem>();

        public List<TimeLineItem> Nations
        {
            get { return m_Nations; }
            set
            {
                m_Nations = value;
                OnPropertyChanged("Nations");
            }
        }

        /// <summary>
        ///     省事件集合
        /// </summary>
        private List<TimeLineItem> m_Provinces = new List<TimeLineItem>();

        public List<TimeLineItem> Provinces
        {
            get { return m_Provinces; }
            set
            {
                m_Provinces = value;
                OnPropertyChanged("Provinces");
            }
        }

        /// <summary>
        ///     市事件集合
        /// </summary>
        private List<TimeLineItem> m_Citys = new List<TimeLineItem>();

        public List<TimeLineItem> Citys
        {
            get { return m_Citys; }
            set
            {
                m_Citys = value;
                OnPropertyChanged("Citys");
            }
        }

        /// <summary>
        ///     县事件集合
        /// </summary>
        private List<TimeLineItem> m_Countys = new List<TimeLineItem>();

        public List<TimeLineItem> Countys
        {
            get { return m_Countys; }
            set
            {
                m_Countys = value;
                OnPropertyChanged("Countys");
            }
        }

        private AlarmDetailModel m_CurrentAlarmDetail;

        /// <summary>
        ///     预警详情页数据
        /// </summary>
        public AlarmDetailModel CurrentAlarmDetail
        {
            get { return m_CurrentAlarmDetail; }
            set
            {
                m_CurrentAlarmDetail = value;
                OnPropertyChanged("CurrentAlarmDetail");
            }
        }

        private ObservableCollection<StationInfo> m_StationInfos = new ObservableCollection<StationInfo>();

        /// <summary>
        ///     观测站弹窗数据
        /// </summary>
        public ObservableCollection<StationInfo> StationInfos
        {
            get { return m_StationInfos; }
            set
            {
                m_StationInfos = value;
                OnPropertyChanged("StationInfos");
            }
        }

        private List<TimeLineStationData> m_TimeLineStationDatas = new List<TimeLineStationData>();

        /// <summary>
        ///     观测站弹窗数据（接口获取原始数据）
        /// </summary>
        public List<TimeLineStationData> TimeLineStationDatas
        {
            get { return m_TimeLineStationDatas; }
            set
            {
                m_TimeLineStationDatas = value;
                OnPropertyChanged("TimeLineStationDatas");
            }
        }

        private ObservableCollection<SingleStationInfo> m_Rains = new ObservableCollection<SingleStationInfo>();

        /// <summary>
        ///     单站降水量柱图
        /// </summary>
        public ObservableCollection<SingleStationInfo> Rains
        {
            get { return m_Rains; }
            set
            {
                m_Rains = value;
                OnPropertyChanged("Rains");
            }
        }

        private ObservableCollection<SingleStationWindInfo> m_Winds = new ObservableCollection<SingleStationWindInfo>();

        /// <summary>
        ///     单站大风折线图
        /// </summary>
        public ObservableCollection<SingleStationWindInfo> Winds
        {
            get { return m_Winds; }
            set
            {
                m_Winds = value;
                OnPropertyChanged("Winds");
            }
        }

        private ObservableCollection<SingleStationColdWaveInfo> m_ColdWaves =
            new ObservableCollection<SingleStationColdWaveInfo>();

        /// <summary>
        ///     单站寒潮折线图
        /// </summary>
        public ObservableCollection<SingleStationColdWaveInfo> ColdWaves
        {
            get { return m_ColdWaves; }
            set
            {
                m_ColdWaves = value;
                OnPropertyChanged("ColdWaves");
            }
        }

        private ObservableCollection<EmergencyBarInfo> m_EmergencyBarInfos =
            new ObservableCollection<EmergencyBarInfo>();

        /// <summary>
        ///     突发事件组合图
        /// </summary>
        public ObservableCollection<EmergencyBarInfo> EmergencyBarInfos
        {
            get { return m_EmergencyBarInfos; }
            set
            {
                m_EmergencyBarInfos = value;
                OnPropertyChanged("EmergencyBarInfos");
            }
        }

        private string m_TimeBarTitle = "实况";

        /// <summary>
        ///     TimeBar标题
        /// </summary>
        public string TimeBarTitle
        {
            get { return m_TimeBarTitle; }
            set
            {
                m_TimeBarTitle = value;
                OnPropertyChanged("TimeBarTitle");
            }
        }

        private string m_CurrentSingleBarTimespan;

        /// <summary>
        ///     降雨单柱图当前时间跨度
        /// </summary>
        public string CurrentSingleBarTimespan
        {
            get { return m_CurrentSingleBarTimespan; }
            set
            {
                m_CurrentSingleBarTimespan = value;
                OnPropertyChanged("CurrentSingleBarTimespan");
            }
        }

        /// <summary>
        ///     当前站点
        /// </summary>
        public StationInfo CurrentStationInfo { get; set; }

        /// <summary>
        ///     突发事件还是预警态势
        /// </summary>
        public string FromMainPageType { get; set; }

        private ObservableCollection<TyphoonChartModel> m_TyphoonChartData = new ObservableCollection<TyphoonChartModel>();
        /// <summary>
        /// 台风图表数据
        /// </summary>
        public ObservableCollection<TyphoonChartModel> TyphoonChartData
        {
            get { return m_TyphoonChartData; }
            set
            {
                m_TyphoonChartData = value;
                OnPropertyChanged("TyphoonChartData");
            }
        }

        #endregion

        #region 命令

        /// <summary>
        ///     切换柱图命令
        /// </summary>
        public DelegateCommand<StationInfo> TimeBarChangeCommand { get; set; }

        /// <summary>
        ///     切换柱图时间跨度命令
        /// </summary>
        public DelegateCommand<string> SingleTimeBarChangeCommand { get; set; }

        /// <summary>
        ///     切换柱图时间跨度命令
        /// </summary>
        public DelegateCommand<string> ColdWaveLineChangeCommand { get; set; }

        /// <summary>
        ///     切换突发事件或预警态势命令
        /// </summary>
        public DelegateCommand<string> EmergencyOrWarningChangeCommand { get; set; }

        #endregion

        #region 私有方法

        /// <summary>
        /// 接收到台风轨迹点选中消息
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveSelectedFocusMessage(SelectedFocusData obj)
        {
            if (obj.SelectedInfoList.Count == 0)
            {
                return;
            }
            var selectTyphoon = obj.SelectedInfoList.First();
            var tableName = selectTyphoon.Split('=')[0];
            if (tableName == "typhoonpath")
            {
                var selectTyphoonId = selectTyphoon.Split('=')[2];
                if (m_TyphoonData != null)
                {
                    var typhoon = m_TyphoonData.First(e => e.bj_datetime == selectTyphoonId);
                    LoadTyphoonCircle(typhoon.bj_datetime, m_TyphoonData, true);//把当前点击的台风画出来

                    #region 同步泳道图跳转居中

                    CurrentTyphoonTime = DateTime.Parse(typhoon.bj_datetime);

                    if (CurrentTyphoonTime < StartTime || CurrentTyphoonTime > EndTime)
                    {
                        return;
                    }
                    VisibleStartTime = CurrentTyphoonTime.AddHours(-5);
                    VisibleEndTime = CurrentTyphoonTime.AddHours(5);

                    if (VisibleStartTime >= EndTime)
                    {
                        VisibleStartTime = EndTime.AddHours(-10);
                        VisibleEndTime = EndTime;
                    }
                    else if (VisibleEndTime <= StartTime)
                    {
                        VisibleStartTime = StartTime;
                        VisibleEndTime = StartTime.AddHours(10);
                    }
                    else if (VisibleStartTime <= StartTime && VisibleEndTime >= StartTime)
                    {
                        VisibleStartTime = StartTime;
                        VisibleEndTime = StartTime.AddHours(10);
                    }
                    else if (VisibleStartTime <= EndTime && VisibleEndTime >= EndTime)
                    {
                        VisibleStartTime = EndTime.AddHours(-10);
                        VisibleEndTime = EndTime;
                    }

                    #endregion
                }
            }
            else if (tableName == "closeststation")
            {
                var selectStationId = selectTyphoon.Split('=')[2];
                LoadEmergencyBarInfoData(selectStationId);
            }
        }

        /// <summary>
        ///     接收事件页预警态势消息
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveCMAScutcheonSelectGroupMessage(ScutcheonSelectGroupMessage obj)
        {
            if (obj != null)
            {
                LoadData(obj);
            }
        }

        /// <summary>
        ///     接收事件页突发事件消息
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveCMAEventPageIntoMessage(EventPageInto obj)
        {
            if (obj != null)
            {
                this.Dispatcher.InvokeAsync(() =>
                {
                    LoadData(obj);

                }, DispatcherPriority.Background);
                //LoadData(obj);
            }
        }

        /// <summary>
        ///     预警态势加载数据
        /// </summary>
        /// <param name="data"></param>
        private void LoadData(ScutcheonSelectGroupMessage data)
        {
            FromMainPageType = "预警态势";
            if (data.typeName == "11B01")
            {
                FromMainPageType = "台风";
            }
            EmergencyOrWarningChangeCommand.Execute(FromMainPageType);
            MaxCountInfos.Clear();

            TimeLineItems.Clear();
            OnViewTimeLineItems.Clear();

            // 取消泳道图时间背景
            CurrentTyphoonTime = DateTime.Today.AddDays(1);

            bool isShort = data.id.Length == 4;

            //获取预警名称
            if (!isShort)
            {
                var wdetail = DataManager.GetWarningByIdentifier(data.id);
                if (!string.IsNullOrEmpty(wdetail.headline))
                {
                    Header = wdetail.headline;
                }
                else
                {
                    if (!string.IsNullOrEmpty(wdetail.eventType))
                    {
                        Header = wdetail.eventType;
                    }
                    else
                    {
                        Header = "";
                    }
                }
            }
            else
            {
                Header = "台风" + data.TyphoonName;
                m_TyphoonData = DataManager.TyphoonData(data.id);//接口拉一下台风数据
            }

            if (DataManager.Instance.IsTestData)
            {
                m_CurrentWarningId = "13073041600000_20191023164215";

                StartTime = new DateTime(2018, 8, 14, 1, 0, 0);
                EndTime = new DateTime(2018, 8, 24, 1, 0, 0);
                VisibleStartTime = new DateTime(2018, 8, 19, 5, 0, 0);
                VisibleEndTime = new DateTime(2018, 8, 20, 15, 0, 0);

                // 台风事件页跳转联动
                //StartTime = new DateTime(2019, 2, 1, 1, 0, 0);
                //EndTime = new DateTime(2019, 2, 28, 1, 0, 0);
                //VisibleStartTime = new DateTime(2019, 2, 8, 5, 0, 0);
                //VisibleEndTime = new DateTime(2019, 2, 8, 15, 0, 0);

                TimeLineWarnings = DataManager.getTimeLineWarningData("", "", "", "");
                TimeLineServices = DataManager.getTimelineServices("", "", "");
                //TimeLineWarningCounts = DataManager.getTimelineWarningCount("", "", "", "");
                TimeLineWarningCounts = DataManager.getTimelineWarningCount("", "", "", "");

                // 收取最大降雨值，暂时弃用
                //MaxRainInfos = DataManager.getPeakData(new PeakDataParamsModel());
            }
            else
            {
                m_CurrentWarningId = data.id;

                var warnid = data.id;
                var eventtype = data.typeName;

                var warnTimeTemp = DateTime.Parse(data.DateTime);
                var warnTime = warnTimeTemp.Date.AddHours(warnTimeTemp.Hour);

                if (isShort)
                {
                    StartTime = warnTime;
                    EndTime = DateTime.Parse(m_TyphoonData.Last().bj_datetime);
                    VisibleStartTime = warnTime.AddHours(-5);
                    VisibleEndTime = warnTime.AddHours(5);
                }
                else
                {
                    StartTime = warnTime.AddDays(-5);
                    EndTime = warnTime.AddDays(5);
                    VisibleStartTime = warnTime.AddHours(-5);
                    VisibleEndTime = warnTime.AddHours(5);
                }

                if (data.typeName == "11B01")//如果是台风
                {
                    TimeLineWarnings = DataManager.GetWarningsOfTyphoon(StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    TimeLineWarnings = DataManager.getTimeLineWarningData((isShort) ? "520200" : warnid, eventtype, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                TimeLineServices = DataManager.getTimelineServices((isShort) ? "520200" : warnid, StartTime.ToString("yyyy-MM-dd HH"), EndTime.ToString("yyyy-MM-dd HH"));
                m_EventType = eventtype;

                // 收取最大降雨值，暂时弃用
                //MaxRainInfos = DataManager.getPeakData(new PeakDataParamsModel
                //{
                //    adminCodeChn = data.areaCode.Substring(0, 6),
                //    startTime = StartTime.ToString("yyyy-MM-dd HH:00:00"),
                //    endTime = EndTime.ToString("yyyy-MM-dd HH:00:00"),
                //    peakName = "pre_1h"
                //});
            }

            #region  灾情舆情/服务国家级/服务省级

            m_Disasters.Clear();
            m_NationServices.Clear();
            m_ProvinceServices.Clear();

            //补充最小时间
            var zqyqmin = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "灾情舆情指数",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(zqyqmin);

            var minNationservice = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "服务-国家级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minNationservice);

            var minservice = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "服务-省级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minservice);

            //补充最大时间
            var zqyqmax = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "灾情舆情指数",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(zqyqmax);

            var maxNationservice = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "服务-国家级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxNationservice);
            var maxservice = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "服务-省级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxservice);

            foreach (var servicedata in TimeLineServices)
            {
                DateTime sTime;
                var service = new TimeLineItem();
                if (DateTime.TryParse(servicedata.cdate, out sTime))
                {
                    service.StartTime = sTime;
                    service.Name = servicedata.title;
                    service.ServiceUrl = servicedata.url;
                    //进行三种服务类型区分
                    switch (servicedata.category)
                    {
                        case "nation":
                            service.GroupName = "服务-国家级";
                            m_NationServices.Add(service);
                            break;
                        case "zaiqing":
                            service.GroupName = "灾情舆情指数";
                            m_Disasters.Add(service);
                            break;
                        default:
                            service.GroupName = "服务-省级";
                            service.EventType = servicedata.category;
                            m_ProvinceServices.Add(service);
                            break;
                    }
                }
            }
            TimeLineItems.AddRange(m_NationServices);
            TimeLineItems.AddRange(m_ProvinceServices);
            TimeLineItems.AddRange(m_Disasters);

            //服务-国家级
            var ressernation = from s in m_NationServices.Where(t => t.GroupName == "服务-国家级") group s by s.StartTime;
            foreach (var ser in ressernation)
            {
                if (ser.Count() > 1)
                {
                    var serTotal = new TimeLineItem
                    {
                        StartTime = ser.Key,
                        Name = "国家级服务信息共" + ser.Count() + "条",
                        GroupName = "服务-国家级",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeSer in
                            TimeLineItems.Where(t => t.StartTime == ser.Key && t.GroupName == "服务-国家级").ToList())
                    {
                        TimeLineItems.Remove(removeSer);
                    }
                    TimeLineItems.Add(serTotal);
                }
            }
            //服务-省级
            var resser = from s in m_ProvinceServices.Where(t => t.GroupName == "服务-省级") group s by s.StartTime;
            foreach (var ser in resser)
            {
                if (ser.Count() > 1)
                {
                    var serTotal = new TimeLineItem
                    {
                        StartTime = ser.Key,
                        Name = "省级服务信息共" + ser.Count() + "条",
                        GroupName = "服务-省级",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeSer in
                            TimeLineItems.Where(t => t.StartTime == ser.Key && t.GroupName == "服务-省级").ToList())
                    {
                        TimeLineItems.Remove(removeSer);
                    }
                    TimeLineItems.Add(serTotal);
                }
            }
            //灾情舆情指数
            var zqyq = from s in m_Disasters.Where(t => t.GroupName == "灾情舆情指数") group s by s.StartTime;
            foreach (var ser in zqyq)
            {
                if (ser.Count() > 1)
                {
                    var serTotal = new TimeLineItem
                    {
                        StartTime = ser.Key,
                        Name = "灾情舆情信息共" + ser.Count() + "条",
                        GroupName = "灾情舆情指数",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeSer in
                            TimeLineItems.Where(t => t.StartTime == ser.Key && t.GroupName == "灾情舆情指数").ToList())
                    {
                        TimeLineItems.Remove(removeSer);
                    }
                    TimeLineItems.Add(serTotal);
                }
            }

            #endregion

            #region 预警

            m_Nations.Clear();
            m_Provinces.Clear();
            m_Citys.Clear();
            m_Countys.Clear();

            //补充最小时间
            var minnation = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "预警-国家级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minnation);
            var minprovinces = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "预警-省级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minprovinces);
            var mincity = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "预警-市级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(mincity);
            var mincounty = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "预警-县级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(mincounty);
            //补充最大时间
            var maxnation = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "预警-国家级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxnation);
            var maxprovinces = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "预警-省级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxprovinces);
            var maxcity = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "预警-市级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxcity);
            var maxcounty = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "预警-县级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxcounty);

            foreach (var warningdata in TimeLineWarnings)
            {
                var yujing = new TimeLineItem();

                DateTime sTime;
                if (DateTime.TryParse(warningdata.sendTime, out sTime))
                {
                    yujing.StartTime = sTime;
                    yujing.Name = warningdata.headline;
                    yujing.Id = warningdata.identifier;
                    yujing.Severity = warningdata.severity.ToLower();
                    yujing.EventType = warningdata.eventType;
                    var areacode = warningdata.senderCode.Substring(0, 6);
                    if (areacode.EndsWith("000000"))
                    {
                        yujing.GroupName = "预警-国家级";
                        m_Nations.Add(yujing);
                    }
                    else if (areacode.EndsWith("0000"))
                    {
                        yujing.GroupName = "预警-省级";
                        m_Provinces.Add(yujing);
                    }
                    else if (areacode.EndsWith("00"))
                    {
                        yujing.GroupName = "预警-市级";
                        m_Citys.Add(yujing);
                    }
                    else
                    {
                        yujing.GroupName = "预警-县级";
                        m_Countys.Add(yujing);
                    }
                }
            }
            TimeLineItems.AddRange(m_Nations);
            TimeLineItems.AddRange(m_Provinces);
            TimeLineItems.AddRange(m_Citys);
            TimeLineItems.AddRange(m_Countys);

            var resNation = from s in m_Nations group s by s.StartTime;
            foreach (var res in resNation)
            {
                if (res.Count() > 1)
                {
                    var resTotal = new TimeLineItem
                    {
                        StartTime = res.Key,
                        Name = "预警信息共" + res.Count() + "条",
                        GroupName = "预警-国家级",
                        EventType="alarm"
                    };
                    foreach (
                        var removeRes in
                            TimeLineItems.Where(t => t.StartTime == res.Key && t.GroupName == "预警-国家级").ToList())
                    {
                        TimeLineItems.Remove(removeRes);
                    }
                    TimeLineItems.Add(resTotal);
                }
            }

            var resProvince = from s in m_Nations group s by s.StartTime;
            foreach (var res in resProvince)
            {
                if (res.Count() > 1)
                {
                    var resTotal = new TimeLineItem
                    {
                        StartTime = res.Key,
                        Name = "预警信息共" + res.Count() + "条",
                        GroupName = "预警-省级",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeRes in
                            TimeLineItems.Where(t => t.StartTime == res.Key && t.GroupName == "预警-省级").ToList())
                    {
                        TimeLineItems.Remove(removeRes);
                    }
                    TimeLineItems.Add(resTotal);
                }
            }

            var resCity = from s in m_Citys group s by s.StartTime;
            foreach (var res in resCity)
            {
                if (res.Count() > 1)
                {
                    var resTotal = new TimeLineItem
                    {
                        StartTime = res.Key,
                        Name = "预警信息共" + res.Count() + "条",
                        GroupName = "预警-市级",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeRes in
                            TimeLineItems.Where(t => t.StartTime == res.Key && t.GroupName == "预警-市级").ToList())
                    {
                        TimeLineItems.Remove(removeRes);
                    }
                    TimeLineItems.Add(resTotal);
                }
            }

            var resCounty = from s in m_Countys group s by s.StartTime;
            foreach (var res in resCounty)
            {
                if (res.Count() > 1)
                {
                    var resTotal = new TimeLineItem
                    {
                        StartTime = res.Key,
                        Name = "预警信息共" + res.Count() + "条",
                        GroupName = "预警-县级",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeRes in
                            TimeLineItems.Where(t => t.StartTime == res.Key && t.GroupName == "预警-县级").ToList())
                    {
                        TimeLineItems.Remove(removeRes);
                    }
                    TimeLineItems.Add(resTotal);
                }
            }

            #endregion

            // TimeLine聚组
            ConcludeGroupData(TimeLineItems, OnViewTimeLineItems);

            #region 最大降雨值（暂时弃用）

            //// 数据Model转换
            //// 从接口取回的数据Model转为绑定所需Model
            //foreach (var maxRainInfo in MaxRainInfos)
            //{
            //    var maxCountInfo = new MaxCountInfo();
            //    maxCountInfo.CountDateTime = DateTime.Parse(maxRainInfo.datetime);
            //    maxCountInfo.MaxValue = maxRainInfo.peakValue.ToString();
            //    maxCountInfo.Name = maxRainInfo.stationName;
            //    MaxCountInfos.Add(maxCountInfo);
            //}

            //// 数据排除
            //// 排除所规定时间段之外的数据
            //for (var i = MaxCountInfos.Count - 1; i >= 0; i--)
            //{
            //    if (MaxCountInfos[i].CountDateTime >= EndTime || MaxCountInfos[i].CountDateTime < StartTime)
            //    {
            //        MaxCountInfos.RemoveAt(i);
            //    }
            //}

            //// 数据时间段内扩充
            //// 不扩充则时间段内可能没有数据，导致上下时间线不对应
            //for (var time = StartTime; time < EndTime; time = time.AddHours(1))
            //{
            //    MaxCountInfos.Add(new MaxCountInfo
            //    {
            //        MaxValue = "",
            //        Name = "",
            //        CountDateTime = time
            //    });
            //}

            #endregion

            #region 台风绘制

            if (data.typeName == "11B01")//如果是台风
            {
                if (isShort)
                {
                    m_TyphoonData = DataManager.TyphoonData(data.id);//接口拉一下台风数据
                    CreateData.Instance.Typhoon(m_TyphoonData);//台风数据入库
                    LoadTyphoonCircle(m_TyphoonData.Last().bj_datetime, m_TyphoonData, isShort);//默认将最后一个时间点的台风风圈画出来
                }
                else
                {
                    m_WarningTyphoonData = DataManager.GetTyphoonByTime(Convert.ToDateTime(data.DateTime).ToString("yyyy-MM-dd"));//接口拉一下台风数据
                    CreateData.Instance.WarningTyphoon(m_WarningTyphoonData);//预警台风数据入库
                    var typhoon = new Typhoon
                    {
                        IsShort = isShort
                    };
                    MessageAggregator.GetMessage<CMATyphoonMessage>().Publish(typhoon);
                }

                //LoadTyphoonChartData();
                FirstLoadOtherAlarmData(data);
            }
            else
            {
                // 堆积柱图
                TimeLineWarningCounts = DataManager.getTimelineWarningCount(data.areaCode, data.typeName, StartTime.ToString("yyyy-MM-dd HH"), EndTime.ToString("yyyy-MM-dd HH"));
                GetOnViewCountInfos();
            }

            #endregion
        }

        /// <summary>
        ///     突发事件加载数据
        /// </summary>
        /// <param name="data"></param>
        private void LoadData(EventPageInto data)
        {
            FromMainPageType = "突发事件";
            EmergencyOrWarningChangeCommand.Execute(FromMainPageType);
            MaxCountInfos.Clear();
            StationInfos.Clear();

            TimeLineItems.Clear();
            OnViewTimeLineItems.Clear();

            // 取消泳道图时间背景
            CurrentTyphoonTime = DateTime.Today.AddDays(1);

            //获取突发事件名称
            Header = data.EventTitle;

            if (DataManager.Instance.IsTestData)
            {
                m_CurrentWarningId = "13073041600000_20191023164215";

                StartTime = new DateTime(2019, 7, 20, 10, 0, 0);
                EndTime = new DateTime(2019, 8, 2, 15, 00, 00);
                VisibleStartTime = new DateTime(2019, 7, 23, 17, 0, 0);
                VisibleEndTime = new DateTime(2019, 7, 24, 18, 0, 0);

                TimeLineWarnings = DataManager.getTimeLineWarningData("", "eventTest", "", "");
                TimeLineServices = DataManager.getTimelineServices("", "eventTest", "");

                FirstLoadEmergencyData(data);
            }
            else
            {
                var warnid = data.ProvinceCode;
                var eventtype = "";

                var warnTimeTemp = DateTime.Parse(data.EventBeginTime);
                var warnTime = warnTimeTemp.Date.AddHours(warnTimeTemp.Hour);
                var endTimeTemp = DateTime.Parse(data.EventEndTIme);
                var endTime = endTimeTemp.Date.AddHours(endTimeTemp.Hour);

                StartTime = warnTime.AddDays(-2);
                EndTime = endTime.AddDays(1);
                VisibleStartTime = warnTime.AddHours(-5);
                VisibleEndTime = warnTime.AddHours(5);

                FirstLoadEmergencyData(data);

                TimeLineWarnings = DataManager.getTimeLineWarningData(data.CountryCode, eventtype, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));

                TimeLineServices = DataManager.getTimelineServices(warnid, StartTime.ToString("yyyy-MM-dd HH"), EndTime.ToString("yyyy-MM-dd HH"));
                m_EventType = eventtype;
            }

            #region  灾情舆情/服务国家级/服务省级

            m_Disasters.Clear();
            m_NationServices.Clear();
            m_ProvinceServices.Clear();

            //补充最小时间
            var zqyqmin = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "灾情舆情指数",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(zqyqmin);

            var minNationservice = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "服务-国家级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minNationservice);

            var minservice = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "服务-省级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minservice);

            //补充最大时间
            var zqyqmax = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "灾情舆情指数",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(zqyqmax);

            var maxNationservice = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "服务-国家级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxNationservice);
            var maxservice = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "服务-省级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxservice);

            foreach (var servicedata in TimeLineServices)
            {
                DateTime sTime;
                var service = new TimeLineItem();
                if (DateTime.TryParse(servicedata.cdate, out sTime))
                {
                    service.StartTime = sTime;
                    service.Name = servicedata.title;
                    service.ServiceUrl = servicedata.url;
                    //进行三种服务类型区分
                    switch (servicedata.category)
                    {
                        case "nation":
                            service.GroupName = "服务-国家级";
                            m_NationServices.Add(service);
                            break;
                        case "zaiqing":
                            service.GroupName = "灾情舆情指数";
                            m_Disasters.Add(service);
                            break;
                        default:
                            service.GroupName = "服务-省级";
                            service.EventType = servicedata.category;
                            m_ProvinceServices.Add(service);
                            break;
                    }
                }
            }
            TimeLineItems.AddRange(m_NationServices);
            TimeLineItems.AddRange(m_ProvinceServices);
            TimeLineItems.AddRange(m_Disasters);

            //服务-国家级
            var ressernation = from s in m_NationServices.Where(t => t.GroupName == "服务-国家级") group s by s.StartTime;
            foreach (var ser in ressernation)
            {
                if (ser.Count() > 1)
                {
                    var serTotal = new TimeLineItem
                    {
                        StartTime = ser.Key,
                        Name = "国家级服务信息共" + ser.Count() + "条",
                        GroupName = "服务-国家级"
                    };
                    foreach (var removeSer in TimeLineItems.Where(t => t.StartTime == ser.Key && t.GroupName == "服务-国家级").ToList())
                    {
                        TimeLineItems.Remove(removeSer);
                    }
                    TimeLineItems.Add(serTotal);
                }
            }
            //服务-省级
            var resser = from s in m_ProvinceServices.Where(t => t.GroupName == "服务-省级") group s by s.StartTime;
            foreach (var ser in resser)
            {
                if (ser.Count() > 1)
                {
                    var serTotal = new TimeLineItem
                    {
                        StartTime = ser.Key,
                        Name = "省级服务信息共" + ser.Count() + "条",
                        GroupName = "服务-省级"
                    };
                    foreach (
                        var removeSer in
                            TimeLineItems.Where(t => t.StartTime == ser.Key && t.GroupName == "服务-省级").ToList())
                    {
                        TimeLineItems.Remove(removeSer);
                    }
                    TimeLineItems.Add(serTotal);
                }
            }
            //灾情舆情指数
            var zqyq = from s in m_Disasters.Where(t => t.GroupName == "灾情舆情指数") group s by s.StartTime;
            foreach (var ser in zqyq)
            {
                if (ser.Count() > 1)
                {
                    var serTotal = new TimeLineItem
                    {
                        StartTime = ser.Key,
                        Name = "灾情舆情信息共" + ser.Count() + "条",
                        GroupName = "灾情舆情指数"
                    };
                    foreach (
                        var removeSer in
                            TimeLineItems.Where(t => t.StartTime == ser.Key && t.GroupName == "灾情舆情指数").ToList())
                    {
                        TimeLineItems.Remove(removeSer);
                    }
                    TimeLineItems.Add(serTotal);
                }
            }

            #endregion

            #region 预警

            m_Nations.Clear();
            m_Provinces.Clear();
            m_Citys.Clear();
            m_Countys.Clear();

            //补充最小时间
            var minNation = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "预警-国家级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minNation);
            var minProvinces = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "预警-省级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minProvinces);
            var minCity = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "预警-市级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minCity);
            var minCounty = new TimeLineItem
            {
                StartTime = DateTime.MinValue,
                GroupName = "预警-县级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(minCounty);
            //补充最大时间
            var maxNation = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "预警-国家级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxNation);
            var maxProvinces = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "预警-省级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxProvinces);
            var maxCity = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "预警-市级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxCity);
            var maxCounty = new TimeLineItem
            {
                StartTime = DateTime.MaxValue.AddSeconds(-1),
                GroupName = "预警-县级",
                Duration = TimeSpan.FromSeconds(1)
            };
            TimeLineItems.Add(maxCounty);

            foreach (var warningData in TimeLineWarnings)
            {
                var yujing = new TimeLineItem();

                DateTime sTime;
                if (DateTime.TryParse(warningData.sendTime, out sTime))
                {
                    yujing.StartTime = sTime;
                    yujing.Name = warningData.headline;
                    yujing.Id = warningData.identifier;
                    yujing.Severity = warningData.severity.ToLower() ;
                    yujing.EventType = warningData.eventType;
                    var areaCode = warningData.level;

                    if (areaCode == "0")
                    {
                        yujing.GroupName = "预警-国家级";
                        m_Nations.Add(yujing);
                    }
                    else if (areaCode == "1")
                    {
                        yujing.GroupName = "预警-省级";
                        m_Provinces.Add(yujing);
                    }
                    else if (areaCode == "2")
                    {
                        yujing.GroupName = "预警-市级";
                        m_Citys.Add(yujing);
                    }
                    else
                    {
                        yujing.GroupName = "预警-县级";
                        m_Countys.Add(yujing);
                    }
                }
            }
            TimeLineItems.AddRange(m_Nations);
            TimeLineItems.AddRange(m_Provinces);
            TimeLineItems.AddRange(m_Citys);
            TimeLineItems.AddRange(m_Countys);

            var resNation = from s in m_Nations group s by s.StartTime;
            foreach (var res in resNation)
            {
                if (res.Count() > 1)
                {
                    var resTotal = new TimeLineItem
                    {
                        StartTime = res.Key,
                        Name = "预警信息共" + res.Count() + "条",
                        GroupName = "预警-国家级",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeRes in
                            TimeLineItems.Where(t => t.StartTime == res.Key && t.GroupName == "预警-国家级").ToList())
                    {
                        TimeLineItems.Remove(removeRes);
                    }
                    TimeLineItems.Add(resTotal);
                }
            }

            var resProvince = from s in m_Nations group s by s.StartTime;
            foreach (var res in resProvince)
            {
                if (res.Count() > 1)
                {
                    var resTotal = new TimeLineItem
                    {
                        StartTime = res.Key,
                        Name = "预警信息共" + res.Count() + "条",
                        GroupName = "预警-省级",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeRes in
                            TimeLineItems.Where(t => t.StartTime == res.Key && t.GroupName == "预警-省级").ToList())
                    {
                        TimeLineItems.Remove(removeRes);
                    }
                    TimeLineItems.Add(resTotal);
                }
            }

            var resCity = from s in m_Citys group s by s.StartTime;
            foreach (var res in resCity)
            {
                if (res.Count() > 1)
                {
                    var resTotal = new TimeLineItem
                    {
                        StartTime = res.Key,
                        Name = "预警信息共" + res.Count() + "条",
                        GroupName = "预警-市级",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeRes in
                            TimeLineItems.Where(t => t.StartTime == res.Key && t.GroupName == "预警-市级").ToList())
                    {
                        TimeLineItems.Remove(removeRes);
                    }
                    TimeLineItems.Add(resTotal);
                }
            }

            var resCounty = from s in m_Countys group s by s.StartTime;
            foreach (var res in resCounty)
            {
                if (res.Count() > 1)
                {
                    var resTotal = new TimeLineItem
                    {
                        StartTime = res.Key,
                        Name = "预警信息共" + res.Count() + "条",
                        GroupName = "预警-县级",
                        EventType = "alarm"
                    };
                    foreach (
                        var removeRes in
                            TimeLineItems.Where(t => t.StartTime == res.Key && t.GroupName == "预警-县级").ToList())
                    {
                        TimeLineItems.Remove(removeRes);
                    }
                    TimeLineItems.Add(resTotal);
                }
            }

            #endregion

            ConcludeGroupData(TimeLineItems, OnViewTimeLineItems);
        }

        /// <summary>
        ///     根据时间范围聚组
        /// </summary>
        /// <param name="orginCollection"></param>
        /// <param name="showCollection"></param>
        private void ConcludeGroupData(ObservableCollection<TimeLineItem> orginCollection, ObservableCollection<TimeLineItem> showCollection)
        {
            if (orginCollection == null || showCollection == null)
            {
                return;
            }

            orginCollection.ToList().ForEach(t => t.InculdeItems.Clear());
            showCollection.Clear();

            //按组名分组
            var groups = orginCollection.GroupBy(t => t.GroupName);
            foreach (var group in groups)
            {
                var orderbytime = group.OrderBy(t => t.StartTime).ToList();

                for (var i = 0; i < orderbytime.Count(); i++)
                {
                    var thisitem = orderbytime[i];
                    //当前不是最后一个数据时
                    if (i < orderbytime.Count() - 1)
                    {
                        var temp = i + 1;
                        var nextitem = orderbytime[temp];


                        if (nextitem.StartTime == thisitem.StartTime)
                        {
                            nextitem = orderbytime.FirstOrDefault(t => t.StartTime > nextitem.StartTime);
                        }
                        //当下个数据距离当前数据不到1小时,持续到下个数据开始
                        if (nextitem != null && nextitem.StartTime - thisitem.StartTime <= TimeSpan.FromHours(1))
                        {
                            thisitem.Duration = nextitem.StartTime - thisitem.StartTime;
                            if (thisitem.Duration == TimeSpan.Zero)
                            {
                                thisitem.Duration = TimeSpan.FromHours(1);
                            }
                        }
                        //不然最长1个小时
                        else
                        {
                            thisitem.Duration = TimeSpan.FromHours(1);
                        }
                        showCollection.Add(thisitem);
                    }
                    else
                    {
                        thisitem.Duration = TimeSpan.FromHours(1);
                    }
                }
            }

            // 选中当前事件
            foreach (var timeLineItem in showCollection)
            {
                if (timeLineItem.Id == m_CurrentWarningId)
                {
                    timeLineItem.IsSelected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 第一次加载其他类型预警数据
        /// </summary>
        /// <param name="eventInfo"></param>
        private async void FirstLoadOtherAlarmData(ScutcheonSelectGroupMessage data)
        {
            //获取最近站点
            var closestStationList = DataManager.GetClosestStation(data.lon.ToString(), data.lat.ToString(), "1");
           
            EmergencyBarInfos = await LoadEmergencyBarInfoDataAsync(closestStationList.FirstOrDefault().stationId);

            // 更改标题显示
            TimeBarTitle = m_ClosestStation[closestStationList.FirstOrDefault().stationId] + "气象";
        }

        /// <summary>
        /// 第一次加载突发事件数据
        /// </summary>
        /// <param name="eventInfo"></param>
        private async void FirstLoadEmergencyData(EventPageInto eventInfo)
        {
            //获取突发事件最近站点
            var closestStationList = DataManager.GetClosestStation(eventInfo.EventLon.ToString(), eventInfo.EventLat.ToString(), "5");
            // 突发事件散点入库
            CreateData.Instance.EventInsertToDatabaseAsync(eventInfo, closestStationList);
            // 数据入库
            CreateData.Instance.ClosestStationToDatabaseAsync(closestStationList);

            // 建立字典
            m_ClosestStation.Clear();
            foreach (var station in closestStationList)
            {
                m_ClosestStation.Add(station.stationId, station.stationNa);
            }

            EmergencyBarInfos = await LoadEmergencyBarInfoDataAsync(closestStationList.FirstOrDefault().stationId);

            // 更改标题显示
            TimeBarTitle = m_ClosestStation[closestStationList.FirstOrDefault().stationId] + "气象";

            //// 加载图表
            //LoadEmergencyBarInfoData(closestStationList.FirstOrDefault().stationId);
        }

        /// <summary>
        /// 加载突发事件组合图数据
        /// </summary>
        /// <param name="stationId">站点ID</param>
        private void LoadEmergencyBarInfoData(string stationId)
        {
            EmergencyBarInfos.Clear();

            if (!DataManager.Instance.IsTestData)
            {
                var data1 = DataManager.GetEmergencyForecastData(stationId, StartTime.ToString("yyyyMMddHH0000"), EndTime.ToString("yyyyMMddHH0000"));
                var data2 = DataManager.GetEmergencyForecastData("56693", StartTime.ToString("yyyyMMddHH0000"), EndTime.ToString("yyyyMMddHH0000"));
                foreach (var item in data1)
                {
                    EmergencyBarInfo singleStation = new EmergencyBarInfo()
                    {
                        Time = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                        BarTime = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                        RainNumber = double.Parse(item.pRE_1h),
                        Temperature = double.Parse(String.IsNullOrWhiteSpace(item.tEM) ? "NaN" : item.tEM),
                        //Wind = double.Parse(item.wIN_S_Avg_10mi),
                        //WindDirection = double.Parse(item.wIN_D_Avg_10mi)
                    };
                    EmergencyBarInfos.Add(singleStation);
                }

                foreach (var item in data2)
                {
                    var selectItem = EmergencyBarInfos.First(t => t.Time == DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture));
                    selectItem.Wind = double.Parse(String.IsNullOrWhiteSpace(item.wIN_S_Avg_10mi) ? "NaN" : item.wIN_S_Avg_10mi);
                    selectItem.WindDirection = double.Parse(String.IsNullOrWhiteSpace(item.wIN_D_Avg_10mi) ? "0" : item.wIN_D_Avg_10mi);
                }
            }
            else
            {
                var data3 = DataManager.GetEmergencyForecastData("56693", StartTime.ToString("yyyyMMddHH0000"), EndTime.ToString("yyyyMMddHH0000"));
                foreach (var item in data3)
                {
                    var singleStation = new EmergencyBarInfo
                    {
                        Time = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                        BarTime = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                        RainNumber = double.Parse(item.pRE_1h),
                        Temperature = double.Parse(String.IsNullOrWhiteSpace(item.tEM) ? "NaN" : item.tEM),
                        Wind = double.Parse(String.IsNullOrWhiteSpace(item.wIN_S_Avg_10mi) ? "NaN" : item.wIN_S_Avg_10mi),
                        WindDirection = double.Parse(String.IsNullOrWhiteSpace(item.wIN_D_Avg_10mi) ? "0" : item.wIN_D_Avg_10mi)
                    };

                    EmergencyBarInfos.Add(singleStation);
                }
            }


            // 数据排除
            // 排除所规定时间段之外的数据
            for (var i = EmergencyBarInfos.Count - 1; i >= 0; i--)
            {
                if (EmergencyBarInfos[i].Time > EndTime || EmergencyBarInfos[i].Time < StartTime)
                {
                    EmergencyBarInfos.RemoveAt(i);
                }
            }

            // 数据时间段内扩充
            // 不扩充则时间段内可能没有数据，导致上下时间线不对应
            for (var time = StartTime; time <= EndTime; time = time.AddHours(1))
            {
                var tempList = EmergencyBarInfos.Where(s => s.Time == time).ToList();

                if (tempList.Count > 0)
                {
                    continue;
                }

                EmergencyBarInfos.Add(new EmergencyBarInfo
                {
                    Time = time,
                    BarTime = time,
                    RainNumber = double.NaN,
                    Temperature = double.NaN,
                    Wind = double.NaN,
                    WindDirection = 0
                });
            }

            var temp = from t in EmergencyBarInfos orderby t.Time ascending select t;
            EmergencyBarInfos = new ObservableCollection<EmergencyBarInfo>(temp.ToList());
            var theLast = EmergencyBarInfos.Last();
            EmergencyBarInfos.Remove(theLast);
            EmergencyBarInfos.Add(new EmergencyBarInfo
            {
                Time = theLast.Time,
                Temperature = theLast.Temperature,
                Wind = theLast.Wind,
                WindDirection = theLast.WindDirection,
                RainNumber = theLast.RainNumber,
                BarTime = theLast.BarTime.AddHours(-1)
            });

            // 更改标题显示
            TimeBarTitle = m_ClosestStation[stationId] + "气象";

            // 更改图表最大最小值，解决图表数值切割问题

            ChartSettingParameter chartSettingParameter = new ChartSettingParameter()
            {
                ChartName = ChartNameEnum.EmergencyRainChart,
                MinimumValue = 0,
                MaximumValue = EmergencyBarInfos.Max(x => x.RainNumber)
            };

            MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);

            if (EmergencyBarInfos.Any(x => !double.IsNaN(x.Temperature)))
            {
                chartSettingParameter = new ChartSettingParameter()
                {
                    ChartName = ChartNameEnum.EmergencyTemperatureChart,
                    MinimumValue = EmergencyBarInfos.Where(x => !double.IsNaN(x.Temperature)).Min(x => x.Temperature),
                    MaximumValue = EmergencyBarInfos.Max(x => x.Temperature)
                };

                MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);
            }

            if (EmergencyBarInfos.Any(x => !double.IsNaN(x.Wind)))
            {
                chartSettingParameter = new ChartSettingParameter()
                {
                    ChartName = ChartNameEnum.EmergencyWindChart,
                    MinimumValue = EmergencyBarInfos.Where(x => !double.IsNaN(x.Wind)).Min(x => x.Wind),
                    MaximumValue = EmergencyBarInfos.Max(x => x.Wind)
                };

                MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);
            }
        }

        private async Task<ObservableCollection<EmergencyBarInfo>> LoadEmergencyBarInfoDataAsync(string stationId)
        {
            return await Task.Run(() =>
            {
                var result = new ObservableCollection<EmergencyBarInfo>();

                if (!DataManager.Instance.IsTestData)
                {
                    var data1 = DataManager.GetEmergencyForecastData(stationId, StartTime.ToString("yyyyMMddHH0000"), EndTime.ToString("yyyyMMddHH0000"));
                    var data2 = DataManager.GetEmergencyForecastData("56693", StartTime.ToString("yyyyMMddHH0000"), EndTime.ToString("yyyyMMddHH0000"));
                    foreach (var item in data1)
                    {
                        EmergencyBarInfo singleStation = new EmergencyBarInfo()
                        {
                            Time = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                            BarTime = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                            RainNumber = double.Parse(item.pRE_1h),
                            Temperature = double.Parse(String.IsNullOrWhiteSpace(item.tEM) ? "NaN" : item.tEM),
                            //Wind = double.Parse(item.wIN_S_Avg_10mi),
                            //WindDirection = double.Parse(item.wIN_D_Avg_10mi)
                        };
                        result.Add(singleStation);
                    }

                    foreach (var item in data2)
                    {
                        var selectItem = result.First(t => t.Time == DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture));
                        selectItem.Wind = double.Parse(String.IsNullOrWhiteSpace(item.wIN_S_Avg_10mi) ? "NaN" : item.wIN_S_Avg_10mi);
                        selectItem.WindDirection = double.Parse(String.IsNullOrWhiteSpace(item.wIN_D_Avg_10mi) ? "0" : item.wIN_D_Avg_10mi);
                    }
                }
                else
                {
                    var data3 = DataManager.GetEmergencyForecastData("56693", StartTime.ToString("yyyyMMddHH0000"), EndTime.ToString("yyyyMMddHH0000"));
                    foreach (var item in data3)
                    {
                        var singleStation = new EmergencyBarInfo
                        {
                            Time = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                            BarTime = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                            RainNumber = double.Parse(item.pRE_1h),
                            Temperature = double.Parse(String.IsNullOrWhiteSpace(item.tEM) ? "NaN" : item.tEM),
                            Wind = double.Parse(String.IsNullOrWhiteSpace(item.wIN_S_Avg_10mi) ? "NaN" : item.wIN_S_Avg_10mi),
                            WindDirection = double.Parse(String.IsNullOrWhiteSpace(item.wIN_D_Avg_10mi) ? "0" : item.wIN_D_Avg_10mi)
                        };

                        result.Add(singleStation);
                    }
                }


                // 数据排除
                // 排除所规定时间段之外的数据
                for (var i = result.Count - 1; i >= 0; i--)
                {
                    if (result[i].Time > EndTime || result[i].Time < StartTime)
                    {
                        result.RemoveAt(i);
                    }
                }

                // 数据时间段内扩充
                // 不扩充则时间段内可能没有数据，导致上下时间线不对应
                for (var time = StartTime; time <= EndTime; time = time.AddHours(1))
                {
                    var tempList = result.Where(s => s.Time == time).ToList();

                    if (tempList.Count > 0)
                    {
                        continue;
                    }

                    result.Add(new EmergencyBarInfo
                    {
                        Time = time,
                        BarTime = time,
                        RainNumber = double.NaN,
                        Temperature = double.NaN,
                        Wind = double.NaN,
                        WindDirection = 0
                    });
                }

                var temp = from t in result orderby t.Time ascending select t;
                result = new ObservableCollection<EmergencyBarInfo>(temp.ToList());
                var theLast = result.Last();
                result.Remove(theLast);
                result.Add(new EmergencyBarInfo
                {
                    Time = theLast.Time,
                    Temperature = theLast.Temperature,
                    Wind = theLast.Wind,
                    WindDirection = theLast.WindDirection,
                    RainNumber = theLast.RainNumber,
                    BarTime = theLast.BarTime.AddHours(-1)
                });

                // 更改图表最大最小值，解决图表数值切割问题

                ChartSettingParameter chartSettingParameter = new ChartSettingParameter()
                {
                    ChartName = ChartNameEnum.EmergencyRainChart,
                    MinimumValue = 0,
                    MaximumValue = result.Max(x => x.RainNumber)
                };

                MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);

                if (result.Any(x => !double.IsNaN(x.Temperature)))
                {
                    chartSettingParameter = new ChartSettingParameter()
                    {
                        ChartName = ChartNameEnum.EmergencyTemperatureChart,
                        MinimumValue = result.Where(x => !double.IsNaN(x.Temperature)).Min(x => x.Temperature),
                        MaximumValue = result.Max(x => x.Temperature)
                    };

                    MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);
                }

                if (result.Any(x => !double.IsNaN(x.Wind)))
                {
                    chartSettingParameter = new ChartSettingParameter()
                    {
                        ChartName = ChartNameEnum.EmergencyWindChart,
                        MinimumValue = result.Where(x => !double.IsNaN(x.Wind)).Min(x => x.Wind),
                        MaximumValue = result.Max(x => x.Wind)
                    };

                    MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);
                }

                return result;
            });
        }

        /// <summary>
        /// 加载台风图表数据
        /// </summary>
        private void LoadTyphoonChartData()
        {
            TyphoonChartData.Clear();

            Random random = new Random();
            for (var time = StartTime; time <= EndTime; time = time.AddHours(1))
            {
                TyphoonChartData.Add(new TyphoonChartModel()
                {
                    Time = time,
                    BarTime = time,
                    WindSpeed = random.Next(15, 30),
                    Pressure = (time.Hour % 3 == 0) ? 1000 + random.Next(-150, 150) : 0,
                    IsVisible = (time.Hour % 3 == 0)
                });
            }

            var temp = from t in TyphoonChartData orderby t.Time ascending select t;
            TyphoonChartData = new ObservableCollection<TyphoonChartModel>(temp.ToList());
            var theLast = TyphoonChartData.Last();
            TyphoonChartData.Remove(theLast);
            TyphoonChartData.Add(new TyphoonChartModel
            {
                Time = theLast.BarTime,
                WindSpeed = theLast.WindSpeed,
                Pressure = theLast.Pressure,
                IsVisible = theLast.IsVisible,
                BarTime = theLast.BarTime.AddHours(-1)
            });

            ChartSettingParameter chartSettingParameter = new ChartSettingParameter()
            {
                ChartName = ChartNameEnum.TyphoonWindChart,
                MinimumValue = TyphoonChartData.Min(x => x.WindSpeed),
                MaximumValue = TyphoonChartData.Max(x => x.WindSpeed)
            };

            MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);

            chartSettingParameter = new ChartSettingParameter()
            {
                ChartName = ChartNameEnum.TyphoonAirPressureChart,
                MinimumValue = 0,
                MaximumValue = TyphoonChartData.Max(x => x.Pressure)
            };

            MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);

        }

        // 堆积柱图Model转为界面上绑定的Model
        private void GetOnViewCountInfos()
        {
            if (TimeLineWarningCounts != null)
            {
                OnViewCountInfos.Clear();

                var blueTypeInfo = new CountTypeInfo();
                blueTypeInfo.Type = "蓝色告警";
                blueTypeInfo.CountInfos = new ObservableCollection<CountInfo>();
                OnViewCountInfos.Add(blueTypeInfo);

                var yellowTypeInfo = new CountTypeInfo();
                yellowTypeInfo.Type = "黄色告警";
                yellowTypeInfo.CountInfos = new ObservableCollection<CountInfo>();
                OnViewCountInfos.Add(yellowTypeInfo);

                var orangeTypeInfo = new CountTypeInfo();
                orangeTypeInfo.Type = "橙色告警";
                orangeTypeInfo.CountInfos = new ObservableCollection<CountInfo>();
                OnViewCountInfos.Add(orangeTypeInfo);

                var redTypeInfo = new CountTypeInfo();
                redTypeInfo.Type = "红色告警";
                redTypeInfo.CountInfos = new ObservableCollection<CountInfo>();
                OnViewCountInfos.Add(redTypeInfo);

                // 数据Model转换
                foreach (var timeLineWarningCount in TimeLineWarningCounts)
                {
                    var time = GetDateTimeFormat(timeLineWarningCount.Time);
                    if (timeLineWarningCount.RedNumber > 0)
                    {
                        var count = new CountInfo();
                        count.CountValue = timeLineWarningCount.RedNumber;
                        count.CountDateTime = time;
                        count.EventType = m_EventType;
                        count.Stations = timeLineWarningCount.StationsRed;
                        redTypeInfo.CountInfos.Add(count);
                    }
                    if (timeLineWarningCount.OrangeNumber > 0)
                    {
                        var count = new CountInfo();
                        count.CountValue = timeLineWarningCount.OrangeNumber;
                        count.CountDateTime = time;
                        count.EventType = m_EventType;
                        count.Stations = timeLineWarningCount.StationsOrange;
                        orangeTypeInfo.CountInfos.Add(count);
                    }
                    if (timeLineWarningCount.YellowNumber > 0)
                    {
                        var count = new CountInfo();
                        count.CountValue = timeLineWarningCount.YellowNumber;
                        count.CountDateTime = time;
                        count.EventType = m_EventType;
                        count.Stations = timeLineWarningCount.StationsYellow;
                        yellowTypeInfo.CountInfos.Add(count);
                    }
                    if (timeLineWarningCount.BlueNumber > 0)
                    {
                        var count = new CountInfo();
                        count.CountValue = timeLineWarningCount.BlueNumber;
                        count.CountDateTime = time;
                        count.EventType = m_EventType;
                        count.Stations = timeLineWarningCount.StationsBlue;
                        blueTypeInfo.CountInfos.Add(count);
                    }
                }

                // 数据排除
                // 排除所规定时间段之外的数据
                foreach (var countInfo in OnViewCountInfos)
                {
                    for (var i = countInfo.CountInfos.Count - 1; i >= 0; i--)
                    {
                        if (countInfo.CountInfos[i].CountDateTime >= EndTime ||
                            countInfo.CountInfos[i].CountDateTime < StartTime)
                        {
                            countInfo.CountInfos.RemoveAt(i);
                        }
                    }
                }

                // 数据时间段内扩充
                // 不扩充则时间段内可能没有数据，导致上下时间线不对应
                if (DataManager.Instance.IsTestData)
                {
                    //for (var time = StartTime; time < EndTime; time = time.AddHours(1))
                    //{
                    //    blueTypeInfo.CountInfos.Add(new CountInfo
                    //    {
                    //        CountValue = 2,
                    //        CountDateTime = time
                    //    });
                    //}
                }
                else
                {
                    for (var time = StartTime; time < EndTime; time = time.AddHours(1))
                    {
                        blueTypeInfo.CountInfos.Add(new CountInfo
                        {
                            CountValue = 0,
                            CountDateTime = time
                        });
                    }
                }
            }

            TimeBarTitle = "实况";
        }

        // 时间转换
        private DateTime GetDateTimeFormat(string datetimeString)
        {
            var dt = DateTime.MinValue;

            if (datetimeString.Length < 14)
            {
                return dt;
            }

            int year;
            var month = 0;
            var day = 0;
            var hour = 0;
            var minute = 0;
            var second = 0;
            var flag = int.TryParse(datetimeString.Substring(0, 4), out year);
            flag = flag && int.TryParse(datetimeString.Substring(4, 2), out month);
            flag = flag && int.TryParse(datetimeString.Substring(6, 2), out day);
            flag = flag && int.TryParse(datetimeString.Substring(8, 2), out hour);
            flag = flag && int.TryParse(datetimeString.Substring(10, 2), out minute);
            flag = flag && int.TryParse(datetimeString.Substring(12, 2), out second);
            if (flag)
            {
                dt = new DateTime(year, month, day, hour, minute, second);
            }

            return dt;
        }

        /// <summary>
        /// 日期赋值
        /// </summary>
        private void DateAssignment()
        {
            CurrentDate = this.VisibleStartTime.Date;
        }

        #endregion

        #region 公共方法

        /// <summary>
        ///     加载单站柱图数据
        /// </summary>
        /// <param name="station">站点信息</param>
        /// <param name="timespan">时间跨度</param>
        public void LoadSingleBarData(StationInfo station, string timespan)
        {
            Rains.Clear();

            var stationDetailData = DataManager.GetRainLiveAndForecastData(station.StationId,
                StartTime.ToString("yyyyMMddHH0000"), EndTime.ToString("yyyyMMddHH0000"));
            var data = stationDetailData[0]; //默认一小时数据

            switch (timespan)
            {
                case "1":
                    data = stationDetailData[0];
                    break;
                case "3":
                    data = stationDetailData[1];
                    break;
                case "6":
                    data = stationDetailData[2];
                    break;
                case "12":
                    data = stationDetailData[3];
                    break;
                case "24":
                    data = stationDetailData[4];
                    break;
            }

            var index = 0;

            if (DataManager.Instance.IsTestData)
            {
                var count = data.Length;
                for (var time = StartTime; time < EndTime; time = time.AddHours(1))
                {
                    var singleStation = new SingleStationInfo
                    {
                        Time = time,
                        RainNumber = index < count - 1 ? double.Parse(data[index]) : 0
                    };
                    singleStation.AlarmLevel = 1;
                    Rains.Add(singleStation);
                    index++;
                }
            }
            else
            {
                for (var time = StartTime; time < EndTime; time = time.AddHours(1))
                {
                    var singleStation = new SingleStationInfo
                    {
                        Time = time,
                        RainNumber = double.Parse(data[index])
                    };
                    //if (signTimes.Contains(singleStation.Time.ToString("yyyyMMddHH0000")))
                    //{
                    //    singleStation.AlarmLevel = 2;
                    //}
                    //else
                    //{
                    //    singleStation.AlarmLevel = 1;
                    //}
                    singleStation.AlarmLevel = 1;
                    Rains.Add(singleStation);
                    index++;
                }
            }
        }

        /// <summary>
        ///     加载单站大风折线图数据
        /// </summary>
        /// <param name="station"></param>
        public void LoadWindLineData(StationInfo station)
        {
            Winds.Clear();

            if (DataManager.Instance.IsTestData)
            {
                var mRandom = new Random();
                for (var time = StartTime; time <= EndTime; time = time.AddHours(1))
                {
                    var wind = new SingleStationWindInfo
                    {
                        Time = time,
                        Wind = mRandom.Next(0, 13),
                        WindAngle = mRandom.Next(0, 361),
                        AvrWind = mRandom.Next(0, 13),
                        AvrWindAngle = mRandom.Next(0, 361)
                    };
                    Winds.Add(wind);
                }
            }
            else
            {
                // 通过接口获取数据
                var waveData = DataManager.GetWindData(station.StationId, StartTime.ToString("yyyyMMddHH0000"), EndTime.ToString("yyyyMMddHH0000"));
                if (waveData == null)
                {
                    return;
                }
                var index = 0;


                for (var time = StartTime; time < EndTime; time = time.AddHours(1))
                {
                    var singleStation = new SingleStationWindInfo
                    {
                        Time = time,
                        Wind = double.Parse(waveData[0][index]),
                        WindAngle = double.Parse(waveData[1][index]),
                        AvrWind = double.Parse(waveData[2][index]),
                        AvrWindAngle = double.Parse(waveData[3][index])
                    };
                    Winds.Add(singleStation);
                    index++;
                }


                // 数据排除
                // 排除所规定时间段之外的数据
                for (var i = Winds.Count - 1; i >= 0; i--)
                {
                    if (Winds[i].Time > EndTime || Winds[i].Time < StartTime)
                    {
                        Winds.RemoveAt(i);
                    }
                }

                // 数据时间段内扩充
                // 不扩充则时间段内可能没有数据，导致上下时间线不对应
                for (var time = StartTime; time <= EndTime; time = time.AddHours(1))
                {
                    var tempList = Winds.Where(s => s.Time == time).ToList();

                    if (tempList.Count > 0)
                    {
                        continue;
                    }

                    Winds.Add(new SingleStationWindInfo
                    {
                        Time = time,
                        Wind = double.NaN,
                        AvrWind = double.NaN,
                        WindAngle = 0,
                        AvrWindAngle = 0,
                    });
                }

                // 数据排序
                var temp = from t in Winds orderby t.Time ascending select t;
                Winds = new ObservableCollection<SingleStationWindInfo>(temp.ToList());

                // 去除最后数据并再次添加
                // 达到重绘图表目的让时间线对应
                var theLast = Winds.Last();
                Winds.Remove(theLast);
                Winds.Add(new SingleStationWindInfo
                {
                    Time = theLast.Time,
                    Wind = theLast.Wind,
                    AvrWind = theLast.AvrWind,
                    AvrWindAngle = theLast.AvrWindAngle,
                    WindAngle = theLast.WindAngle
                });

                // 标题名字更改
                TimeBarTitle = string.Format("{0}实况", station.StationName);

                if (Winds.Count == 0) return;


                // 更改图表最大最小值，解决图表数值切割问题

                if (Winds.Any(x => !double.IsNaN(x.Wind)))
                {
                    ChartSettingParameter chartSettingParameter = new ChartSettingParameter()
                    {
                        ChartName = ChartNameEnum.WindChart,
                        MinimumValue = Winds.Where(x => !double.IsNaN(x.Wind)).Min(x => x.Wind),
                        MaximumValue = Winds.Max(x => x.Wind)
                    };

                    MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);
                }
            }
        }

        /// <summary>
        ///     加载单站寒潮折线图数据
        /// </summary>
        /// <param name="station"></param>
        /// <param name="timespan"></param>
        public void LoadColdWaveLineData(StationInfo station, string timespan)
        {
            ColdWaves.Clear();

            // 通过接口获取数据
            var coldWaveData = DataManager.GetColdWaveData(station.StationId, StartTime.ToString("yyyyMMddHH0000"), EndTime.ToString("yyyyMMddHH0000"));
            if (coldWaveData == null)
            {
                return;
            }
            var data = coldWaveData[0]; //默认48小时数据

            switch (timespan)
            {
                case "24":
                    data = coldWaveData[1];
                    break;
                case "48":
                    data = coldWaveData[0];
                    break;
            }

            var index = 0;

            if (DataManager.Instance.IsTestData)
            {
                var count = data.Length;
                for (var time = StartTime; time < EndTime; time = time.AddHours(1))
                {
                    var singleStation = new SingleStationColdWaveInfo
                    {
                        Time = time,
                        TemValue = index < count - 1 ? double.Parse(data[index]) : 0,
                        TemAngle = 0,
                        WindValue = index < count - 1 ? double.Parse(coldWaveData[2][index]) : 0,
                        WindAngle = index < count - 1 ? double.Parse(coldWaveData[3][index]) : 0
                    };
                    ColdWaves.Add(singleStation);
                    index++;
                }
            }
            else
            {
                for (var time = StartTime; time < EndTime; time = time.AddHours(1))
                {
                    var singleStation = new SingleStationColdWaveInfo
                    {
                        Time = time,
                        TemValue = double.Parse(data[index]),
                        TemAngle = double.Parse(data[index]) > 0 ? 180 : 0,
                        WindValue = double.Parse(coldWaveData[2][index]),
                        WindAngle = double.Parse(coldWaveData[3][index])
                    };
                    ColdWaves.Add(singleStation);
                    index++;
                }
            }

            // 数据排除
            // 排除所规定时间段之外的数据
            for (var i = ColdWaves.Count - 1; i >= 0; i--)
            {
                if (ColdWaves[i].Time > EndTime || ColdWaves[i].Time < StartTime)
                {
                    ColdWaves.RemoveAt(i);
                }
            }

            // 数据时间段内扩充
            // 不扩充则时间段内可能没有数据，导致上下时间线不对应
            for (var time = StartTime; time <= EndTime; time = time.AddHours(1))
            {
                var tempList = ColdWaves.Where(s => s.Time == time).ToList();

                if (tempList.Count > 0)
                {
                    continue;
                }

                ColdWaves.Add(new SingleStationColdWaveInfo
                {
                    Time = time,
                    WindValue = double.NaN,
                    TemValue = double.NaN,
                    WindAngle = 0,
                    TemAngle = 0
                });
            }

            // 数据排序
            var temp = from t in ColdWaves orderby t.Time ascending select t;
            ColdWaves = new ObservableCollection<SingleStationColdWaveInfo>(temp.ToList());

            // 去除最后数据并再次添加
            // 达到重绘图表目的让时间线对应
            var theLast = ColdWaves.Last();
            ColdWaves.Remove(theLast);
            ColdWaves.Add(new SingleStationColdWaveInfo
            {
                Time = theLast.Time,
                WindValue = theLast.WindValue,
                TemValue = theLast.TemValue,
                WindAngle = theLast.WindAngle,
                TemAngle = theLast.TemAngle
            });

            // 标题名字更改
            TimeBarTitle = string.Format("{0}实况", station.StationName);

            if (ColdWaves.Count == 0) return;


            // 更改图表最大最小值，解决图表数值切割问题

            if (ColdWaves.Any(x => !double.IsNaN(x.TemValue)))
            {
                ChartSettingParameter chartSettingParameter = new ChartSettingParameter()
                {
                    ChartName = ChartNameEnum.ColdWaveTemperatureChart,
                    MinimumValue = ColdWaves.Where(x => !double.IsNaN(x.TemValue)).Min(x => x.TemValue),
                    MaximumValue = ColdWaves.Max(x => x.TemValue)
                };

                MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);
            }


            if (ColdWaves.Any(x => !double.IsNaN(x.WindValue)))
            {
                ChartSettingParameter chartSettingParameter = new ChartSettingParameter()
                {
                    ChartName = ChartNameEnum.ColdWaveWindChart,
                    MinimumValue = ColdWaves.Where(x => !double.IsNaN(x.WindValue)).Min(x => x.WindValue),
                    MaximumValue = ColdWaves.Max(x => x.WindValue)
                };

                MessageAggregator.GetMessage<CMAChartSettingMessage>().Publish(chartSettingParameter);
            }
        }

        /// <summary>
        ///     加载预警弹窗数据
        /// </summary>
        /// <param name="alarmId"></param>
        public void LoadAlarmDetailWindowData(string alarmId)
        {
            if (DataManager.Instance.IsTestData)
            {
                CurrentAlarmDetail = new AlarmDetailModel
                {
                    AlarmTitle = "应急管理部、国家林业和草原局、中国气象应急管理部",
                    AlarmTime = DateTime.Now,
                    AlarmLevel = "orange",
                    AlarmType = "11B03",
                    AlarmDescription =
                        "近30天，内蒙古东北部、辽宁西南部和中北部、吉林中南部、黑龙江西北部、四川西南部、云南中部的大部分地区降水较常年同期偏少5成以上，部分地区偏少8成以上，气温较常年同期偏高1-2℃，出现中到重度气象干旱，局部地区达到特旱；预计未来3天，降水仍然偏少，气温较常年同期偏高1-2℃，出现中到重度气象干旱，局部地区达到特旱。",
                    AlarmPeopleNumber = 2064,
                    AlarmInformation = new AlarmInformation
                    {
                        SendNumber = 77,
                        BrowseNumber = 65,
                        RelayNumber = 46
                    },
                    AlarmProcess = new List<AlarmProcess>
                    {
                        new AlarmProcess
                        {
                            action = "录入",
                            @operator = "操作员1",
                            time = DateTime.Now
                        },
                        new AlarmProcess
                        {
                            action = "应急办签发",
                            @operator = "操作员2",
                            time = DateTime.Now
                        },
                        new AlarmProcess
                        {
                            action = "审核",
                            @operator = "操作员3",
                            time = DateTime.Now
                        }
                    },
                    AlarmMethod = new AlarmMethod
                    {
                        Message = 3021,
                        Web = 650,
                        WeiBo = 35918,
                        WeChat = 6752,
                        Mail = 8,
                        Fax = 33,
                        Horn = 254,
                        Screen = 85
                    }
                };
                CurrentAlarmDetail.AlarmDescription += CurrentAlarmDetail.AlarmDescription;
                CurrentAlarmDetail.AlarmDescription += CurrentAlarmDetail.AlarmDescription;
                CurrentAlarmDetail.AlarmDescription += CurrentAlarmDetail.AlarmDescription;
                CurrentAlarmDetail.AlarmDescription += CurrentAlarmDetail.AlarmDescription;
            }
            else
            {
                // 获取标题、描述、时间、等级
                var headData = DataManager.GetWarningByIdentifier(alarmId);
                CurrentAlarmDetail = new AlarmDetailModel
                {
                    AlarmTitle = headData.headline,
                    AlarmDescription = headData.description,
                    AlarmTime = DateTime.Parse(headData.sendTime),
                    AlarmLevel = headData.severity,
                    AlarmType = headData.eventType
                };

                // 获取责任人数量
                var dutyPersonNumber = DataManager.GetAlarmDutyPersonNumber(alarmId);
                CurrentAlarmDetail.AlarmPeopleNumber = int.Parse(dutyPersonNumber.num);

                // 获取信息员数据
                var infoPeopleData = DataManager.getTimelineWarningDetailPeopleInfo(alarmId, "month");
                CurrentAlarmDetail.AlarmInformation = new AlarmInformation
                {
                    BrowseNumber = int.Parse(infoPeopleData.FirstOrDefault().read),
                    RelayNumber = int.Parse(infoPeopleData.FirstOrDefault().send),
                    SendNumber = int.Parse(infoPeopleData.FirstOrDefault().org)
                };

                // 获取预警流程
                var list = from t in DataManager.GetSendProcessInfoAboutEarlyWarning(alarmId)
                           orderby t.time ascending
                           select t;
                CurrentAlarmDetail.AlarmProcess = list.ToList();

                // 获取预警手段
                CurrentAlarmDetail.AlarmMethod = new AlarmMethod();
                var alarmMethod = DataManager.getSendResultInfoAboutEarlyWarning(alarmId);
                foreach (var alarmMethodInfo in alarmMethod)
                {
                    switch (alarmMethodInfo.type)
                    {
                        case "短信":
                            CurrentAlarmDetail.AlarmMethod.Message = alarmMethodInfo.failcount +
                                                                     alarmMethodInfo.succcount;
                            break;
                        case "网站":
                            CurrentAlarmDetail.AlarmMethod.Web = alarmMethodInfo.failcount + alarmMethodInfo.succcount;
                            break;
                        case "微博":
                            CurrentAlarmDetail.AlarmMethod.WeiBo = alarmMethodInfo.failcount + alarmMethodInfo.succcount;
                            break;
                        case "微信":
                            CurrentAlarmDetail.AlarmMethod.WeChat = alarmMethodInfo.failcount +
                                                                    alarmMethodInfo.succcount;
                            break;
                        case "邮件":
                            CurrentAlarmDetail.AlarmMethod.Mail = alarmMethodInfo.failcount + alarmMethodInfo.succcount;
                            break;
                        case "传真":
                            CurrentAlarmDetail.AlarmMethod.Fax = alarmMethodInfo.failcount + alarmMethodInfo.succcount;
                            break;
                        case "大喇叭":
                            CurrentAlarmDetail.AlarmMethod.Horn = alarmMethodInfo.failcount + alarmMethodInfo.succcount;
                            break;
                        case "显示屏":
                            CurrentAlarmDetail.AlarmMethod.Screen = alarmMethodInfo.failcount +
                                                                    alarmMethodInfo.succcount;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 发送绘制台风风圈消息
        /// </summary>
        /// <param name="arriveTime">到达时间</param>
        /// <param name="typhoonData">台风数据</param>
        /// <param name="isShort">台风数据</param>
        private void LoadTyphoonCircle(string arriveTime, List<TyphoonDataInfo> typhoonData, bool isShort)
        {
            var selectTyphoon = typhoonData.Where(e => e.bj_datetime == arriveTime).First();
            var region = new List<TyphoonRegionModel>();
            if (isShort)
            {
                region = DataManager.GetTyphoonWarningsByTime(typhoonData.Min(t => t.bj_datetime), arriveTime);
            }
            var typhoon = new Typhoon
            {
                CenterLon = double.Parse(selectTyphoon.lon),
                CenterLat = double.Parse(selectTyphoon.lat),
                YellowRadius1 = double.Parse(selectTyphoon.en7radii.Equals("null") ? "1" : selectTyphoon.en7radii),
                YellowRadius2 = double.Parse(selectTyphoon.es7radii.Equals("null") ? "1" : selectTyphoon.es7radii),
                YellowRadius3 = double.Parse(selectTyphoon.ws7radii.Equals("null") ? "1" : selectTyphoon.ws7radii),
                YellowRadius4 = double.Parse(selectTyphoon.wn7radii.Equals("null") ? "1" : selectTyphoon.wn7radii),
                RedRadius1 = double.Parse(selectTyphoon.en10radii.Equals("null") ? "1" : selectTyphoon.en10radii),
                RedRadius2 = double.Parse(selectTyphoon.es10radii.Equals("null") ? "1" : selectTyphoon.es10radii),
                RedRadius3 = double.Parse(selectTyphoon.ws10radii.Equals("null") ? "1" : selectTyphoon.ws10radii),
                RedRadius4 = double.Parse(selectTyphoon.wn10radii.Equals("null") ? "1" : selectTyphoon.wn10radii),
                TyphoonIntensity = selectTyphoon.trank,
                TyphoonRegionModels = region,
                IsShort = isShort
            };
            MessageAggregator.GetMessage<CMATyphoonMessage>().Publish(typhoon);
        }

        /// <summary>
        ///     取消订阅
        /// </summary>
        public void UnsubscribeMessage()
        {
            MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>()
                .Unsubscribe(ReceiveCMAScutcheonSelectGroupMessage);
            MessageAggregator.GetMessage<CMAEventPageIntoMessage>().Unsubscribe(ReceiveCMAEventPageIntoMessage);
        }

        #endregion
    }
}