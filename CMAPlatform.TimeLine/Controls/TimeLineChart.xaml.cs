using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CMAPlatform.Models.TimeBarModel;
using CMAPlatform.Models.TimeLineModel;
using CMAPlatform.TimeLine.Window;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Controls.TimeBar;
using Telerik.Windows.Controls.Timeline;

namespace CMAPlatform.TimeLine.Controls
{
    /// <summary>
    ///     TimeLineChart.xaml 的交互逻辑
    /// </summary>
    public partial class TimeLineChart : UserControl, INotifyPropertyChanged, IDisposable
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        public TimeLineChart()
        {
            InitializeComponent();
            DataContextChanged += TimeLineChart_DataContextChanged;
            Loaded += TimeLineChart_Loaded;
        }

        #endregion

        #region 公共属性

        /// <summary>
        ///     DataContext
        /// </summary>
        public TimeLineChartController Controller
        {
            get { return m_Controller; }
            set
            {
                m_Controller = value;
                OnPropertyChanged("Controller");
            }
        }

        #endregion

        #region 释放

        /// <summary>
        ///     释放资源
        /// </summary>
        public void Dispose()
        {
            if (m_Controller != null)
            {
                m_Controller.UnsubscribeMessage();
            }
        }

        #endregion

        #region 私有函数

        // 下方柱图切换方法
        private void ChangeToSingleBar(StationInfo station)
        {
            //先全部隐藏
            realtimeRainControl.Visibility = Visibility.Collapsed;
            emergencyMultiControl.Visibility = Visibility.Collapsed;
            singleRealtimeRainControl.Visibility = Visibility.Collapsed;
            windLine.Visibility = Visibility.Collapsed;
            coldWaveLine.Visibility = Visibility.Collapsed;

            if (station.EventType == "11B03")
            {
                singleRealtimeRainControl.Visibility = Visibility.Visible;
                m_Controller.TimeBarTitle = string.Format("实况（{0}站降水）", station.StationName);
            }
            else if (station.EventType == "11B06")
            {
                windLine.Visibility = Visibility.Visible;
                m_Controller.TimeBarTitle = string.Format("实况（{0}站大风）", station.StationName);
            }
            else if (station.EventType == "11B05")
            {
                coldWaveLine.Visibility = Visibility.Visible;
                m_Controller.TimeBarTitle = string.Format("实况（{0}站寒潮）", station.StationName);
            }
            returnButton.Visibility = Visibility.Visible;
        }

        #endregion

        #region public 方法

