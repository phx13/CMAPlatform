using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;
using CMAPlatform.Chart.Controller;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCP.Helper;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.CCP.Utilities;
using Digihail.CCPSOE.Models.PageModel;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Microsoft.Practices.Prism.Events;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     LayerControl.xaml 的交互逻辑
    /// </summary>
    public partial class LayerControl : ChartViewBase, INotifyPropertyChanged
    {
        private bool m_init;

        private CheckBox m_LastCheckBox;


        private DispatcherTimer m_Timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(DataManager.Instance.DelaySeconds)
        };

        public LayerControl(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            Loaded += LayerControl_Loaded;
            m_Controller = Controllers[0] as LayerControlController;

            DataContext = m_Controller;

            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>()
                .Subscribe(ReceiveCMAMainPageSelectedTabMessage, ThreadOption.UIThread);
        }

        private void ReceiveStateChangedMessage(StateChangedModel obj)
        {
            m_Controller.LayerVisibilityControl(m_Controller.TextGuids, false);
            m_Timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(DataManager.Instance.DelaySeconds)};
            m_Timer.Tick += M_Timer_Tick;
            m_Timer.Start();
        }

        private void LayerControl_Loaded(object sender, RoutedEventArgs e)
        {
            OnDadChartLoaded();
            //读取xmal里的序列化图片对应信息
            LayerPicCollection = LoadData();
            //获取当前页面的信息
            m_Page3dModel = CCPHelper.Instance.GetCurrentCCPPageModel() as Page3DModel;

            if (m_Page3dModel != null)
            {
                m_Controller.PageId = m_Page3dModel.ID;
                var roorCatagory = m_Page3dModel.GisObjectCategorys;
                if (roorCatagory != null && roorCatagory.Count == 2)
                {
                    var COECatagory = roorCatagory[0];
                    if (COECatagory != null && COECatagory.SubCategorys != null)
                    {
                        for (var i = 1; i < COECatagory.SubCategorys.Count; i++)
                        {
                            var curlayermodel = new LayerModel();
                            if (curlayermodel != null)
                            {
                                curlayermodel.ObjectModelId = COECatagory.SubCategorys[i].ObjectModelId;
                                curlayermodel.IsObject = COECatagory.SubCategorys[i].IsObject;
                                curlayermodel.LayerName = COECatagory.SubCategorys[i].CategoryName;
                                curlayermodel.IsChecked = COECatagory.SubCategorys[i].IsChecked;
                                curlayermodel.IsExpand = COECatagory.SubCategorys[i].IsExpand;
                                curlayermodel.SubLayerPicCollection = new ObservableCollection<LayerModel>();
                                if (COECatagory.SubCategorys[i].CategoryName == "基础信息")
                                {
                                    curlayermodel.SelectType = true;
                                }
                                if (COECatagory.SubCategorys[i].CategoryName == "隐患点")
                                {
                                    var cursublayermodel = new LayerModel();
                                    cursublayermodel.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel.LayerName = "中小河流";
                                    cursublayermodel.IsChecked = false;
                                    cursublayermodel.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel);
                                    var cursublayermodel1 = new LayerModel();
                                    cursublayermodel1.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel1.LayerName = "山洪沟";
                                    cursublayermodel1.IsChecked = false;
                                    cursublayermodel1.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel1);
                                    var cursublayermodel2 = new LayerModel();
                                    cursublayermodel2.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel2.LayerName = "地质灾害";
                                    cursublayermodel2.IsChecked = false;
                                    cursublayermodel2.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel2);
                                    var cursublayermodel3 = new LayerModel();
                                    cursublayermodel3.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel3.LayerName = " 城镇易涝";
                                    cursublayermodel3.IsChecked = false;
                                    cursublayermodel3.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel3);
                                }

                                for (var j = 0; j < COECatagory.SubCategorys[i].SubCategorys.Count; j++)
                                {
                                    var cursublayermodel = new LayerModel();
                                    cursublayermodel.ObjectModelId =
                                        COECatagory.SubCategorys[i].SubCategorys[j].ObjectModelId;
                                    curlayermodel.IsObject = COECatagory.SubCategorys[i].SubCategorys[j].IsObject;
                                    cursublayermodel.LayerName =
                                        COECatagory.SubCategorys[i].SubCategorys[j].CategoryName;
                                    cursublayermodel.IsChecked = COECatagory.SubCategorys[i].SubCategorys[j].IsChecked;
                                    cursublayermodel.IsExpand = COECatagory.SubCategorys[i].SubCategorys[j].IsExpand;

                                    var cursublayerpic =
                                        LayerPicCollection.ToList()
                                            .Find(t => t.SubLayerName == cursublayermodel.LayerName);
                                    if (cursublayerpic != null)
                                    {
                                        cursublayermodel.PicName = cursublayerpic.PicName;
                                    }
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel);
                                }
                                if (COECatagory.SubCategorys[i].CategoryName == "基础信息")
                                {
                                    var cursublayermodel3 = new LayerModel();
                                    cursublayermodel3.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel3.LayerName = "医院";
                                    cursublayermodel3.IsChecked = false;
                                    cursublayermodel3.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel3);
                                    var cursublayermodel5 = new LayerModel();
                                    cursublayermodel5.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel5.LayerName = "旅游景点";
                                    cursublayermodel5.IsChecked = false;
                                    cursublayermodel5.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel5);
                                    var cursublayermodel2 = new LayerModel();
                                    cursublayermodel2.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel2.LayerName = "预警设备";
                                    cursublayermodel2.IsChecked = false;
                                    cursublayermodel2.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel2);
                                    var cursublayermodel = new LayerModel();
                                    cursublayermodel.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel.LayerName = "灾害责任人";
                                    cursublayermodel.IsChecked = false;
                                    cursublayermodel.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel);
                                    var cursublayermodel1 = new LayerModel();
                                    cursublayermodel1.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel1.LayerName = "信息员";
                                    cursublayermodel1.IsChecked = false;
                                    cursublayermodel1.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel1);
                                    var cursublayermodel4 = new LayerModel();
                                    cursublayermodel4.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel4.LayerName = "易燃易爆场所";
                                    cursublayermodel4.IsChecked = false;
                                    cursublayermodel4.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel4);
                                    var cursublayermodel6 = new LayerModel();
                                    cursublayermodel6.ObjectModelId = new Guid();
                                    curlayermodel.IsObject = false;
                                    cursublayermodel6.LayerName = "山塘水库";
                                    cursublayermodel6.IsChecked = false;
                                    cursublayermodel6.IsExpand = false;
                                    curlayermodel.SubLayerPicCollection.Add(cursublayermodel6);
                                }

                                //第一个集合初始化默认展开显示
                                if (i == 1)
                                {
                                    curlayermodel.IsExpand = true;
                                }
                                else
                                {
                                    curlayermodel.IsExpand = false;
                                }

                                m_Controller.LayerCollection.Add(curlayermodel);
                            }
                        }
                        //if (m_Controller.LayerCollection.Count > 0)
                        //{
                        //    for (int i = 0; i < UPPER; i++)
                        //    {

                        //    }
                        //}
                    }
                }
            }
            m_Timer.Tick += M_Timer_Tick;
            m_Timer.Start();
            Loaded -= LayerControl_Loaded;
        }

        private void M_Timer_Tick(object sender, EventArgs e)
        {
            m_Timer.Stop();
            m_Timer.Tick -= M_Timer_Tick;
            //触发层级显隐
            if (m_Controller != null)
            {
                m_Controller.LayerVisibilityControl(m_Controller.TextGuids, true);
            }

            if (!m_init)
            {
                m_MessageAggregator.GetMessage<StateChangedMessage>()
                    .Subscribe(ReceiveStateChangedMessage, ThreadOption.UIThread);
                m_init = true;
            }
        }

        /// <summary>
        ///     从文件中读取数据
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<SubLayerPicModdel> LoadData()
        {
            var xmlSerializer = new XmlSerializer(typeof (ObservableCollection<SubLayerPicModdel>));
            var path = AppDomain.CurrentDomain.BaseDirectory + "Data/LayPic.xml";
            //string path = "D:/ZH/工作安排/气象局/CMAPlatform/CMAPlatform.Chart/Resouces/Data/LayPic-test.xml";
            if (File.Exists(path))
            {
                var fileStream = new FileStream(path, FileMode.Open);
                using (fileStream)
                {
                    return (ObservableCollection<SubLayerPicModdel>) xmlSerializer.Deserialize(fileStream);
                }
            }
            return new ObservableCollection<SubLayerPicModdel>();
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


        public override void Dispose()
        {
            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>()
                .Unsubscribe(ReceiveCMAMainPageSelectedTabMessage);
            m_MessageAggregator.GetMessage<StateChangedMessage>().Unsubscribe(ReceiveStateChangedMessage);

            base.Dispose();
        }

        #region 接收联动消息

        /// <summary>
        ///     接收列表发来的联动消息
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveCMAMainPageSelectedTabMessage(CMAMainPageSelectedTabData obj)
        {
            if (obj != null && obj.InstanceGuid != m_Controller.PageInstanceId)
            {
                switch (obj.TabName)
                {
                    case "突发事件":
                        cbCheckedBox2.IsChecked = true;
                        break;
                    case "预警态势":
                        cbCheckedBox3.IsChecked = true;
                        break;
                    case "极端天气":
                        break;
                }
            }
        }

        #endregion

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            if (sender as CheckBox == m_LastCheckBox)
            {
                m_LastCheckBox = null;

                var guids = new List<Guid>();
                guids.AddRange(m_Controller.BasicPoiLayer);
                m_Controller.LayerVisibilityControl(guids, false);
                Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "无"); },
                    DispatcherPriority.Background);

                var gcelayerData = new GceLayer
                {
                    LayerName = "全国省界",
                    LayerVisiable = false
                };
                m_MessageAggregator.GetMessage<CMAGceLayerControlMessage>().Publish(gcelayerData);
            }
        }

        #region 发送图层显隐消息

        #endregion

        #region Properties

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly LayerControlController m_Controller;


        private Page3DModel m_Page3dModel;


        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        /// <summary>
        ///     图片名称类集合
        /// </summary>
        private ObservableCollection<SubLayerPicModdel> LayerPicCollection { get; set; }

        #endregion

        #region  新 图层控制

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Layer_CheckedNew(object sender, RoutedEventArgs e)
        {
            if (m_Controller == null)
            {
                return;
            }

            var ckbox = sender as CheckBox;
            var guids = new List<Guid>();
            var ischecked = ckbox.IsChecked == true;

            if (ckbox != null && ckbox.Tag != null)
            {
                var tag = ckbox.Tag.ToString();
                switch (tag)
                {
                    case "突发事件_标签":
                        guids.AddRange(m_Controller.EmergengcyPointLayer);
                        guids.AddRange(m_Controller.EmergengcBubbleLayer);

                        if (ischecked)
                        {
                            var data = new CMAMainPageSelectedTabData
                            {
                                InstanceGuid = m_Controller.PageInstanceId,
                                TabName = "突发事件"
                            };
                            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>().Publish(data);
                        }
                        else if (cbCheckedBox3.IsChecked == true)
                        {
                            var data = new CMAMainPageSelectedTabData
                            {
                                InstanceGuid = m_Controller.PageInstanceId,
                                TabName = "风险态势"
                            };
                            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>().Publish(data);
                        }
                        break;
                    case "预警态势":
                        guids.AddRange(m_Controller.WarningLabelLayer);
                        guids.AddRange(m_Controller.WarningZoneLayer);
                        if (ischecked && cbCheckedBox2.IsChecked == false)
                        {
                            var data1 = new CMAMainPageSelectedTabData
                            {
                                InstanceGuid = m_Controller.PageInstanceId,
                                TabName = "风险态势"
                            };
                            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>().Publish(data1);
                        }
                        break;
                    default:
                        var gcelayerData = new GceLayer
                        {
                            LayerName = tag,
                            LayerVisiable = ischecked
                        };
                        m_MessageAggregator.GetMessage<CMAGceLayerControlMessage>().Publish(gcelayerData);
                        break;
                }
            }
            m_Controller.LayerVisibilityControl(guids, ischecked);
        }


        [LogCCPError]
        private void BasicInfo_Click(object sender, RoutedEventArgs e)
        {
            var rbtn = sender as CheckBox;

            var tempCheckBox = m_LastCheckBox;
            m_LastCheckBox = rbtn;

            if (tempCheckBox != null)
            {
                tempCheckBox.IsChecked = false;
            }

            var guids = new List<Guid>();
            switch (rbtn.Content.ToString())
            {
                case "无":
                default:
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, false);

                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "无"); },
                        DispatcherPriority.Background);
                    break;
                case "灾害责任人":
                    //暂无
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, false);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "无"); },
                        DispatcherPriority.Background);
                    break;
                case "信息员":
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, true);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "信息员"); },
                        DispatcherPriority.Background);
                    break;
                case "预警设备":
                    //暂无
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, false);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "无"); },
                        DispatcherPriority.Background);
                    break;
                case "学校":
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, true);
                    this.Dispatcher.InvokeAsync(() =>
                    {
                        m_Controller.LayerFilterControl(this, "学校");
                    }, DispatcherPriority.Background);

                    //// todo ArcGIS图层控制
                    //var gcelayerData = new GceLayer
                    //{
                    //    LayerName = "全国省界",
                    //    LayerVisiable = true
                    //};
                    //m_MessageAggregator.GetMessage<CMAGceLayerControlMessage>().Publish(gcelayerData);

                    break;
                case "医院":
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, true);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "医院"); },
                        DispatcherPriority.Background);
                    break;
                case "旅游景点":
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, true);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "景点"); },
                        DispatcherPriority.Background);
                    break;
                case "易燃易爆场所":
                    //暂无
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, false);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "无"); },
                        DispatcherPriority.Background);
                    break;
                case "山塘水库":
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, true);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "水库"); },
                        DispatcherPriority.Background);
                    break;
                case "中小河流隐患点":
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, true);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "中小河流"); },
                        DispatcherPriority.Background);
                    break;
                case "山洪沟隐患点":
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, true);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "山洪沟"); },
                        DispatcherPriority.Background);
                    break;
                case "地质灾害隐患点":
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, true);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "地灾隐患点"); },
                        DispatcherPriority.Background);
                    break;
                case "城镇易涝点":
                    //暂无
                    guids = new List<Guid>();
                    guids.AddRange(m_Controller.BasicPoiLayer);
                    m_Controller.LayerVisibilityControl(guids, false);
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "无"); },
                        DispatcherPriority.Background);
                    break;
            }
        }

        #endregion
    }
}