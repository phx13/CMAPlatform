using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Launcher.Controls;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCP.Helper;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.CCP.UserControls;

namespace CMAPlatform.Chart.Window
{
    /// <summary>
    ///     ComprehenSiveWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ComprehenSiveWindow : PopWindow, INotifyPropertyChanged
    {
        private readonly MessageAggregator m_MessageAggregator = new MessageAggregator();

        public ComprehenSiveWindow()
        {
            InitializeComponent();
            SelectData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SelectData()
        {
            DataMessage();

            EventDictionary = DataManager.Instance.EventDictiinaryType();
            EventComboBox.ItemsSource = EventDictionary;
            var nationwideAreas = DataManager.Instance.NationwideArea();
            ProvinceAreaEvent = nationwideAreas;
            ProvinceAreaRisk = nationwideAreas;
            ProvinceComboBox.ItemsSource = ProvinceAreaEvent.Where(p => p.region_type == "province");
            ShengValue.ItemsSource = ProvinceAreaRisk.Where(p => p.region_type == "province");
            //CreateData createData = new CreateData();
            //EventListEnter eventList =null;
            //EventItemsControl.ItemsSource = createData.findEmergencise(eventList).Events;
            //RiskEventsControl.ItemsSource = createData.getWarningByEventType("", "", "", "", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            //TyphoonEvents = createData.searchTyphoon(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            //TyphoonEventsControl.ItemsSource = TyphoonEvents;
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var tabItem = e.AddedItems[0] as TabItem;
                if (tabItem != null && tabItem.Header.ToString() == "突发事件")
                {
                    ProvinceComboBox.SelectedIndex = 32;
                    CityComboBox.SelectedIndex = 1;
                    CountyComboBox.SelectedIndex = 2;
                    PickerStaTime.SelectedValue = new DateTime(2019, 7, 1, 0, 0, 0);
                    PickerEndTime.SelectedValue = new DateTime(2019, 11, 1, 0, 0, 0);
                }
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <summary>
        ///     预警类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogBtn_Categor3_OnClick(object sender, RoutedEventArgs e)
        {
            var thickness = new Thickness(220, 0, 0, 0);
            YuJingType.Margin = thickness;
            ShengName.Visibility = Visibility.Collapsed;
            ShengValue.Visibility = Visibility.Collapsed;
            ShiName.Visibility = Visibility.Collapsed;
            ShiValue.Visibility = Visibility.Collapsed;
            XianName.Visibility = Visibility.Collapsed;
            XianValue.Visibility = Visibility.Collapsed;
            m_EventNameType = "预警类型";
        }

        /// <summary>
        ///     地域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogBtn_Categor4_OnClick(object sender, RoutedEventArgs e)
        {
            var thickness = new Thickness(40, 0, 0, 0);
            YuJingType.Margin = thickness;
            ShengName.Visibility = Visibility.Visible;
            ShengValue.Visibility = Visibility.Visible;
            ShiName.Visibility = Visibility.Visible;
            ShiValue.Visibility = Visibility.Visible;
            XianName.Visibility = Visibility.Visible;
            XianValue.Visibility = Visibility.Visible;
            m_EventNameType = "地域";

            ShengValue.SelectedIndex = 31;
            ShiValue.SelectedIndex = 6;
            XianValue.SelectedIndex = 8;
            EventComboBox3.SelectedIndex = 1;
            PickerStaYJTime.SelectedValue = new DateTime(2018, 8, 19, 0, 0, 0);
            PickerEndYJTime.SelectedValue = new DateTime(2018, 8, 20, 0, 0, 0);
        }

        /// <summary>
        ///     综合查询_突发事件_搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EventBtn_OnClick(object sender, RoutedEventArgs e)
        {
            EventSc.Visibility = Visibility.Visible;
            EventEmptyGrid.Visibility = Visibility.Collapsed;


            var eventList = new EventListEnter();
            eventList.province = Transition(ProvinceComboBox.SelectedValue + "");
            eventList.city = Transition(CityComboBox.SelectedValue + "");
            eventList.country = Transition(CountyComboBox.SelectedValue + "");
            if (!string.IsNullOrEmpty(PickerStaTime.SelectedValue.ToString()))
            {
                eventList.Begintime = Convert.ToDateTime(PickerStaTime.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                MessageBoxWindow.Show("提示", "请选择开始时间");
                return;
                //eventList.Begintime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            }
            if (!string.IsNullOrEmpty(PickerEndTime.SelectedValue.ToString()))
            {
                eventList.endtime = Convert.ToDateTime(PickerEndTime.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                MessageBoxWindow.Show("提示", "请选择结束时间");
                return;
                //eventList.endtime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            }
            if (Convert.ToDateTime(PickerStaTime.SelectedValue) > Convert.ToDateTime(PickerEndTime.SelectedValue))
            {
                MessageBoxWindow.Show("提示", "开始时间不能大于结束时间");
                return;
            }

            eventList.eventType = EventComboBox.SelectedValue + ""; // "12A00";
            if (EventComboBox.SelectedValue == null)
            {
                eventList.eventType = "null";
            }

            var typhoonWorker = new BackgroundWorker();

            typhoonWorker.DoWork += (o, args) =>
            {
                var createData = new CreateData();
                args.Result = createData.findEmergencise(eventList).Events;
            };
            typhoonWorker.RunWorkerCompleted += (o, args) =>
            {
                if (args.Result is List<Events>)
                {
                    var list = args.Result as List<Events>;
                    EventItemsControl.ItemsSource = list;

                    if (list == null || list.Count <= 0)
                    {
                        EventSc.Visibility = Visibility.Collapsed;
                        EventEmptyGrid.Visibility = Visibility.Visible;
                    }
                }
                IsBusy = false;
            };
            typhoonWorker.RunWorkerAsync();
            IsBusy = true;
        }


        public string Transition(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "-1")
            {
                return "null";
            }
            return value;
        }

        /// <summary>
        ///     综合查询_风险态势_台风事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TyphoonSearch_OnClick(object sender, RoutedEventArgs e)
        {
            typhoonSc.Visibility = Visibility.Visible;
            TyphoonEmptyGrid.Visibility = Visibility.Collapsed;

            var createData = new CreateData();
            string beginTime;
            string endTime;
            //if (!string.IsNullOrEmpty(PickerStaTFTime.SelectedValue.ToString()))
            //{
            //    beginTime = Convert.ToDateTime(PickerStaTFTime.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss");
            //}
            //else
            //{
            //    MessageBoxWindow.Show("提示", "请选择开始时间");
            //    return;
            //}
            //if (!string.IsNullOrEmpty(PickerEndTFTime.SelectedValue.ToString()))
            //{
            //    endTime = Convert.ToDateTime(PickerEndTFTime.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss");
            //}
            //else
            //{
            //    MessageBoxWindow.Show("提示", "请选择结束时间");
            //    return;
            //}
            //if (Convert.ToDateTime(PickerStaTFTime.SelectedValue) > Convert.ToDateTime(PickerEndTFTime.SelectedValue))
            //{
            //    MessageBoxWindow.Show("提示", "开始时间不能大于结束时间");
            //    return;
            //}

            var typhoonWorker = new BackgroundWorker();

            typhoonWorker.DoWork += (o, args) =>
            {
                //TyphoonEvents = createData.searchTyphoon(beginTime, endTime);
                TyphoonEvents = createData.searchTyphoon("", "");
            };
            typhoonWorker.RunWorkerCompleted += (o, args) =>
            {
                TyphoonEventsControl.ItemsSource = TyphoonEvents;
                if (TyphoonEvents == null || TyphoonEvents.Count <= 0)
                {
                    typhoonSc.Visibility = Visibility.Collapsed;
                    TyphoonEmptyGrid.Visibility = Visibility.Visible;
                }

                IsBusy = false;
            };
            typhoonWorker.RunWorkerAsync();
            IsBusy = true;
        }


        /// <summary>
        ///     综合查询_风险态势_预警信息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RiskBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string senderCode;
            string beginTime;
            string endTime;
            var msgType = Transition(EventComboBox2.SelectedValue + "");
            var eventType = Transition(EventComboBox3.SelectedValue + "");
            var severity = Transition(EventComboBox1.SelectedValue + "");

            warningSc.Visibility = Visibility.Visible;
            WarningEmptyGrid.Visibility = Visibility.Collapsed;


            if (!string.IsNullOrEmpty(PickerStaYJTime.SelectedValue.ToString()))
            {
                beginTime = Convert.ToDateTime(PickerStaYJTime.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                MessageBoxWindow.Show("提示", "请选择开始时间");
                return;
                // beginTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            }
            if (!string.IsNullOrEmpty(PickerEndYJTime.SelectedValue.ToString()))
            {
                endTime = Convert.ToDateTime(PickerEndYJTime.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                MessageBoxWindow.Show("提示", "请选择结束时间");
                return;
                //endTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            }
            if (Convert.ToDateTime(PickerStaYJTime.SelectedValue) > Convert.ToDateTime(PickerEndYJTime.SelectedValue))
            {
                MessageBoxWindow.Show("提示", "开始时间不能大于结束时间");
                return;
            }

            var typhoonWorker = new BackgroundWorker();

            senderCode = AreaCode();

            typhoonWorker.DoWork += (o, args) =>
            {
                var createData = new CreateData();
                if (m_EventNameType == "地域")
                {
                    args.Result = createData.getWarningByEventType(senderCode, msgType, eventType, severity, beginTime,
                        endTime);
                }
                else
                {
                    args.Result = createData.getWarningByEventType("", msgType, eventType, severity, beginTime, endTime);
                }
            };
            typhoonWorker.RunWorkerCompleted += (o, args) =>
            {
                if (args.Result is List<RiskSituationModel>)
                {
                    var list = args.Result as List<RiskSituationModel>;
                    RiskEventsControl.ItemsSource = list;

                    if (list == null || list.Count <= 0)
                    {
                        warningSc.Visibility = Visibility.Collapsed;
                        WarningEmptyGrid.Visibility = Visibility.Visible;
                    }
                }
                IsBusy = false;
            };
            typhoonWorker.RunWorkerAsync();
            IsBusy = true;
        }

        /// <summary>
        ///     区域编码
        /// </summary>
        /// <returns></returns>
        public string AreaCode()
        {
            var value = "null";
            if (ShengValue.SelectedValue + "" != "" && ShengValue.SelectedValue + "" != "-1" &&
                ShengValue.SelectedValue != null)
            {
                value = ShengValue.SelectedValue + "";
                if (ShiValue.SelectedValue + "" != "" && ShiValue.SelectedValue + "" != "-1" &&
                    ShiValue.SelectedValue != null)
                {
                    value = ShiValue.SelectedValue + "";
                    if (XianValue.SelectedValue + "" != "" && XianValue.SelectedValue + "" != "-1" &&
                        XianValue.SelectedValue != null)
                    {
                        value = XianValue.SelectedValue + "";
                    }
                }
            }
            return value;
        }

        private void CityComboBoxEvent_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CityComboBox.SelectedIndex != -1)
            {
                CountyComboBox.ItemsSource =
                    ProvinceAreaEvent.Where(p => p.parent_code == CityComboBox.SelectedValue.ToString()).ToList();
                CountyComboBox.SelectedIndex = -1;
            }
        }

        private void ProvinceComboBoxEvent_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityComboBox.ItemsSource =
                ProvinceAreaEvent.Where(p => p.parent_code == ProvinceComboBox.SelectedValue.ToString()).ToList();
            CountyComboBox.SelectedIndex = -1;
            CityComboBox.SelectedIndex = -1;
        }

        private void ShengValueRisk_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShiValue.ItemsSource =
                ProvinceAreaEvent.Where(p => p.parent_code == ShengValue.SelectedValue.ToString()).ToList();
            XianValue.SelectedIndex = -1;
            ShiValue.SelectedIndex = -1;
        }

        private void ShiValueRisk_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShiValue.SelectedIndex != -1)
            {
                XianValue.SelectedIndex = -1;
                XianValue.ItemsSource =
                    ProvinceAreaEvent.Where(p => p.parent_code == ShiValue.SelectedValue.ToString()).ToList();
            }
        }

        /// <summary>
        ///     综合查询_下拉值绑定
        /// </summary>
        public void DataMessage()
        {
            //综合查询_风险态势_预警信息_预警状态
            var evenTypes = new List<Nationwide_Area>();
            var evenType1 = new Nationwide_Area();
            evenType1.code = "Alert";
            evenType1.region_name = "首次";
            var evenType2 = new Nationwide_Area();
            evenType2.code = "Update";
            evenType2.region_name = "更新";
            var evenType3 = new Nationwide_Area();
            evenType3.code = "Cancle";
            evenType3.region_name = "解除";
            evenTypes.Add(evenType1);
            evenTypes.Add(evenType2);
            evenTypes.Add(evenType3);
            EventComboBox2.ItemsSource = evenTypes;
            //综合查询_风险态势_预警信息_预警类型
            var evenTypes1 = new List<Nationwide_Area>();
            var eventOne = new Nationwide_Area();
            eventOne.code = "11B01";
            eventOne.region_name = "台风事件";
            evenTypes1.Add(eventOne);
            var eventTwo = new Nationwide_Area();
            eventTwo.code = "11B03";
            eventTwo.region_name = "暴雨事件";
            evenTypes1.Add(eventTwo);
            var eventThree = new Nationwide_Area();
            eventThree.code = "11B04";
            eventThree.region_name = "暴雪事件";
            evenTypes1.Add(eventThree);
            var eventFour = new Nationwide_Area();
            eventFour.code = "11B05";
            eventFour.region_name = "寒潮事件";
            evenTypes1.Add(eventFour);
            var eventFive = new Nationwide_Area();
            eventFive.code = "11B06";
            eventFive.region_name = "大风事件";
            evenTypes1.Add(eventFive);
            var eventSix = new Nationwide_Area();
            eventSix.code = "11B07";
            eventSix.region_name = "沙尘暴事件";
            evenTypes1.Add(eventSix);
            var eventSeven = new Nationwide_Area();
            eventSeven.code = "11B09";
            eventSeven.region_name = "高温事件";
            evenTypes1.Add(eventSeven);
            var eventEight = new Nationwide_Area();
            eventEight.code = "11B14";
            eventEight.region_name = "雷电事件";
            evenTypes1.Add(eventEight);
            var eventNine = new Nationwide_Area();
            eventNine.code = "11B15";
            eventNine.region_name = "冰雹事件";
            evenTypes1.Add(eventNine);
            var eventTen = new Nationwide_Area();
            eventTen.code = "11B16";
            eventTen.region_name = "霜冻事件";
            evenTypes1.Add(eventTen);
            var eventEleven = new Nationwide_Area();
            eventEleven.code = "11B17";
            eventEleven.region_name = "大雾事件";
            evenTypes1.Add(eventEleven);
            var eventTwelve = new Nationwide_Area();
            eventTwelve.code = "11B19";
            eventTwelve.region_name = "霾事件";
            evenTypes1.Add(eventTwelve);
            var eventThirteen = new Nationwide_Area();
            eventThirteen.code = "11B21";
            eventThirteen.region_name = "道路结冰事件";
            evenTypes1.Add(eventThirteen);
            var eventFourteen = new Nationwide_Area();
            eventFourteen.code = "11B22";
            eventFourteen.region_name = "干旱事件";
            evenTypes1.Add(eventFourteen);
            EventComboBox3.ItemsSource = evenTypes1;
            //综合查询_风险态势_预警信息_预警状态
            var evenLevels = new List<Nationwide_Area>();
            var evenLevel1 = new Nationwide_Area();
            evenLevel1.code = "uknown";
            evenLevel1.region_name = "全部";
            var evenLevel2 = new Nationwide_Area();
            evenLevel2.code = "red";
            evenLevel2.region_name = "红";
            var evenLevel3 = new Nationwide_Area();
            evenLevel3.code = "orange";
            evenLevel3.region_name = "橙";
            var evenLevel4 = new Nationwide_Area();
            evenLevel4.code = "yellow";
            evenLevel4.region_name = "黄";
            var evenLevel5 = new Nationwide_Area();
            evenLevel5.code = "blue";
            evenLevel5.region_name = "蓝";
            evenLevels.Add(evenLevel1);
            evenLevels.Add(evenLevel2);
            evenLevels.Add(evenLevel3);
            evenLevels.Add(evenLevel4);
            evenLevels.Add(evenLevel5);
            EventComboBox1.ItemsSource = evenLevels;
        }


        /// <summary>
        ///     突发事件点击跳转事件页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventItemsControlBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn.Content == null) return;

            var events = btn.Content as Events;

            Close();

            var stateChangedModel = new StateChangedModel
            {
                PageNameList = new List<string>(),
                StateName = "突发事件页"
            };
            //切换状态
            m_MessageAggregator.GetMessage<StateChangedMessage>().Publish(stateChangedModel);

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

            var view = CommonHelper.Instance.FindEventPageViewModel("XXXX00");

            if (view != null)
            {
                //发送三维位置消息
                var globalData = new GlobeTranslocationData
                {
                    PageId = CCPHelper.Instance.GetCurrentCCPPageModel().ID,
                    PageInstanceId = Guid.NewGuid(),
                    IsFree = true,
                    CurLongitude = events.Longitude,
                    CurLatitude = events.Latitude,
                    CurHeight = 30000,
                    CurSurroundAngle = view.Yaw,
                    CurPitch = view.Pitch
                };
                //globalData.CurHeight = view.Height;

                m_MessageAggregator.GetMessage<GlobeTranslocationMessage>().Publish(globalData);
            }
        }

        /// <summary>
        ///     预警态势点击预警跳转事件页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RiskItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn.Content != null)
            {
                var warnings = btn.Content as Warnings;

                Close();

                var stateChangedModel = new StateChangedModel
                {
                    PageNameList = new List<string>(),
                    StateName = "事件页"
                };

                //if (warnings != null && warnings.WarningType == "11B01")
                //{
                //    stateChangedModel = new StateChangedModel
                //    {
                //        PageNameList = new List<string>(),
                //        StateName = "台风事件页"
                //    };
                //}

                //切换状态
                m_MessageAggregator.GetMessage<StateChangedMessage>().Publish(stateChangedModel);

                //发送进入事件页
                var data = new ScutcheonSelectGroupMessage();
                data.areaCode = warnings.AreaCode;
                data.id = warnings.ID;

                var info = DataManager.GetWarningByIdentifier(warnings.ID);

                data.lon = warnings.Lon = double.Parse(info.lon);
                data.lat = warnings.Lat = double.Parse(info.lat);
                data.colour = warnings.WarningLevel;
                data.typeName = warnings.WarningType;
                data.DateTime = warnings.Time;

                var i = 0;
                while (i < 2)
                {
                    m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>().Publish(data);
                    Thread.Sleep(20);
                    i++;
                }

                //if (warnings != null && warnings.WarningType == "11B01") return;

                var view = CommonHelper.Instance.FindEventPageViewModel(data.areaCode);

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
            }
        }

        // 台风跳转
        private void Typhoon_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();

            var typhoonEventsModel = (sender as Grid).DataContext as TyphoonEventsModel;

            var stateChangedModel = new StateChangedModel
            {
                PageNameList = new List<string>(),
                StateName = "台风事件页"
            };

            //切换状态
            m_MessageAggregator.GetMessage<StateChangedMessage>().Publish(stateChangedModel);

            //发送进入事件页
            var data = new ScutcheonSelectGroupMessage
            {
                areaCode = "",
                id = typhoonEventsModel.ID,
                typeName = "11B01",
                DateTime = typhoonEventsModel.StartTime,
                TyphoonName = typhoonEventsModel.Name
            };

            m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>().Publish(data);

            //var i = 0;
            //while (i < 2)
            //{
            //    m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>().Publish(data);
            //    Thread.Sleep(20);
            //    i++;
            //}
        }

