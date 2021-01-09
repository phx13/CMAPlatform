using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Digihail.AVE.Controls.Utils;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Data;

namespace CMAPlatform.TestWindow
{
    public class TimeLineItem : INotifyPropertyChanged
    {
        private TimeSpan m_Duration;

        private string m_GroupName;


        private ObservableCollection<TimeLineItem> m_InculdeItems = new ObservableCollection<TimeLineItem>();

        private string m_Name;
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

        /// <summary>
        ///     开始时间
        /// </summary>
        public TimeSpan Duration
        {
            get { return m_Duration; }
            set
            {
                m_Duration = value;
                OnPropertyChanged("Duration");
            }
        }

        /// <summary>
        ///     名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        ///     组名
        /// </summary>
        public string GroupName
        {
            get { return m_GroupName; }
            set
            {
                m_GroupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        /// <summary>
        ///     聚组
        /// </summary>
        public ObservableCollection<TimeLineItem> InculdeItems
        {
            get { return m_InculdeItems; }
            set
            {
                m_InculdeItems = value;
                OnPropertyChanged("InculdeItems");
            }
        }

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


    /// <summary>
    ///     按类型统计信息
    /// </summary>
    public class CountTypeInfo
    {
        /// <summary>
        ///     统计类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     统计类型下的统计信息
        /// </summary>
        public ObservableCollection<CountInfo> CountInfos { get; set; }
    }

    /// <summary>
    ///     统计类
    /// </summary>
    public class CountInfo
    {
        /// <summary>
        ///     统计时间
        /// </summary>
        public DateTime CountDateTime { get; set; }

        /// <summary>
        ///     统计值
        /// </summary>
        public int CountValue { get; set; }
    }

    /// <summary>
    ///     最大降水量类
    /// </summary>
    public class MaxCountInfo
    {
        /// <summary>
        ///     统计时间
        /// </summary>
        public DateTime CountDateTime { get; set; }

        /// <summary>
        ///     单位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     降水值
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        ///     降水值
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    ///     观测站类
    /// </summary>
    public class StationInfo
    {
        /// <summary>
        ///     观测站名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        ///     观测站降水量
        /// </summary>
        public int RainNumber { get; set; }
    }

    /// <summary>
    ///     单站降水量信息
    /// </summary>
    public class SingleStationInfo
    {
        /// <summary>
        ///     时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     降水量
        /// </summary>
        public double RainNumber { get; set; }

        /// <summary>
        ///     预警等级
        /// </summary>
        public int AlarmLevel { get; set; }
    }

    public class TimeLineChartController : INotifyPropertyChanged
    {
        #region 命令

        public DelegateCommand<StationInfo> TimeBarChangeCommand { get; set; }

        #endregion

        public void InitTestData()
        {
            var mRandom = new Random();

            StartTime = new DateTime(2019, 7, 1);
            EndTime = new DateTime(2019, 7, 3);
            VisibleStartTime = new DateTime(2019, 7, 1, 11, 50, 0);
            VisibleEndTime = new DateTime(2019, 7, 1, 15, 0, 0);

            TimeLineItems = new ObservableItemCollection<TimeLineItem>();
            OnViewTimeLineItems = new ObservableItemCollection<TimeLineItem>();
            MaxCountInfos = new ObservableCollection<MaxCountInfo>();
            StationInfos = new ObservableCollection<StationInfo>();
            SingleStationInfos = new ObservableCollection<SingleStationInfo>();
            WindInfos = new ObservableCollection<WindInfo>();

            #region 灾情舆情指数

            var zqyq1 = new TimeLineItem();
            zqyq1.StartTime = new DateTime(2019, 7, 1, 12, 0, 0);
            zqyq1.Name = "台风在山东南部沿海登陆";
            zqyq1.GroupName = "灾情舆情指数";
            TimeLineItems.Add(zqyq1);

            var zqyq = new TimeLineItem();
            zqyq.StartTime = new DateTime(2019, 7, 1, 12, 30, 0);
            zqyq.Name = "在山东境内减弱为热带低压，20日清晨在山东北部变性为热带低压，在山东境内减弱为热带低压，20日清晨在山东北部变性为热带低压，";
            zqyq.GroupName = "灾情舆情指数";
            TimeLineItems.Add(zqyq);

            zqyq = new TimeLineItem();
            zqyq.StartTime = new DateTime(2019, 7, 1, 12, 40, 0);
            zqyq.Name = "在山东境内减弱为热带低压，20日清晨在山东北部变性为热带低压，在山东境内减弱为热带低压，20日清晨在山东北部变性为热带低压，";
            zqyq.GroupName = "灾情舆情指数";
            TimeLineItems.Add(zqyq);

            zqyq = new TimeLineItem();
            zqyq.StartTime = new DateTime(2019, 7, 1, 12, 50, 0);
            zqyq.Name = "在山东境内减弱为热带低压，20日清晨在山东北部变性为热带低压，在山东境内减弱为热带低压，20日清晨在山东北部变性为热带低压，";
            zqyq.GroupName = "灾情舆情指数";
            TimeLineItems.Add(zqyq);

            zqyq = new TimeLineItem();
            zqyq.StartTime = new DateTime(2019, 7, 1, 13, 50, 0);
            zqyq.Name = "台风在山东南部沿海登陆,台风在山东南部沿海登陆,台风在山东南部沿海登陆";
            zqyq.GroupName = "灾情舆情指数";
            TimeLineItems.Add(zqyq);

            zqyq = new TimeLineItem();
            zqyq.StartTime = new DateTime(2019, 7, 1, 14, 10, 0);
            zqyq.Name = "台风在山东南部沿海登陆,台风在山东南部沿海登陆,台风在山东南部沿海登陆";
            zqyq.GroupName = "灾情舆情指数";
            TimeLineItems.Add(zqyq);

            #endregion

            #region 服务

            var service = new TimeLineItem();

            for (var i = 0; i < 10; i++)
            {
                service = new TimeLineItem();
                service.StartTime = new DateTime(2019, 7, 1, 11, 0, 0);
                service.StartTime = service.StartTime.AddMinutes(20 * i);
                service.Name = string.Format("《两办刊物信息》第{0}期", i + 184);
                service.GroupName = "服务";
                TimeLineItems.Add(service);
            }

            #endregion

            #region 预警

            var yujing = new TimeLineItem();
            for (var i = 0; i < 5; i++)
            {
                yujing = new TimeLineItem();
                yujing.StartTime = new DateTime(2019, 7, 1, 10, 20, 0);
                yujing.StartTime = yujing.StartTime.AddMinutes(100 * i);
                yujing.Name = "中央气象台发布暴雨橙色预警,中央气象台发布暴雨橙色预警";
                yujing.GroupName = "预警-国家级";
                TimeLineItems.Add(yujing);
            }


            var yujingsheng = new TimeLineItem();
            for (var i = 0; i < 10; i++)
            {
                yujingsheng = new TimeLineItem();
                yujingsheng.StartTime = new DateTime(2019, 7, 1, 10, 30, 0);
                yujingsheng.StartTime = yujingsheng.StartTime.AddMinutes(40 * i);
                yujingsheng.Name = "陕西省发布暴雨橙色预警,陕西省发布暴雨橙色预警,陕西省发布暴雨橙色预警,陕西省发布暴雨橙色预警";
                yujingsheng.GroupName = "预警-省级";
                TimeLineItems.Add(yujingsheng);
            }

            var yujingshi = new TimeLineItem();
            for (var i = 0; i < 10; i++)
            {
                yujingshi = new TimeLineItem();
                yujingshi.StartTime = new DateTime(2019, 7, 1, 9, 50, 0);
                yujingshi.StartTime = yujingshi.StartTime.AddMinutes(20 * i);
                yujingshi.Name = "安康市发布暴雨蓝色预警,安康市发布暴雨蓝色预警";
                yujingshi.GroupName = "预警-市级";
                TimeLineItems.Add(yujingshi);
            }

            var yujingxian = new TimeLineItem();
            for (var i = 0; i < 25; i++)
            {
                yujingxian = new TimeLineItem();
                yujingxian.StartTime = new DateTime(2019, 7, 1, 9, 20, 0);
                yujingxian.StartTime = yujingxian.StartTime.AddMinutes(10 * i);
                yujingxian.Name = "甸阳县发布暴雨蓝色预警,甸阳县发布暴雨蓝色预警";
                yujingxian.GroupName = "预警-县级";
                TimeLineItems.Add(yujingxian);
            }

            #endregion

            #region 预警分组

            ConcludeGroupData(TimeLineItems, OnViewTimeLineItems, VisibleStartTime, VisibleEndTime);

            #endregion

            #region 降雨量 - 堆积柱

            OnViewCountInfos = new ObservableCollection<CountTypeInfo>();

            for (var i = 0; i < 4; i++)
            {
                var countTypeInfo = new CountTypeInfo();
                switch (i)
                {
                    case 0:
                        countTypeInfo.Type = "红色预警";
                        break;
                    case 1:
                        countTypeInfo.Type = "橙色预警";
                        break;
                    case 2:
                        countTypeInfo.Type = "黄色预警";
                        break;
                    case 3:
                    default:
                        countTypeInfo.Type = "蓝色预警";
                        break;
                }
                countTypeInfo.CountInfos = new ObservableCollection<CountInfo>();

                for (var time = StartTime; time < EndTime; time = time.AddHours(1))
                {
                    var newCountInfo = new CountInfo();
                    newCountInfo.CountDateTime = time;
                    newCountInfo.CountValue = mRandom.Next() % 10;
                    countTypeInfo.CountInfos.Add(newCountInfo);
                }

                OnViewCountInfos.Add(countTypeInfo);
            }


            for (var time = StartTime; time < EndTime; time = time.AddHours(1))
            {
                var maxCountInfo = new MaxCountInfo();
                maxCountInfo.CountDateTime = time;
                maxCountInfo.MaxValue = (mRandom.Next() % 10).ToString();
                maxCountInfo.Value = 1;
                maxCountInfo.Name = "测试";
                MaxCountInfos.Add(maxCountInfo);
            }

            #endregion

            #region 预警详情窗口

            CurrentAlarmDetail = new AlarmDetailModel
            {
                AlarmTitle = "应急管理部、国家林业和草原局、中国气象应急管理部",
                AlarmTime = DateTime.Now.AddDays(1),
                AlarmLevel = 1,
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
                    //new AlarmProcess()
                    //{
                    //    action = "录入",
                    //    @operator = "操作员4",
                    //    time = DateTime.Now
                    //},
                    //new AlarmProcess()
                    //{
                    //    action = "录入",
                    //    @operator = "操作员5",
                    //    time = DateTime.Now
                    //}
                },
                AlarmMethod = new AlarmMethod
                {
                    Message = 3021,
                    Phone = 650,
                    WeiBo = 35918,
                    WeChat = 6752,
                    Television = 8,
                    Fax = 33,
                    Horn = 254,
                    Screen = 85
                }
            };

            #endregion

            #region 观测站弹窗

            StationInfos.Add(new StationInfo
            {
                StationName = "仙河镇",
                RainNumber = 99
            });
            StationInfos.Add(new StationInfo
            {
                StationName = "石门乡",
                RainNumber = 99
            });
            StationInfos.Add(new StationInfo
            {
                StationName = "吕河",
                RainNumber = 45
            });
            StationInfos.Add(new StationInfo
            {
                StationName = "旬阳",
                RainNumber = 66
            });
            StationInfos.Add(new StationInfo
            {
                StationName = "白柳",
                RainNumber = 30
            });
            StationInfos.Add(new StationInfo
            {
                StationName = "神河",
                RainNumber = 56
            });

            #endregion

            #region 单站降雨柱图

            for (var time = StartTime; time < EndTime; time = time.AddHours(1))
            {
                var singleStation = new SingleStationInfo
                {
                    Time = time,
                    RainNumber = mRandom.Next() % 10 * mRandom.Next(8, 12)
                };
                if (singleStation.RainNumber > 75)
                {
                    singleStation.AlarmLevel = 2;
                }
                else
                {
                    singleStation.AlarmLevel = 1;
                }

                SingleStationInfos.Add(singleStation);
            }

            #endregion

            #region 突发事件图表（三合一）

            var data = new List<EmergencyBarInfoJson>();
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "突发事件泳道图右下图表.json");
            var root = JsonHelper.DeserializeFromFile<EmergencyBarInfoRoot>(filePath);
            if (root != null)
            {
                data = root.liveDataList.ToList();
            }
            foreach (var item in data)
            {
                var singleStation = new EmergencyBarInfo
                {
                    Time = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                    BarTime = DateTime.ParseExact(item.nowTime, "yyyyMMddHH0000", CultureInfo.CurrentCulture),
                    RainNumber = double.Parse(item.pRE_1h),
                    Temperature = double.Parse(item.tEM),
                    Wind = double.Parse(item.wIN_S_Avg_10mi),
                    WindDirection = item.wIN_D_Avg_10mi
                };
                EmergencyBarInfos.Add(singleStation);
            }

            var temp = from t in EmergencyBarInfos orderby t.Time ascending select t;
            EmergencyBarInfos = new ObservableCollection<EmergencyBarInfo>(temp.ToList());

            EmergencyBarInfos.LastOrDefault().RainNumber = 0;
            EmergencyBarInfos.LastOrDefault().BarTime = EndTime.AddHours(-1);

            //EmergencyBarInfos.Add(new EmergencyBarInfo()
            //{
            //    Time = this.EndTime.AddHours(-1),
            //    RainNumber = mRandom.Next() % 10 * mRandom.Next(8, 12),
            //    Temperature = mRandom.Next() % 10 * mRandom.Next(8, 12),
            //    Wind = mRandom.Next() % 10 * mRandom.Next(8, 12)
            //});

            #endregion

            #region 大风图表

            int fff = 0;

            for (var time = StartTime; time < EndTime; time = time.AddHours(1))
            {
                var wind = new WindInfo
                {
                    Time = time,
                    Wind = mRandom.Next(0, 13),
                    WindAngle = mRandom.Next(0, 361),
                    AveWind = mRandom.Next(0, 13),
                    AveWindAngle = mRandom.Next(0, 361),
                };

                if (fff % 2 == 0)
                {
                    wind.IsVisible = true;
                }
                else
                {
                    wind.IsVisible = false;
                }

                fff++;
                WindInfos.Add(wind);
            }

            #endregion
        }


        /// <summary>
        ///     根据时间范围聚组
        /// </summary>
        /// <param name="orginCollection"></param>
        /// <param name="showCollection"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void ConcludeGroupData(ObservableCollection<TimeLineItem> orginCollection,
            ObservableCollection<TimeLineItem> showCollection, DateTime startTime, DateTime endTime)
        {
            if (orginCollection == null || showCollection == null)
            {
                return;
            }

            orginCollection.ToList().ForEach(t => t.InculdeItems.Clear());
            showCollection.Clear();


            var visibleDurations = endTime - startTime;

            //超过3天按1天算
            if (visibleDurations.TotalDays >= 3)
            {
                orginCollection.ToList().ForEach(t => t.Duration = TimeSpan.FromDays(1));
            }
            if (visibleDurations.TotalDays >= 1)
            {
                orginCollection.ToList().ForEach(t => t.Duration = TimeSpan.FromHours(8));
            }
            else if (visibleDurations.TotalHours >= 12)
            {
                orginCollection.ToList().ForEach(t => t.Duration = TimeSpan.FromHours(1));
            }
            else if (visibleDurations.TotalHours >= 4)
            {
                orginCollection.ToList().ForEach(t => t.Duration = TimeSpan.FromMinutes(30));
            }
            else
            {
                orginCollection.ToList().ForEach(t => t.Duration = TimeSpan.FromMinutes(10));
            }


            //按组名分组
            var groups = orginCollection.GroupBy(t => t.GroupName);
            foreach (var group in groups)
            {
                //整合组
                var itemsInGroup = new List<TimeLineItem>();
                foreach (var timeLineItem in group.OrderBy(t => t.StartTime))
                {
                    //找到整合组中覆盖包含开始时间的组
                    var existgroup =
                        itemsInGroup.FirstOrDefault(
                            t =>
                                t.StartTime <= timeLineItem.StartTime &&
                                t.StartTime + t.Duration > timeLineItem.StartTime);

                    //存在加入组里
                    if (existgroup != null)
                    {
                        existgroup.InculdeItems.Add(timeLineItem);
                        existgroup.Name = string.Format("{0}-共{1}条", existgroup.GroupName, existgroup.InculdeItems.Count);
                    }
                    else
                    {
                        existgroup = new TimeLineItem();
                        existgroup.StartTime = timeLineItem.StartTime;
                        existgroup.Duration = timeLineItem.Duration;
                        existgroup.Name = timeLineItem.Name;
                        existgroup.GroupName = timeLineItem.GroupName;
                        existgroup.InculdeItems.Add(timeLineItem);
                        itemsInGroup.Add(existgroup);
                    }
                }

                foreach (var lineItem in itemsInGroup)
                {
                    showCollection.Add(lineItem);
                }
            }
        }

        #region 属性

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

                ConcludeGroupData(TimeLineItems, OnViewTimeLineItems, VisibleStartTime, VisibleEndTime);
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

                ConcludeGroupData(TimeLineItems, OnViewTimeLineItems, VisibleStartTime, VisibleEndTime);
            }
        }

