using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using CMAPlatform.Chart.Controller;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCPSOE.Models.PageModel;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     EventInfoShowList.xaml 的交互逻辑
    /// </summary>
    public partial class EventInfoShowList : ChartViewBase, INotifyPropertyChanged
    {
        public EventInfoShowList(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            Loaded += EventInfoShowList_Loaded;
            m_Controller = Controllers[0] as EventInfoShowListController;

            DataContext = m_Controller;

            //事件页底部切换消息间隔，暂时设置30秒
            bgTimer.Interval = new TimeSpan(0, 0, 30);
            bgTimer.Tick += bgTimer_Tick;
            bgTimer.Start();
        }

        private void EventInfoShowList_Loaded(object sender, RoutedEventArgs e)
        {
            m_Controller.EventInfoList.Add("国家气象局领导批示：地方立即组织资源前往");
            //m_Controller.EventInfoList.Add("测试1测试1测试1测试1测试1测试1测试1测试1测试1测试1测试1");
            //m_Controller.EventInfoList.Add("测试2测试2测试2测试2测试2测试2测试2测试2测试2测试2测试2");
            //m_Controller.EventInfoList.Add("测试3测试3测试3测试3测试3测试3测试3测试3测试3测试3测试3");
            m_Controller.CurEventInfo = m_Controller.EventInfoList[0];
            //渐隐效果
            var doubleAnimation = new DoubleAnimationUsingKeyFrames();
            var sd = new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)));
            var sd1 = new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300)));
            doubleAnimation.KeyFrames.Add(sd);
            doubleAnimation.KeyFrames.Add(sd1);
            Storyboard.SetTarget(doubleAnimation, border); //设置目标
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(OpacityProperty));

            //渐现效果
            var doubleAnimation1 = new DoubleAnimationUsingKeyFrames();
            //doubleAnimation1.BeginTime = new TimeSpan(0, 0, 0, 1, 5);
            var sd2 = new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)));
            var sd3 = new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1000)));
            doubleAnimation1.KeyFrames.Add(sd2);
            doubleAnimation1.KeyFrames.Add(sd3);
            Storyboard.SetTarget(doubleAnimation1, border); //设置目标
            Storyboard.SetTargetProperty(doubleAnimation1, new PropertyPath(OpacityProperty));

            storyboard.Children.Add(doubleAnimation); //添加故事板
            storyboard.Children.Add(doubleAnimation1); //添加故事板
        }

        private void bgTimer_Tick(object sender, EventArgs e)
        {
            if (m_Controller.EventInfoList.Count > 0)
            {
                if (InfoIndex >= m_Controller.EventInfoList.Count)
                {
                    InfoIndex = 0;
                }
                if (InfoIndex == 1 && m_Controller.EventInfoList.Count == 1)
                {
                    bgTimer.Stop();
                }
                else
                {
                    storyboard.Begin(border, true);
                    storyboard.Completed += storyboard_Completed;
                    m_Controller.CurEventInfo = m_Controller.EventInfoList[InfoIndex];
                    storyboard1.Begin(border, true);
                    storyboard1.Completed += storyboard_Completed1;
                    InfoIndex++;
                }
            }
        }

        private void storyboard_Completed1(object sender, EventArgs e)
        {
            storyboard1.Stop(border);
        }

        private void storyboard_Completed(object sender, EventArgs e)
        {
            storyboard.Stop(border);
        }

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

        #region Properties

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly EventInfoShowListController m_Controller;


        private Page3DModel m_Page3dModel;


        private Guid m_PageId = Guid.Empty;

        private Guid m_PageInstanceId = Guid.NewGuid();

        private IMessageAggregator m_MessageAggregator = new MessageAggregator();

        private readonly Storyboard storyboard = new Storyboard();
        private readonly Storyboard storyboard1 = new Storyboard();
        private readonly DispatcherTimer bgTimer = new DispatcherTimer();
        private int InfoIndex = 1;

        #endregion
    }
}