        #region

        /// <summary>
        ///     预警类型
        /// </summary>
        private string m_EventNameType = "";

        private List<EventListModel> m_EventListModels;

        /// <summary>
        ///     突发事件列表Model
        /// </summary>
        public List<EventListModel> EventListModels
        {
            get { return m_EventListModels; }
            set
            {
                m_EventListModels = value;
                OnPropertyChanged("EventListModels");
            }
        }

        private List<RiskSituationModel> m_RiskSituationModels = new List<RiskSituationModel>();

        /// <summary>
        ///     风险态势
        /// </summary>
        public List<RiskSituationModel> RiskSituationModels
        {
            get { return m_RiskSituationModels; }
            set
            {
                m_RiskSituationModels = value;
                OnPropertyChanged("RiskSituationModels");
            }
        }

        private List<ExemtremeWeatherModel> m_ExemtremeWeatherModel;

        /// <summary>
        ///     极端天气
        /// </summary>
        public List<ExemtremeWeatherModel> ExemtremeWeatherModel
        {
            get { return m_ExemtremeWeatherModel; }
            set
            {
                m_ExemtremeWeatherModel = value;
                OnPropertyChanged("ExemtremeWeatherModel");
            }
        }

        private List<TyphoonEventsModel> m_TyphoonEvents;