        private ObservableCollection<TimeLineItem> m_TimeLineItems;

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

        private ObservableCollection<TimeLineItem> m_OnViewTimeLineItems;

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


        private ObservableCollection<CountTypeInfo> m_OnViewCountInfos;

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

        private ObservableCollection<MaxCountInfo> m_MaxCountInfos;

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

        private ObservableCollection<StationInfo> m_StationInfos;

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

        private ObservableCollection<SingleStationInfo> m_SingleStationInfos;

        /// <summary>
        ///     单站降水量柱图
        /// </summary>
        public ObservableCollection<SingleStationInfo> SingleStationInfos
        {
            get { return m_SingleStationInfos; }
            set
            {
                m_SingleStationInfos = value;
                OnPropertyChanged("SingleStationInfos");
            }
        }

        private string m_TimeBarTitle = "实况（杭州市多站降水）";

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

        /// <summary>
        ///     大风数据集合
        /// </summary>
        public ObservableCollection<WindInfo> WindInfos { get; set; }

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

        #region 预警弹窗

        public class AlarmDetailModel
        {
            /// <summary>
            ///     预警标题
            /// </summary>
            public string AlarmTitle { get; set; }

            /// <summary>
            ///     预警时间
            /// </summary>
            public DateTime AlarmTime { get; set; }

