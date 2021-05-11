using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CMAPlatform.Models.TimeLineModel;
using Digihail.AVE.Launcher.Controls;

namespace CMAPlatform.TimeLine.Window
{
    /// <summary>
    ///     AlarmPopWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmPopWindow : PopWindow
    {
        //private string m_Title;
        //public string Title
        //{
        //    get { return m_Title; }
        //    set
        //    {
        //        m_Title = value;
        //    }
        //}
        private readonly TimeLineChartController m_Controller;

        /// <summary>
        ///     构造函数
        /// </summary>
        public AlarmPopWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="controller">Controller</param>
        public AlarmPopWindow(TimeLineChartController controller, List<TimeLineItem> items)
        {
            InitializeComponent();
            DataContext = this;
            listBox.ItemsSource = Items = new ObservableCollection<TimeLineItem>(items);
            m_Controller = controller;
            if (items[0].GroupName.StartsWith("服务"))
            {
                Title = "服务信息";
            }
            else if (items[0].GroupName == "灾情舆情指数")
            {
                Title = "灾情信息";
            }
            else
            {
                Title = "预警信息";
            }
        }

        public ObservableCollection<TimeLineItem> Items { get; set; }

        //选中事件
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //// 加载该站数据
            //(this.DataContext as TimeLineChartController).LoadSingleBarData(e.AddedItems[0] as StationInfo);

            //// 图表跳转
            //(this.DataContext as TimeLineChartController).TimeBarChangeCommand.Execute(e.AddedItems[0] as StationInfo);

            //// 摘除选择变更事件 
            //this.listBox.SelectionChanged -= Selector_OnSelectionChanged;

            //// 关闭窗体
            //this.Close();
        }

        private void listBox_Selected(object sender, RoutedEventArgs e)
        {
            var tli = listBox.SelectedItems[0] as TimeLineItem;
            if (tli != null)
            {
                if (tli.GroupName == "服务-国家级" || tli.GroupName == "服务-省级" || tli.GroupName == "灾情舆情指数")
                {
                    var path = tli.ServiceUrl;
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        return;
                    }
                    var documentsWindow = new DocumentsWindow(path, tli.GroupName)
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    documentsWindow.ShowDialog();
                }
                else
                {
                    m_Controller.LoadAlarmDetailWindowData(tli.Id);
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