        /// <summary>
        ///     台风事件
        /// </summary>
        public List<TyphoonEventsModel> TyphoonEvents
        {
            get { return m_TyphoonEvents; }
            set
            {
                m_TyphoonEvents = value;
                OnPropertyChanged("TyphoonEvents");
            }
        }

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

        private List<Nationwide_Area> m_ProvinceAreaEvent = new List<Nationwide_Area>();

        /// <summary>
        ///     全国地区数据
        /// </summary>
        public List<Nationwide_Area> ProvinceAreaEvent
        {
            get { return m_ProvinceAreaEvent; }
            set
            {
                m_ProvinceAreaEvent = value;
                OnPropertyChanged("ProvinceAreaEvent");
            }
        }

        private List<Nationwide_Area> m_ProvinceAreaRisk = new List<Nationwide_Area>();

        /// <summary>
        ///     全国地区数据
        /// </summary>
        public List<Nationwide_Area> ProvinceAreaRisk
        {
            get { return m_ProvinceAreaRisk; }
            set
            {
                m_ProvinceAreaRisk = value;
                OnPropertyChanged("ProvinceAreaRisk");
            }
        }


        private bool m_IsBusy;

        /// <summary>
        ///     忙碌状态
        /// </summary>
        public bool IsBusy
        {
            get { return m_IsBusy; }
            set
            {
                m_IsBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        #endregion
    }
}