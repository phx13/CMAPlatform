using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Digihail.CCP.Utilities;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Controls.ChartView;

namespace CMAPlatform.TimeLine.Controls
{
    /// <summary>
    ///     SingleRealtimeRainControl.xaml 的交互逻辑
    /// </summary>
    public partial class SingleRealtimeRainControl : UserControl
    {
        private bool m_Init;

        private bool m_IsFirstLoad = true;

        /// <summary>
        ///     构造函数
        /// </summary>
        public SingleRealtimeRainControl()
        {
            InitializeComponent();

            Loaded += SingleRealtimeRainControl_Loaded;
        }

        private void SingleRealtimeRainControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_Init == false)
            {
                (DataContext as TimeLineChartController).SingleTimeBarChangeCommand =
                    new DelegateCommand<string>(SelectButton);
                m_Init = true;
            }
        }

        // 显示时间跨度更改
        private void BarTimeSpanChanged(object sender, RoutedEventArgs e)
        {
            var tag = (sender as RadioButton).Tag.ToString();
            var controller = DataContext as TimeLineChartController;
            controller.LoadSingleBarData(controller.CurrentStationInfo, tag);
            ChangeYAxis();
            //controller.CurrentSingleBarTimespan = tag;
            ChangeAnnotations(tag);
        }

        // 按钮选中
        private void SelectButton(string tag)
        {
            switch (tag)
            {
                case "1":
                    button1.IsChecked = false;
                    button1.IsChecked = true;
                    break;
                case "3":
                    button3.IsChecked = false;
                    button3.IsChecked = true;
                    break;
                case "6":
                    button6.IsChecked = false;
                    button6.IsChecked = true;
                    break;
                case "12":
                    button12.IsChecked = false;
                    button12.IsChecked = true;
                    break;
                case "24":
                    button24.IsChecked = false;
                    button24.IsChecked = true;
                    break;
            }
        }

        /// <summary>
        ///     切换Y轴刻度
        /// </summary>
        public void ChangeYAxis()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);

            dispatcherTimer.Tick += (sender, args) =>
            {
                dispatcherTimer.Stop();

                var range = (maximumChart.VerticalAxis as LinearAxis).ActualRange;

                var tbmin = this.GetChildByName<TextBlock>("mintb");
                var tbmax = this.GetChildByName<TextBlock>("maxtb");

                if (tbmin != null && tbmax != null)
                {
                    tbmin.Text = string.Format("{0}mm", range.Minimum);
                    tbmax.Text = string.Format("{0}mm", range.Maximum);
                }
            };
            dispatcherTimer.Start();
        }

        // 更改参考线
        private void ChangeAnnotations(string tag)
        {
            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += delegate
            {
                if (m_IsFirstLoad)
                {
                    Thread.Sleep(100);
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    maximumChart.Annotations.Clear();
                    var annotation1 = new CartesianGridLineAnnotation();
                    var annotation2 = new CartesianGridLineAnnotation();

                    var tbmark1 = this.GetChildByName<TextBlock>("marktb1");
                    var tbmark2 = this.GetChildByName<TextBlock>("marktb2");
                    tbmark1.Visibility = tbmark2.Visibility = Visibility.Hidden;
                    var grid = this.GetChildByName<Grid>("headerGrid");

                    var dispatcherTimer = new DispatcherTimer();
                    dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);

                    switch (tag)
                    {
                        case "1":
                        case "3":
                            annotation1.Axis = maximumChart.VerticalAxis;
                            annotation1.Value = 100;
                            annotation1.Stroke = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF4D4D"));
                            annotation1.StrokeThickness = 2;
                            maximumChart.Annotations.Add(annotation1);

                            annotation2.Axis = maximumChart.VerticalAxis;
                            annotation2.Value = 50;
                            annotation2.Stroke = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFA64D"));
                            annotation2.StrokeThickness = 2;
                            maximumChart.Annotations.Add(annotation2);

                            dispatcherTimer.Tick += (sender, args) =>
                            {
                                dispatcherTimer.Stop();
                                var range = (maximumChart.VerticalAxis as LinearAxis).ActualRange;
                                tbmark1.Text = "100mm";
                                tbmark1.Margin = new Thickness(0, 0, 0, 100*grid.ActualHeight/range.Maximum);
                                tbmark1.Visibility = Visibility.Visible;

                                tbmark2.Text = "50mm";
                                tbmark2.Margin = new Thickness(0, 0, 0, 50*grid.ActualHeight/range.Maximum);
                                tbmark2.Visibility = Visibility.Visible;
                            };
                            dispatcherTimer.Start();
                            break;
                        case "6":
                            annotation1.Axis = maximumChart.VerticalAxis;
                            annotation1.Value = 50;
                            annotation1.Stroke = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFFF00"));
                            annotation1.StrokeThickness = 2;
                            maximumChart.Annotations.Add(annotation1);

                            dispatcherTimer.Tick += (sender, args) =>
                            {
                                dispatcherTimer.Stop();
                                var range = (maximumChart.VerticalAxis as LinearAxis).ActualRange;
                                tbmark2.Text = "50mm";
                                tbmark2.Margin = new Thickness(0, 0, 0, 50*grid.ActualHeight/range.Maximum);
                                tbmark2.Visibility = Visibility.Visible;
                            };
                            dispatcherTimer.Start();
                            break;
                        case "24":
                        case "12":
                            annotation1.Axis = maximumChart.VerticalAxis;
                            annotation1.Value = 50;
                            annotation1.Stroke = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#00BFFF"));
                            annotation1.StrokeThickness = 2;
                            maximumChart.Annotations.Add(annotation1);

                            dispatcherTimer.Tick += (sender, args) =>
                            {
                                dispatcherTimer.Stop();
                                var range = (maximumChart.VerticalAxis as LinearAxis).ActualRange;
                                tbmark2.Text = "50mm";
                                tbmark2.Margin = new Thickness(0, 0, 0, 50*grid.ActualHeight/range.Maximum);
                                tbmark2.Visibility = Visibility.Visible;
                                //maximumChart.RefreshNode(new AxisLabelModel());
                            };
                            dispatcherTimer.Start();
                            break;
                    }
                }));
            };

            backgroundWorker.RunWorkerCompleted += delegate
            {
                if (m_IsFirstLoad)
                {
                    m_IsFirstLoad = false;

                    switch (tag)
                    {
                        case "1":
                            button24.IsChecked = true;
                            Thread.Sleep(100);
                            button1.IsChecked = true;
                            break;
                        case "3":
                            button24.IsChecked = true;
                            Thread.Sleep(100);
                            button3.IsChecked = true;
                            break;
                        case "6":
                            button24.IsChecked = true;
                            Thread.Sleep(100);
                            button6.IsChecked = true;
                            break;
                        case "12":
                            button24.IsChecked = true;
                            Thread.Sleep(100);
                            button12.IsChecked = true;
                            break;
                        case "24":
                            button1.IsChecked = true;
                            Thread.Sleep(100);
                            button24.IsChecked = true;
                            break;
                    }
                }
            };

            backgroundWorker.RunWorkerAsync();
        }
    }
}