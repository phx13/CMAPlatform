using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CMAPlatform.TimeLine.Controls;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.CCP.UserControls;
using Digihail.CCP.Utilities;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Microsoft.Practices.Prism.Events;

namespace CMAPlatform.TimeLine
{
    /// <summary>
    ///     OnGis3DChartView.xaml 的交互逻辑
    /// </summary>
    public partial class OnGis3DChartView : ChartViewBase
    {
        /// <summary>
        ///     三维球所在Column
        /// </summary>
        private readonly ColumnDefinition m_GlobalColumnDefinition = new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        };

        /// <summary>
        ///     泳道图所在Column
        /// </summary>
        private readonly ColumnDefinition m_TimeLineColumnDefinition = new ColumnDefinition
        {
            Width = new GridLength(0, GridUnitType.Pixel)
        };

        private ExtenComponentsControl ExtenComponentsControl;

        private NewLayoutPanelContainer LayoutPanelContainer;
        private TimeLineChartController m_Controller;


        private Grid m_Grid;

        private bool m_HasAddTimeLineChart;

        private bool m_Init;

        private ToggleButton m_tgButton;

        /// <summary>
        ///     泳道图图表
        /// </summary>
        private TimeLineChart m_TimeLineChart;

        private Grid m_TimeLineInnerGrid;


        /// <summary>
        ///     泳道图容器
        /// </summary>
        private Grid m_TimeLineOuterGrid;

        public OnGis3DChartView(ChartViewBaseModel model) : base(model)
        {
            InitializeComponent();
            Loaded += MainChartView_Loaded;
        }

