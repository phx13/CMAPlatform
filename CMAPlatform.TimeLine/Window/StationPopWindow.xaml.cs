using System.Windows.Controls;
using CMAPlatform.DataClient;
using CMAPlatform.Models.TimeBarModel;
using Digihail.AVE.Launcher.Controls;

namespace CMAPlatform.TimeLine.Window
{
    /// <summary>
    ///     StationPopWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StationPopWindow : PopWindow
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public StationPopWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="controller">Controller</param>
        public StationPopWindow(TimeLineChartController controller)
        {
            InitializeComponent();

            if (controller != null)
            {
                DataContext = controller;
            }
        }

        //选中事件
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectItem =
                (DataContext as TimeLineChartController).CurrentStationInfo = e.AddedItems[0] as StationInfo;

            if (DataManager.Instance.IsTestData)
            {
                //Random mRandom = new Random();

                //switch (mRandom.Next(1, 4))
                //{
                //    case 1:
                //        selectItem.EventType = "11B03";
                //        break;
                //    case 2:
                //        selectItem.EventType = "11B05";
                //        break;
                //    case 3:
                //        selectItem.EventType = "11B06";
                //        break;
                //}
                selectItem.EventType = "11B03";
            }

            if (selectItem != null)
            {
                //暴雨
                if (selectItem.EventType == "11B03")
                {
                    // 图表跳转
                    (DataContext as TimeLineChartController).TimeBarChangeCommand.Execute(selectItem);
                    // 加载降水柱图数据
                    switch (selectItem.EventColor)
                    {
                        case "blue":
                            (DataContext as TimeLineChartController).SingleTimeBarChangeCommand.Execute("12");
                            break;
                        case "orange":
                            (DataContext as TimeLineChartController).SingleTimeBarChangeCommand.Execute("3");
                            break;
                        case "yellow":
                            (DataContext as TimeLineChartController).SingleTimeBarChangeCommand.Execute("6");
                            break;
                        case "red":
                            (DataContext as TimeLineChartController).SingleTimeBarChangeCommand.Execute("3");
                            break;
                        default:
                            (DataContext as TimeLineChartController).SingleTimeBarChangeCommand.Execute("12");
                            break;
                    }
                }
                //大风
                else if (selectItem.EventType == "11B06")
                {
                    // 图表跳转
                    (DataContext as TimeLineChartController).TimeBarChangeCommand.Execute(selectItem);
                    // 加载大风折线图数据
                    (DataContext as TimeLineChartController).LoadWindLineData(selectItem);
                }
                //寒潮
                else if (selectItem.EventType == "11B05")
                {
                    // 图表跳转
                    (DataContext as TimeLineChartController).TimeBarChangeCommand.Execute(selectItem);
                    // 加载寒潮折线图数据
                    (DataContext as TimeLineChartController).ColdWaveLineChangeCommand.Execute("48");
                }
            }
            // 摘除选择变更事件 
            listBox.SelectionChanged -= Selector_OnSelectionChanged;

            // 关闭窗体
            Close();
        }
    }
}