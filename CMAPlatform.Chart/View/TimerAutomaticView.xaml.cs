using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using CMAPlatform.Chart.Window;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using Digihail.CCP.Utilities;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Microsoft.Practices.Prism;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     TimerAutomatic.xaml 的交互逻辑
    /// </summary>
    public partial class TimerAutomaticView : ChartViewBase
    {
        private readonly CreateData createData = new CreateData();

        private readonly DispatcherTimer m_AIStimer = new DispatcherTimer();
        private ResultModel resultModel = new ResultModel();

        public TimerAutomaticView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            var temp = DataManager.Instance;
            CreateDataResult();
        }

        /// <summary>
        ///     计时入库
        /// </summary>
        private void CreateDataResult()
        {
            m_AIStimer.Interval = TimeSpan.FromSeconds(300);
            m_AIStimer.Tick += m_AIStimer_Tick;
            //Thread vThread = new Thread(() =>
            //{
            //    resultModel = createData.CreateDataResult();
            //});
            //if (resultModel.Result == false)
            //{
            //    textBlock.Text = resultModel.ErrorMessage;
            //    textBlock.Visibility = Visibility.Visible;
            //}
            //vThread.Start();
            CreateData();
            m_AIStimer.Start();
        }

        private void m_AIStimer_Tick(object sender, EventArgs e)
        {
            //Thread vThread = new Thread(() =>
            //{
            //    CreateData createData = new CreateData();
            //    resultModel = createData.CreateDataResult();

            //});
            //if (resultModel.Result==false)
            //{
            //    textBlock.Text = resultModel.ErrorMessage;
            //    textBlock.Visibility = Visibility.Visible;
            //}
            //vThread.Start();

            CreateData();
        }

        /// <summary>
        ///     创建数据方法
        /// </summary>
        [LogCCPError]
        private void CreateData()
        {
            var bgBackgroundWorker = new BackgroundWorker();
            bgBackgroundWorker.DoWork += (sender, args) =>
            {
                var result = createData.CreateDataResult();
                args.Result = result;
            };
            bgBackgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var result = args.Result as ResultModel;
                    if (result != null)
                    {
                        if (result.Result == false)
                        {
                            CcpLoggerManager.WriteMessageLog(result.ErrorMessage);
                        }
                        DataManager.Instance.EventModels.Clear();
                        DataManager.Instance.EventModels.AddRange(result.EventListModel.Events);

                        var checkeditem = DataManager.Instance.RiskSituationModels.FirstOrDefault(t => t.IsCheck2);


                        DataManager.Instance.RiskSituationModels.Clear();
                        DataManager.Instance.RiskSituationModels.AddRange(
                            result.RiskSituationModels.OrderBy(p => p.OrderId));

                        if (checkeditem != null)
                        {
                            var type = checkeditem.WarningType;

                            var newmodel =
                                DataManager.Instance.RiskSituationModels.FirstOrDefault(t => t.WarningType == type);
                            if (newmodel != null)
                            {
                                newmodel.IsCheck2 = true;
                            }
                        }

                        DataManager.Instance.ExemtremeWeatherModel.Clear();
                        DataManager.Instance.ExemtremeWeatherModel.AddRange(result.ExemtremeWeatherModels);
                    }
                });
            };
            bgBackgroundWorker.RunWorkerAsync();
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

        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var comprehenSive = new ComprehenSiveWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            comprehenSive.Show();
        }
    }
}