        private void MainChartView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!m_Init)
            {
                m_Controller = Controllers[0] as TimeLineChartController;
                DataContext = m_Controller;
                OnDadChartLoaded();
                m_Init = true;

                if (m_Controller != null)
                {
                    m_Controller.MessageAggregator.GetMessage<StateChangedMessage>()
                        .Subscribe(ReceiveStateChangedMessage, ThreadOption.UIThread);
                }

                if (GetGrid())
                {
                    var result = AddTimeLineChart();
                }
            }
        }

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

        public override void Dispose()
        {
            if (m_Controller != null)
            {
                m_Controller.MessageAggregator.GetMessage<StateChangedMessage>().Unsubscribe(ReceiveStateChangedMessage);
            }

            base.Dispose();
        }

        #endregion

        #region Method

        /// <summary>
        ///     通过视觉树找到对应Grid的方法
        /// </summary>
        private bool GetGrid()
        {
            var find = false;
            try
            {
                var outgrid = this.GetParentByName<Grid>("outgrid");
                if (outgrid != null)
                {
                    m_Grid = outgrid.GetChildByName<Grid>("grid");
                    LayoutPanelContainer = outgrid.GetChildByName<NewLayoutPanelContainer>("layoutPanelContainer");
                    ExtenComponentsControl = outgrid.GetChildByName<ExtenComponentsControl>("userControl");

                    if (m_Grid != null)
                    {
                        find = true;
                        m_Grid.ColumnDefinitions.Clear();
                        m_Grid.ColumnDefinitions.Add(m_GlobalColumnDefinition);
                        m_Grid.ColumnDefinitions.Add(m_TimeLineColumnDefinition);
                        if (m_Grid.Children != null)
                        {
                            foreach (UIElement child in m_Grid.Children)
                            {
                                Grid.SetColumn(child, 0);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                find = false;
            }
            return find;
        }

        /// <summary>
        ///     把泳道图加到Grid中
        /// </summary>
        private bool AddTimeLineChart()
        {
            var result = false;
            try
            {
                if (m_TimeLineChart == null)
                {
                    m_TimeLineChart = new TimeLineChart();

                    var binding = new Binding("DataContext");
                    binding.Source = this;
                    binding.Mode = BindingMode.OneWay;
                    m_TimeLineChart.SetBinding(DataContextProperty, binding);

                    m_TimeLineChart.HorizontalAlignment = HorizontalAlignment.Stretch;
                    m_TimeLineChart.VerticalAlignment = VerticalAlignment.Stretch;
                }
                if (!m_HasAddTimeLineChart)
                {
                    m_TimeLineOuterGrid = new Grid();
                    m_TimeLineOuterGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    m_TimeLineOuterGrid.VerticalAlignment = VerticalAlignment.Stretch;
                    m_TimeLineOuterGrid.Visibility = Visibility.Collapsed;


                    m_TimeLineInnerGrid = new Grid();

                    m_TimeLineInnerGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});
                    m_TimeLineInnerGrid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });

                    m_tgButton = new ToggleButton();
                    m_tgButton.Cursor = Cursors.Hand;
                    m_tgButton.Template = FindResource("TimelineToogleButton") as ControlTemplate;
                    m_tgButton.Width = 30;
                    m_tgButton.HorizontalAlignment = HorizontalAlignment.Left;
                    m_tgButton.VerticalAlignment = VerticalAlignment.Stretch;

                    m_tgButton.Checked += M_tgButton_Checked;
                    m_tgButton.Unchecked += M_tgButton_Checked;

                    m_TimeLineInnerGrid.Children.Add(m_tgButton);
                    m_TimeLineInnerGrid.Children.Add(m_TimeLineChart);

                    var viewbox = new Viewbox();
                    viewbox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    viewbox.VerticalAlignment = VerticalAlignment.Stretch;
                    viewbox.Stretch = Stretch.Fill;
                    viewbox.Child = m_TimeLineInnerGrid;

                    m_TimeLineOuterGrid.Children.Add(viewbox);


                    Grid.SetColumn(m_tgButton, 0);
                    Grid.SetColumn(m_TimeLineChart, 1);

                    m_Grid.Children.Add(m_TimeLineOuterGrid);
                    Grid.SetColumn(m_TimeLineOuterGrid, 1);
                    m_HasAddTimeLineChart = true;

                    InitBackgroundLayer();
                }
            }
            catch (Exception ee)
            {
                result = false;
            }
            return result;
        }

        private void M_tgButton_Checked(object sender, RoutedEventArgs e)
        {
            var tbtn = sender as ToggleButton;
            if (tbtn != null)
            {
                if (tbtn.IsChecked == true)
                {
                    ShowAllTimeLine();
                }
                else
                {
                    ShowNormalTimeLine();
                }
            }
        }

        /// <summary>
        ///     接收跳转事件页消息
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveStateChangedMessage(StateChangedModel obj)
        {
            if (obj != null)
            {
                if (obj.StateName.Contains("事件页"))
                {
                    ShowNormalTimeLine();
                }
                else
                {
                    CollapseTimeLine();
                }
            }
        }

        /// <summary>
        ///     显示Timeline
        /// </summary>
        private void ShowNormalTimeLine()
        {
            if (m_GlobalColumnDefinition != null && m_TimeLineColumnDefinition != null && m_TimeLineOuterGrid != null)
            {
                m_GlobalColumnDefinition.Width = new GridLength(3, GridUnitType.Star);
                m_TimeLineColumnDefinition.Width = new GridLength(2, GridUnitType.Star);
                m_TimeLineInnerGrid.Width = 1536;
                m_TimeLineInnerGrid.Height = 1352;
                m_TimeLineOuterGrid.Visibility = Visibility.Visible;
                LayoutPanelContainer.Visibility = Visibility.Visible;
                //ExtenComponentsControl.Visibility = Visibility.Visible;

                if (m_TimeLineChart != null)
                {
                    m_TimeLineChart.SetBackground(false);
                }

                ResetBackgroundLayer();
            }
        }

        /// <summary>
        ///     显示全时间轴
        /// </summary>
        private void ShowAllTimeLine()
        {
            if (m_GlobalColumnDefinition != null && m_TimeLineColumnDefinition != null && m_TimeLineOuterGrid != null)
            {
                m_GlobalColumnDefinition.Width = new GridLength(0, GridUnitType.Pixel);
                m_TimeLineColumnDefinition.Width = new GridLength(1, GridUnitType.Star);
                m_TimeLineInnerGrid.Width = 3840;
                m_TimeLineInnerGrid.Height = 1352;
                m_TimeLineOuterGrid.Visibility = Visibility.Visible;
                LayoutPanelContainer.Visibility = Visibility.Collapsed;
                ExtenComponentsControl.Visibility = Visibility.Collapsed;

                if (m_TimeLineChart != null)
                {
                    m_TimeLineChart.SetBackground(true);
                }

                ResetBackgroundLayer();
            }
        }

        /// <summary>
        ///     隐藏时间轴
        /// </summary>
        private void CollapseTimeLine()
        {
            if (m_GlobalColumnDefinition != null && m_TimeLineColumnDefinition != null && m_TimeLineOuterGrid != null)
            {
                m_GlobalColumnDefinition.Width = new GridLength(1, GridUnitType.Star);
                m_TimeLineColumnDefinition.Width = new GridLength(0, GridUnitType.Pixel);
                m_TimeLineInnerGrid.Width = 0;
                m_TimeLineInnerGrid.Height = 1352;
                m_TimeLineOuterGrid.Visibility = Visibility.Collapsed;
                LayoutPanelContainer.Visibility = Visibility.Visible;
                ExtenComponentsControl.Visibility = Visibility.Visible;

                if (m_TimeLineChart != null)
                {
                    m_TimeLineChart.SetBackground(false);
                }

                ResetBackgroundLayer();
            }
        }

        private void InitBackgroundLayer()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(3000);
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var temp = LayoutPanelContainer.ActualWidth;
                    LayoutPanelContainer.LayoutPanelRealWidth = temp - 1;
                    LayoutPanelContainer.LayoutPanelRealWidth = temp;
                }));
            };
            timer.Start();
        }


        private void ResetBackgroundLayer()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            var i = 0;
            timer.Tick += (sender, args) =>
            {
                if (i >= 10)
                {
                    timer.Stop();
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var temp = LayoutPanelContainer.ActualWidth;
                    LayoutPanelContainer.LayoutPanelRealWidth = temp - 1;
                    LayoutPanelContainer.LayoutPanelRealWidth = temp;
                }));
                i++;
            };
            timer.Start();
        }

        #endregion
    }
}