            /// <summary>
            ///     预警时间
            /// </summary>
            public int AlarmLevel { get; set; }

            /// <summary>
            ///     预警描述
            /// </summary>
            public string AlarmDescription { get; set; }

            /// <summary>
            ///     预警责任人数量
            /// </summary>
            public int AlarmPeopleNumber { get; set; }

            /// <summary>
            ///     信息员
            /// </summary>
            public AlarmInformation AlarmInformation { get; set; }

            /// <summary>
            ///     预警流程
            /// </summary>
            public List<AlarmProcess> AlarmProcess { get; set; }

            /// <summary>
            ///     预警手段
            /// </summary>
            public AlarmMethod AlarmMethod { get; set; }
        }

        public class AlarmInformation
        {
            /// <summary>
            ///     发送数量
            /// </summary>
            public int SendNumber { get; set; }

            /// <summary>
            ///     浏览数量
            /// </summary>
            public int BrowseNumber { get; set; }

            /// <summary>
            ///     转发数量
            /// </summary>
            public int RelayNumber { get; set; }
        }


        public class AlarmMethod
        {
            /// <summary>
            ///     短信数量
            /// </summary>
            public int Message { get; set; }

            /// <summary>
            ///     电话数量
            /// </summary>
            public int Phone { get; set; }

