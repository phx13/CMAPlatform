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
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCP.Helper;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.CCP.Utilities;
using Digihail.CCPSOE.Models.Group;
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
    public partial class NewLayerControl : ChartViewBase, INotifyPropertyChanged
    {
        private CheckBox m_LastCheckBox;

        public NewLayerControl(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            Loaded += LayerControl_Loaded;
            m_Controller = Controllers[0] as NewLayerControlController;

            DataContext = m_Controller;

            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>()
                .Subscribe(ReceiveCMAMainPageSelectedTabMessage, ThreadOption.UIThread);
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

            base.Dispose();
        }

        #region 发送图层显隐消息

        /// <summary>
        ///     图层显隐控制 外部自定义控件控制图层或对象显隐
        /// </summary>
        /// <param name="obj">消息对象</param>
        private void LayerVisibilityControl(LayerModel item, string type = "check")
        {
            if (item == null)
            {
                return;
            }
            if (type == "radio")
            {
                var parent = FindParentCategory(m_Controller.LayerCollection, item);
                if (parent != null && !parent.IsObject)
                {
                    foreach (var submodel in parent.SubLayerPicCollection)
                    {
                        var data = new ObjectShowOrHideOneData
                        {
                            From = m_Controller.PageId,
                            IsShow = submodel.IsChecked == true,
                            DvpObjId = new List<Guid>(),
                            LayerGroupName = new List<string>(),
                            PageInstanceId = m_Controller.PageInstanceId
                        };

                        if (!submodel.IsObject && submodel.ObjectModelId != Guid.NewGuid())
                        {
                            data.DvpObjId.Add(submodel.ObjectModelId);
                        }
                        else
                        {
                            if (submodel.SubLayerPicCollection != null && submodel.SubLayerPicCollection.Count > 0)
                            {
                                var list = submodel.SubLayerPicCollection.Select(t => t.ObjectModelId).ToList();
                                list.RemoveAll(t => t == Guid.NewGuid());
                                foreach (var guid in list)
                                {
                                    data.DvpObjId.Add(guid);
                                }
                            }
                        }

                        m_MessageAggregator.GetMessage<ObjectShowOrHideOneMessage>().Publish(data);
                    }
                }
            }
            else
            {
                var data = new ObjectShowOrHideOneData
                {
                    From = m_Controller.PageId,
                    IsShow = item.IsChecked == true,
                    DvpObjId = new List<Guid>(),
                    LayerGroupName = new List<string>(),
                    PageInstanceId = m_Controller.PageInstanceId
                };

                if (!item.IsObject && item.ObjectModelId != Guid.NewGuid())
                {
                    data.DvpObjId.Add(item.ObjectModelId);
                }
                else
                {
                    if (item.SubLayerPicCollection != null && item.SubLayerPicCollection.Count > 0)
                    {
                        var list = item.SubLayerPicCollection.Select(t => t.ObjectModelId).ToList();
                        list.RemoveAll(t => t == Guid.NewGuid());
                        foreach (var guid in list)
                        {
                            data.DvpObjId.Add(guid);
                        }
                    }
                }

                m_MessageAggregator.GetMessage<ObjectShowOrHideOneMessage>().Publish(data);
            }
        }

        #endregion

        #region 接收联动消息

        /// <summary>
        ///     接收列表发来的联动消息
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveCMAMainPageSelectedTabMessage(CMAMainPageSelectedTabData obj)
        {
            if (obj != null && obj.InstanceGuid != m_Controller.PageInstanceId)
            {
                var guids = new List<Guid>();
                switch (obj.TabName)
                {
                    case "突发事件":
                        cbCheckedBox2.IsChecked = true;
                        //var layer = m_Controller.LayerCollection.FirstOrDefault(t => t.LayerName == "突发事件");
                        //if (layer != null)
                        //{
                        //    layer.IsChecked = true;
                        //    SetSubCategorysIsChecked(layer, layer.IsChecked == true);
                        //    LayerVisibilityControl(layer);
                        //}
                        break;
                    case "预警态势":
                        // cbCheckedBox3.IsChecked = true;
                        //layer = m_Controller.LayerCollection.FirstOrDefault(t => t.LayerName == "预警态势");
                        //if (layer != null)
                        //{
                        //    layer.IsChecked = true;
                        //    SetSubCategorysIsChecked(layer, layer.IsChecked == true);
                        //    LayerVisibilityControl(layer);
                        //}
                        break;
                    case "极端天气":
                        //layer = m_Controller.LayerCollection.FirstOrDefault(t => t.LayerName == "极值站");
                        //if (layer != null)
                        //{
                        //    layer.IsChecked = true;
                        //    SetSubCategorysIsChecked(layer, layer.IsChecked == true);
                        //    LayerVisibilityControl(layer);
                        //}
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly NewLayerControlController m_Controller;


        private Page3DModel m_Page3dModel;


        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        /// <summary>
        ///     图片名称类集合
        /// </summary>
        private ObservableCollection<SubLayerPicModdel> LayerPicCollection { get; set; }

        #endregion

        #region 设置节点树选中操作

        /// <summary>
        ///     查找勾选的对象
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="isContainRealTimeLayer">是否包含回放图层</param>
        /// <param name="dvpObjects"></param>
        public void FindCheckedObjects(ObservableCollection<LayerModel> categories, bool isContainRealTimeLayer,
            out List<Guid> dvpObjects)
        {
            dvpObjects = new List<Guid>();

            if (categories != null)
            {
                foreach (var baseCategory in categories)
                {
                    if (baseCategory.IsObject)
                    {
                        if (baseCategory.IsChecked == true)
                        {
                            dvpObjects.Add(baseCategory.ObjectModelId);
                        }
                    }

                    var dvpList = new List<Guid>();
                    FindCheckedObjects(baseCategory.SubLayerPicCollection, isContainRealTimeLayer, out dvpList);
                    dvpObjects.AddRange(dvpList);
                }
            }
        }


        public void FindUnCheckedObjects(ObservableCollection<LayerModel> categories, bool isContainRealTimeLayer,
            out List<Guid> dvpObjects)
        {
            dvpObjects = new List<Guid>();

            if (categories != null)
            {
                foreach (var baseCategory in categories)
                {
                    if (baseCategory.IsObject)
                    {
                        if (baseCategory.IsChecked != true)
                        {
                            dvpObjects.Add(baseCategory.ObjectModelId);
                        }
                    }

                    var dvpList = new List<Guid>();
                    FindUnCheckedObjects(baseCategory.SubLayerPicCollection, isContainRealTimeLayer, out dvpList);
                    dvpObjects.AddRange(dvpList);
                }
            }
        }


        [LogCCPError]
        private void cb2d3d_Click(object sender, RoutedEventArgs e)
        {
            var box = e.OriginalSource as FrameworkElement;
            if (box != null)
            {
                var item = box.DataContext as LayerModel;
                if (item != null)
                {
                    // 这里是新逻辑：

                    bool? ischecked = false;

                    if (box is CheckBox)
                    {
                        ischecked = (box as CheckBox).IsChecked;
                    }
                    else if (box is RadioButton)
                    {
                        ischecked = (box as RadioButton).IsChecked;
                    }

                    //if (item.LayerName == "突发事件")
                    //{
                    //    List<Guid> guids = new List<Guid>();
                    //    guids.AddRange(m_Controller.EmergengcyPointLayer);
                    //    guids.AddRange(m_Controller.EmergengcBubbleLayer);

                    //    m_Controller.LayerVisibilityControl(guids, ischecked == true);
                    //    return;
                    //}

                    if (item.LayerName == "基础信息")
                    {
                        var guids = new List<Guid>();
                        guids.AddRange(m_Controller.BasicPoiLayer);

                        m_Controller.LayerVisibilityControl(guids, ischecked == true);
                        return;
                    }


                    if (item.LayerName == "医院")
                    {
                        if (ischecked == true)
                        {
                            m_Controller.LayerFilterControl(this, "医院");
                        }
                        return;
                    }

                    if (item.LayerName == "旅游景点")
                    {
                        if (ischecked == true)
                        {
                            m_Controller.LayerFilterControl(this, "景点");
                        }
                        return;
                    }

                    if (item.LayerName == "信息员")
                    {
                        if (ischecked == true)
                        {
                            m_Controller.LayerFilterControl(this, "信息员");
                        }
                        return;
                    }


                    //...


                    //新逻辑结束


                    var coelayer = ImageHelper.GetString("s_COELayer");
                    var goelayer = ImageHelper.GetString("s_GOELayer");

                    if (item.CategoryName == coelayer || item.CategoryName == goelayer)
                    {
                        SetSubCategorysIsChecked(item, item.IsChecked == true);
                        SetParentCategorysIsChecked(item, item.IsChecked);

                        var dvpList = new List<Guid>();
                        FindCheckedObjects(m_Controller.LayerCollection, true, out dvpList);
                    }
                    else
                    {
                        SetSubCategorysIsChecked(item, item.IsChecked == true);
                        SetParentCategorysIsChecked(item, item.IsChecked);
                    }
                    var parent = FindParentCategory(m_Controller.LayerCollection, item);
                    if (parent.LayerName == "基础信息")
                    {
                        LayerVisibilityControl(item, "radio");
                    }
                    else
                    {
                        LayerVisibilityControl(item, "check");
                    }


                    var tfsjlayer = m_Controller.LayerCollection.FirstOrDefault(t => t.LayerName == "突发事件");
                    var fxtslayer = m_Controller.LayerCollection.FirstOrDefault(t => t.LayerName == "风险态势");
                    var jdtqlayer = m_Controller.LayerCollection.FirstOrDefault(t => t.LayerName == "极值站");

                    if (tfsjlayer != null && fxtslayer != null && jdtqlayer != null)
                    {
                        if (tfsjlayer.IsChecked == true)
                        {
                            var data = new CMAMainPageSelectedTabData
                            {
                                InstanceGuid = m_Controller.PageInstanceId,
                                TabName = "突发事件"
                            };
                            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>().Publish(data);
                        }
                        else if (fxtslayer.IsChecked == true)
                        {
                            var data = new CMAMainPageSelectedTabData
                            {
                                InstanceGuid = m_Controller.PageInstanceId,
                                TabName = "风险态势"
                            };
                            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>().Publish(data);
                        }
                        else if (jdtqlayer.IsChecked == true)
                        {
                            var data = new CMAMainPageSelectedTabData
                            {
                                InstanceGuid = m_Controller.PageInstanceId,
                                TabName = "极端天气"
                            };
                            m_MessageAggregator.GetMessage<CMAMainPageSelectedTabMessage>().Publish(data);
                        }
                    }
                }
            }
        }

        [LogCCPError]
        private LayerModel FindParentCategory(ObservableCollection<LayerModel> list, BaseCategory category)
        {
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.SubLayerPicCollection != null)
                    {
                        if (item.SubLayerPicCollection.Any(p => p.Key == category.Key))
                        {
                            return item;
                        }
                        var parent = FindParentCategory(item.SubLayerPicCollection, category);
                        if (parent != null)
                        {
                            return parent;
                        }
                    }
                }
            }
            return null;
        }

        [LogCCPError]
        private void SetParentCategorysIsChecked(LayerModel parentCategory, bool? isChecked)
        {
            var parent = FindParentCategory(m_Controller.LayerCollection, parentCategory);
            if (parent != null && !parent.IsObject)
            {
                var flag = false;
                foreach (var submodel in parent.SubLayerPicCollection)
                {
                    if (submodel.IsChecked != isChecked)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    parent.IsChecked = null;
                }
                else
                {
                    parent.IsChecked = isChecked;
                }

                //向上遍历
                SetParentCategorysIsChecked(parent, isChecked);
            }
        }

        /// <summary>
        ///     设置子级IsChecked状态
        /// </summary>
        /// <param name="parentCategory"></param>
        /// <param name="value"></param>
        [LogCCPError]
        private void SetSubCategorysIsChecked(LayerModel parentCategory, bool value)
        {
            parentCategory.IsChecked = value;
            if (parentCategory.SubLayerPicCollection == null)
            {
                return;
            }
            for (var i = 0; i < parentCategory.SubLayerPicCollection.Count; i++)
            {
                SetSubCategorysIsChecked(parentCategory.SubLayerPicCollection[i], value);
            }
        }

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
                    case "责任/警戒/监视区":
                        guids.AddRange(m_Controller.ResponsibilityLabelLayer);

                        break;
                    //case "预警态势":

                    //    break;
                    default:
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
                    Dispatcher.InvokeAsync(() => { m_Controller.LayerFilterControl(this, "学校"); },
                        DispatcherPriority.Background);
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
            }
        }

        #endregion
    }
}