        /// <summary>
        ///     修改全屏/半屏背景
        /// </summary>
        /// <param name="isFullscreem"></param>
        public void SetBackground(bool isFullscreem)
        {
            if (isFullscreem)
            {
                bg_halfScreen.Visibility = Visibility.Collapsed;
                bg_fullScreen.Visibility = Visibility.Visible;
            }
            else
            {
                bg_halfScreen.Visibility = Visibility.Visible;
                bg_fullScreen.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region 成员变量

        private bool m_Init;

        private TimeLineChartController m_Controller;

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

        #region 控件事件

        // 点击泳道图某一块预警/灾情舆情/服务
        private void TimeLineItem_Click(object sender, MouseButtonEventArgs e)
        {
            var itemControl = sender as TimelineItemControl;
            if (itemControl != null && itemControl.DataContext is TimelineDataItem)
            {
                var dataItem = itemControl.DataContext as TimelineDataItem;

                if (dataItem.DataItem is TimeLineItem)
                {
                    var item = dataItem.DataItem as TimeLineItem;

                    var path = "";
                    if (item.Name.Contains("信息共"))
                    {
                        if (item.GroupName == "服务-国家级" || item.GroupName == "服务-省级" || item.GroupName == "灾情舆情指数")
                        {
                            if (item.GroupName == "服务-国家级")
                            {
                                var res = Controller.NationServices.FindAll(t => t.StartTime == item.StartTime);

                                var alarmPopWindow = new AlarmPopWindow(Controller, res)
                                {
                                    Owner = Application.Current.MainWindow,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                };
                                alarmPopWindow.ShowDialog();
                            }
                            else if (item.GroupName == "服务-省级")
                            {
                                var res = Controller.ProvinceServices.FindAll(t => t.StartTime == item.StartTime);

                                var alarmPopWindow = new AlarmPopWindow(Controller, res)
                                {
                                    Owner = Application.Current.MainWindow,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                };
                                alarmPopWindow.ShowDialog();
                            }
                            else if (item.GroupName == "灾情舆情指数")
                            {
                                var res = Controller.Disasters.FindAll(t => t.StartTime == item.StartTime);

                                var alarmPopWindow = new AlarmPopWindow(Controller, res)
                                {
                                    Owner = Application.Current.MainWindow,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                };
                                alarmPopWindow.ShowDialog();
                            }
                        }
                        else
                        {
                            if (item.GroupName == "预警-国家级")
                            {
                                var res = Controller.Nations.FindAll(t => t.StartTime == item.StartTime);

                                var alarmPopWindow = new AlarmPopWindow(Controller, res)
                                {
                                    Owner = Application.Current.MainWindow,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                };
                                alarmPopWindow.ShowDialog();
                            }
                            else if (item.GroupName == "预警-省级")
                            {
                                var res = Controller.Provinces.FindAll(t => t.StartTime == item.StartTime);

                                var alarmPopWindow = new AlarmPopWindow(Controller, res)
                                {
                                    Owner = Application.Current.MainWindow,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                };
                                alarmPopWindow.ShowDialog();
                            }
                            else if (item.GroupName == "预警-市级")
                            {
                                var res = Controller.Citys.FindAll(t => t.StartTime == item.StartTime);

                                var alarmPopWindow = new AlarmPopWindow(Controller, res)
                                {
                                    Owner = Application.Current.MainWindow,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                };
                                alarmPopWindow.ShowDialog();
                            }
                            else if (item.GroupName == "预警-县级")
                            {
                                var res = Controller.Countys.FindAll(t => t.StartTime == item.StartTime);

                                var alarmPopWindow = new AlarmPopWindow(Controller, res)
                                {
                                    Owner = Application.Current.MainWindow,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                };
                                alarmPopWindow.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        if (item.GroupName == "服务-国家级" || item.GroupName == "服务-省级" || item.GroupName == "灾情舆情指数")
                        {
                            path = item.ServiceUrl;
                            if (string.IsNullOrWhiteSpace(path))
                            {
                            }
                            else
                            {
                                var documentsWindow = new DocumentsWindow(path, item.GroupName)
                                {
                                    Owner = Application.Current.MainWindow,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                };
                                documentsWindow.ShowDialog();
                            }
                        }
                        else
                        {
                            m_Controller.LoadAlarmDetailWindowData(item.Id);

                            var alarmDetail = new AlarmDetail(m_Controller.CurrentAlarmDetail)
                            {
                                Owner = Application.Current.MainWindow,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            };
                            alarmDetail.ShowDialog();
                        }
                    }
                }
            }
        }

        // 返回按钮
        private void ReturnButton_OnClick(object sender, RoutedEventArgs e)
        {
            returnButton.Visibility = Visibility.Collapsed;
            m_Controller.TimeBarTitle = "实况";
            realtimeRainControl.Visibility = Visibility.Visible; //堆积柱图显示
            emergencyMultiControl.Visibility = Visibility.Collapsed; //突发事件图隐藏
            singleRealtimeRainControl.Visibility = Visibility.Collapsed; //暴雨单站隐藏
            windLine.Visibility = Visibility.Collapsed; //大风单站隐藏
            coldWaveLine.Visibility = Visibility.Collapsed; //寒潮单站隐藏
        }

        // 图表加载事件
        private void TimeLineChart_Loaded(object sender, RoutedEventArgs e)
        {
            m_Controller.TimeBarChangeCommand = new DelegateCommand<StationInfo>(ChangeToSingleBar);
            m_Controller.EmergencyOrWarningChangeCommand = new DelegateCommand<string>(EmergencyOrWarningChange);
        }

        private void EmergencyOrWarningChange(string obj)
        {
            if (obj == "预警态势")
            {
                realtimeRainControl.Visibility = Visibility.Visible;
                emergencyMultiControl.Visibility = Visibility.Collapsed;
                singleRealtimeRainControl.Visibility = Visibility.Collapsed; //暴雨单站隐藏
                windLine.Visibility = Visibility.Collapsed; //大风单站隐藏
                coldWaveLine.Visibility = Visibility.Collapsed; //寒潮单站隐藏
                typhoonChart.Visibility = Visibility.Collapsed;
                realtimeRainControl.ChangeYAxis();
            }
            else if (obj == "突发事件")
            {
                realtimeRainControl.Visibility = Visibility.Collapsed;
                emergencyMultiControl.Visibility = Visibility.Visible;
                singleRealtimeRainControl.Visibility = Visibility.Collapsed; //暴雨单站隐藏
                windLine.Visibility = Visibility.Collapsed; //大风单站隐藏
                coldWaveLine.Visibility = Visibility.Collapsed; //寒潮单站隐藏
                typhoonChart.Visibility = Visibility.Collapsed;
                //emergencyMultiControl.ChangeYAxis();
            }
            else if (obj == "台风")
            {
                realtimeRainControl.Visibility = Visibility.Collapsed;
                emergencyMultiControl.Visibility = Visibility.Visible;
                singleRealtimeRainControl.Visibility = Visibility.Collapsed; //暴雨单站隐藏
                windLine.Visibility = Visibility.Collapsed; //大风单站隐藏
                coldWaveLine.Visibility = Visibility.Collapsed; //寒潮单站隐藏
                typhoonChart.Visibility = Visibility.Collapsed;
                //emergencyMultiControl.ChangeYAxis();
            }
        }

        // 图表数据上下文更改事件
        private void TimeLineChart_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Controller = DataContext as TimeLineChartController;

            if (!m_Init)
            {
                if (Controller != null)
                {
                    DataContext = Controller;
                    m_Init = true;
                }
            }
        }

        // Timeline时间间隔改变事件
        private void Timeline_OnItemIntervalChanged(object sender, DrillEventArgs e)
        {
        }

        #endregion

    }
}