            /// <summary>
            ///     微博数量
            /// </summary>
            public int WeiBo { get; set; }

            /// <summary>
            ///     微信数量
            /// </summary>
            public int WeChat { get; set; }

            /// <summary>
            ///     电视数量
            /// </summary>
            public int Television { get; set; }

            /// <summary>
            ///     传真数量
            /// </summary>
            public int Fax { get; set; }

            /// <summary>
            ///     大喇叭数量
            /// </summary>
            public int Horn { get; set; }

            /// <summary>
            ///     显示屏数量
            /// </summary>
            public int Screen { get; set; }
        }

        #endregion
    }


    /// <summary>
    ///     流程单个步骤Model
    /// </summary>
    public class AlarmProcess
    {
        /// <summary>
        ///     行为
        /// </summary>
        public string action { get; set; }

        /// <summary>
        ///     操作者
        /// </summary>
        public string @operator { get; set; }

        /// <summary>
        ///     操作时间
        /// </summary>
        public DateTime time { get; set; }
    }

    /// <summary>
    ///     突发事件Timebar信息
    /// </summary>
    public class EmergencyBarInfo
    {
        /// <summary>
        ///     时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     时间
        /// </summary>
        public DateTime BarTime { get; set; }

        /// <summary>
        ///     降水量
        /// </summary>
        public double RainNumber { get; set; }

        /// <summary>
        ///     预警等级
        /// </summary>
        public int AlarmLevel { get; set; }

        /// <summary>
        ///     阵风
        /// </summary>
        public double Wind { get; set; }

        /// <summary>
        ///     阵风风向
        /// </summary>
        public string WindDirection { get; set; }

        /// <summary>
        ///     温度
        /// </summary>
        public double Temperature { get; set; }
    }

    /// <summary>
    ///     突发事件json
    /// </summary>
    public class EmergencyBarInfoRoot
    {
        public EmergencyBarInfoJson[] liveDataList { get; set; }
    }

    public class EmergencyBarInfoJson
    {
        /// <summary>
        ///     站点名称
        /// </summary>
        public string stationName { get; set; }

        /// <summary>
        ///     温度
        /// </summary>
        public string tEM { get; set; }

        /// <summary>
        ///     降雨
        /// </summary>
        public string pRE_1h { get; set; }

        /// <summary>
        ///     风向
        /// </summary>
        public string wIN_D_Avg_10mi { get; set; }

        /// <summary>
        ///     风速
        /// </summary>
        public string wIN_S_Avg_10mi { get; set; }

        /// <summary>
        ///     时间
        /// </summary>
        public string nowTime { get; set; }
    }

    /// <summary>
    ///     大风Model
    /// </summary>
    public class WindInfo
    {
        /// <summary>
        ///     时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     阵风
        /// </summary>
        public int Wind { get; set; }

        /// <summary>
        ///     阵风风向角
        /// </summary>
        public double WindAngle { get; set; }

        /// <summary>
        ///     平均风力
        /// </summary>
        public int AveWind { get; set; }

        /// <summary>
        ///     平均风力风向角
        /// </summary>
        public double AveWindAngle { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }
    }
}