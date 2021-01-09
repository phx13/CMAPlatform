using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CMAPlatform.Chart.DVM;
using CMAPlatform.Chart.View;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using Digihail.AVE.Controls.GIS3D.OSG.Engine;
using Digihail.AVE.Playback;
using Digihail.AVECLI.Controls.GIS3D.Core.EntityComponent.Transform;
using Digihail.AVECLI.Media3D.EntityFramework;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.BillboardStyles;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using OpenTK;

namespace CMAPlatform.Chart.Controller
{
    /// <summary>
    ///     风险态势标签控制器(落区标签)
    /// </summary>
    public class GIS3DRiskSituationLabelController : GIS3DControllerBase
    {
        #region Public Methods

        /// <summary>
        ///     添加到地图上
        /// </summary>
        /// <param name="control"></param>
        public void AddToMap(RiskSituationLabelControl control)
        {
            if (m_EngineContainer == null) return;

            var filePath = Path.Combine(Environment.CurrentDirectory, "CMARiskSituationLabel", control.DataKey + ".png");
            var filePath3d = string.Format(@".\CMARiskSituationLabel\{0}.png", control.DataKey);

            var result = SaveFrameworkElementToImage(control, filePath);
            if (!result) return;

            var entity = m_EngineContainer.GlobeWorld.World.AddEntity(DVM.Name + "_" + control.DataKey);
            entity.Visible = DVM.ShowLayer;

            var geo = new GeographicCoordinateTransform();
            geo.Longitude = double.Parse(control.Row[m_LonName].ToString());
            geo.Latitude = double.Parse(control.Row[m_LatName].ToString());
            entity.AddComponent(geo);

            var billboarStyle = new DefaultBillboardMaterialStyle(m_EngineContainer.GlobeWorld.World.ContentManager);
            billboarStyle.Texture = m_EngineContainer.GlobeWorld.World.ContentManager.LoadTexture(filePath3d);
            billboarStyle.ClipRange = Convert.ToSingle(DVM.ClipRange);
            billboarStyle.NearFactor = new Vector2(Convert.ToSingle(DVM.NearFactor));
            billboarStyle.FarFactor = new Vector2(Convert.ToSingle(DVM.FarFactor));
            billboarStyle.IsPerspective = false;

            var billboard = new BillboardComponent();
            billboard.MaterialStyle = billboarStyle;
            billboard.Width = 241;
            billboard.Height = 546;
            billboard.MaxVisibleDistance = DVM.MaxVisibleDistance;
            billboard.MinVisibleDistance = DVM.MinVisibleDistance;
            billboard.AutoHideByDistance = true;
            billboard.Pickable = false;
            entity.AddComponent(billboard);


            var clickentity =
                m_EngineContainer.GlobeWorld.World.AddEntity(DVM.Name + "clickentity_" +
                                                             control.Row[DVM.GroupKeyField.AsName]);
            clickentity.Visible = DVM.ShowLayer;


            var clickHeaderbillboard = new BillboardComponent();
            var billboarStyle1 = new DefaultBillboardMaterialStyle(m_EngineContainer.GlobeWorld.World.ContentManager);

            var emptylabel = @".\Icons\None_01.png";

            var geo1 = new GeographicCoordinateTransform();
            geo1.Longitude = double.Parse(control.Row[m_LonName].ToString());
            geo1.Latitude = double.Parse(control.Row[m_LatName].ToString());
            clickentity.AddComponent(geo1);


            billboarStyle1.Texture = m_EngineContainer.GlobeWorld.World.ContentManager.LoadTexture(emptylabel);
            billboarStyle1.ClipRange = Convert.ToSingle(DVM.ClipRange);
            billboarStyle1.NearFactor = new Vector2(Convert.ToSingle(DVM.NearFactor));
            billboarStyle1.FarFactor = new Vector2(Convert.ToSingle(DVM.FarFactor));
            billboarStyle1.IsPerspective = false;

            clickHeaderbillboard.MaterialStyle = billboarStyle1;
            clickHeaderbillboard.Width = 241;
            clickHeaderbillboard.Height = 99;
            clickHeaderbillboard.Offset = new Vector2d(0, 220);
            clickHeaderbillboard.MaxVisibleDistance = DVM.MaxVisibleDistance;
            clickHeaderbillboard.MinVisibleDistance = DVM.MinVisibleDistance;
            clickHeaderbillboard.AutoHideByDistance = true;
            clickHeaderbillboard.Pickable = true;
            clickentity.AddComponent(clickHeaderbillboard);


            m_DicEntities.AddOrUpdate(control.DataKey, entity, (key, value) => { return value; });

            m_DicClickedEntities.AddOrUpdate(control.DataKey, clickentity, (key, value) => { return value; });
        }

        #endregion

        #region Properties & Fields

        /// <summary>
        ///     当前控制器的dvm
        /// </summary>
        public GIS3DRiskSituationLabelDataViewModel DVM { get; private set; }

        /// <summary>
        ///     引擎容器
        /// </summary>
        private EngineContainer m_EngineContainer;

        /// <summary>
        ///     控件字典
        ///     key: groupKey
        /// </summary>
        private readonly ConcurrentDictionary<string, RiskSituationLabelControl> m_DicLabelControl =
            new ConcurrentDictionary<string, RiskSituationLabelControl>();

        /// <summary>
        ///     实体字典
        ///     key: groupKey
        /// </summary>
        private readonly ConcurrentDictionary<string, Entity3D> m_DicEntities =
            new ConcurrentDictionary<string, Entity3D>();

        /// <summary>
        ///     实体字典
        ///     key: groupKey
        /// </summary>
        private readonly ConcurrentDictionary<string, Entity3D> m_DicClickedEntities =
            new ConcurrentDictionary<string, Entity3D>();

        /// <summary>
        ///     经度列名称
        /// </summary>
        private string m_LonName;

        /// <summary>
        ///     纬度列名称
        /// </summary>
        private string m_LatName;

        /// <summary>
        ///     高度列名称
        /// </summary>
        private string m_AltName = string.Empty;

        /// <summary>
        ///     批号列名称
        /// </summary>
        private string m_GroupKeyName;

        /// <summary>
        ///     告警级别字段名称
        /// </summary>
        private string m_LevelName;

        /// <summary>
        ///     类型名称
        /// </summary>
        private string m_TypeNameName;

        /// <summary>
        ///     红色
        /// </summary>
        private string m_RedCountName;

        /// <summary>
        ///     黄色
        /// </summary>
        private string m_YellowCountName;

        /// <summary>
        ///     橙色
        /// </summary>
        private string m_OrangeCountName;

        /// <summary>
        ///     蓝色
        /// </summary>
        private string m_BlueCountName;

        #endregion

        #region Constructor

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public GIS3DRiskSituationLabelController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DVM = DataViewModel as GIS3DRiskSituationLabelDataViewModel;

            CreateDirectory();

            SetBaseValue();
            InitGlobe();

            EventDictionary = DataManager.Instance.EventDictiinaryType();
        }

        /// <summary>
        ///     基础字段设置
        /// </summary>
        private void SetBaseValue()
        {
            if (DVM == null || DVM.LonField == null || DVM.LatField == null || DVM.GroupKeyField == null)
            {
                return;
            }

            m_LonName = DVM.LonField.AsName;
            m_LatName = DVM.LatField.AsName;
            m_GroupKeyName = DVM.GroupKeyField.AsName;
            m_LevelName = DVM.LevelField.AsName;
            m_TypeNameName = DVM.TypeNameField.AsName;

            m_RedCountName = DVM.RedCountField.AsName;
            m_YellowCountName = DVM.YellowCountField.AsName;
            m_OrangeCountName = DVM.OrangeCountField.AsName;
            m_BlueCountName = DVM.BlueCountField.AsName;

            if (DVM.AltField != null)
            {
                m_AltName = DVM.AltField.AsName;
            }
            else
            {
                m_AltName = string.Empty;
            }
        }

        #endregion

        #region Override

        /// <summary>
        ///     接收数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            InitGlobe();

            if (m_EngineContainer == null) return;

            UpdateByDataTable(adt);
        }

        /// <summary>
        ///     刷新图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        /// <summary>
        ///     清空图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        /// <summary>
        ///     属性变更设置
        /// </summary>
        public override void RefreshStyle()
        {
            SetGlobeStyleConfig(DVM);
            SetGlobeStyle(DVM);

            SetShowLayer(DVM.ShowLayer);
        }

        /// <summary>
        ///     更新样式
        /// </summary>
        /// <param name="propertyDescription"></param>
        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
            if (propertyDescription.SubCategory == DescriptionEnum.地图样式)
            {
                SetGlobeStyleConfig(DVM);
                SetGlobeStyle(DVM);
            }
            else if (propertyDescription.Category == DescriptionEnum.样式设置
                     && propertyDescription.SubCategory == DescriptionEnum.其他)
            {
            }
            else
            {
                SetShowLayer(DVM.ShowLayer);
            }
        }

        /// <summary>
        ///     时间轴停止时
        /// </summary>
        public override void OnAVEPlayerStoped()
        {
            RemoveAll();
        }

        /// <summary>
        ///     更新dvm
        /// </summary>
        /// <param name="dvm"></param>
        public override void UpdateDataViewModel(ChartDataViewModel dvm)
        {
            DVM = dvm as GIS3DRiskSituationLabelDataViewModel;
            SetBaseValue();
            // 更新所有样式
            RefreshStyle();
        }

        /// <summary>
        ///     设置图层显隐
        /// </summary>
        /// <param name="showLayer"></param>
        public override void SetShowLayer(bool showLayer)
        {
            if (m_DicEntities == null) return;
            foreach (var entity in m_DicEntities.Values)
            {
                entity.Visible = showLayer;
            }

            if (m_DicClickedEntities == null) return;
            foreach (var entity in m_DicClickedEntities.Values)
            {
                entity.Visible = showLayer;
            }
        }

        /// <summary>
        ///     销毁
        /// </summary>
        public override void Dispose()
        {
            RemoveAll();
            base.Dispose();
        }

        #endregion

        #region Add Update Remove

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="adt"></param>
        private void UpdateByDataTable(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                return;
            }
            // 先全部删除，再重新添加
            RemoveAll();
            // 添加新对象
            foreach (var adtRow in adt.Rows)
            {
                var dataKey = adtRow[m_GroupKeyName].ToString();
                AddModel(adtRow, dataKey);
            }
            OnAddLabelEvent(m_DicLabelControl.Values.ToList());
        }

        ///// <summary>
        ///// 突发事件_灾害类型
        ///// </summary>
        public ObservableCollection<EventDictionary> EventDictionary
        {
            get
            {
                return new ObservableCollection<EventDictionary>();
            }
            set
            {
                
            }
        }

        /// <summary>
        ///     添加对象
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="dataKey"></param>
        private void AddModel(AdapterDataRow dataRow, string dataKey)
        {
            var ctlLabel = new RiskSituationLabelControl();
            ctlLabel.DataKey = dataKey;
            ctlLabel.Row = dataRow;
            ctlLabel.Type = dataRow[m_TypeNameName].ToString();
            var firstOrDefault = EventDictionary.FirstOrDefault(x => x.Name == ctlLabel.Type);
            if (firstOrDefault != null)
                ctlLabel.TypeName = firstOrDefault.Code;
            ctlLabel.Color = dataRow[m_LevelName].ToString();

            var redCount = double.Parse(dataRow[m_RedCountName].ToString());
            var yellowCount = double.Parse(dataRow[m_YellowCountName].ToString());
            var orangeCount = double.Parse(dataRow[m_OrangeCountName].ToString());
            var blueCount = double.Parse(dataRow[m_BlueCountName].ToString());

            var items = new ObservableCollection<RiskSituationLabelModel>();
            if (redCount != 0)
            {
                items.Add(new RiskSituationLabelModel {Name = "红色", Value = redCount, Foreground = "#FFC5364B"});
            }
            if (orangeCount != 0)
            {
                items.Add(new RiskSituationLabelModel {Name = "橙色", Value = orangeCount, Foreground = "#FFFF8000"});
            }
            if (yellowCount != 0)
            {
                items.Add(new RiskSituationLabelModel {Name = "黄色", Value = yellowCount, Foreground = "#FFECEC00"});
            }
            if (blueCount != 0)
            {
                items.Add(new RiskSituationLabelModel {Name = "蓝色", Value = blueCount, Foreground = "#FF13B6FE"});
            }
            double uiWidth = 240;
            if (items.Count == 2)
            {
                uiWidth = 120;
            }
            foreach (var item in items)
            {
                item.UiWidth = uiWidth;
            }
            ctlLabel.Items = items;

            m_DicLabelControl.AddOrUpdate(dataKey, ctlLabel, (key, value) => { return value; });
        }

        /// <summary>
        ///     清除图层上所有对象
        /// </summary>
        private void RemoveAll()
        {
            if (DVM == null) return;

            foreach (var key in m_DicLabelControl.Keys)
            {
                var ctlLabel = m_DicLabelControl[key];
                m_DicLabelControl.TryRemove(key, out ctlLabel);
            }
            // 清空图片目录
            ClearDirectory();
            // 清空控件
            OnDeleteLabelEvent();
            // 清空Entity
            foreach (var entity in m_DicEntities.Values)
            {
                m_EngineContainer.GlobeWorld.World.RemoveEntity(entity);
            }
            m_DicEntities.Clear();

            foreach (var entity in m_DicClickedEntities.Values)
            {
                m_EngineContainer.GlobeWorld.World.RemoveEntity(entity);
            }
            m_DicClickedEntities.Clear();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     将ui元素转换为图片
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool SaveFrameworkElementToImage(FrameworkElement ui, string path)
        {
            try
            {
                var ms = new FileStream(path, FileMode.Create);
                var bmp = new RenderTargetBitmap((int) ui.ActualWidth, (int) ui.ActualHeight, 96d, 96d,
                    PixelFormats.Pbgra32);
                bmp.Render(ui);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(ms);
                ms.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///     初始化地图控件相关内容
        /// </summary>
        private void InitGlobe()
        {
            if (EngineContainer == null) return;

            if (m_EngineContainer == null)
            {
                m_EngineContainer = (EngineContainer) EngineContainer;

                ClearDirectory();

                SetGlobeStyleConfig(DVM);
                SetGlobeStyle(DVM);
            }
        }

        /// <summary>
        ///     清空图片目录
        /// </summary>
        private void ClearDirectory()
        {
            var dirPath = Path.Combine(Environment.CurrentDirectory, "CMARiskSituationLabel");
            var dirInfo = new DirectoryInfo(dirPath);
            try
            {
                var fileinfo = dirInfo.GetFiles(); //返回目录中所有文件和子目录
                foreach (var i in fileinfo)
                {
                    File.Delete(i.FullName); //删除指定文件
                }
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        ///     创建图片目录
        /// </summary>
        private void CreateDirectory()
        {
            // 创建图片存储目录
            var dirPath = Path.Combine(Environment.CurrentDirectory, "CMARiskSituationLabel");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        #endregion

        #region Events

        /// <summary>
        ///     添加Label控件的事件
        /// </summary>
        public event AddLabelEventHandler AddLabelEvent;

        /// <summary>
        ///     激活添加Label控件
        /// </summary>
        /// <param name="labelControls"></param>
        private void OnAddLabelEvent(List<RiskSituationLabelControl> labelControls)
        {
            if (AddLabelEvent != null)
            {
                AddLabelEvent(labelControls);
            }
        }

        /// <summary>
        ///     删除Label控件的事件
        /// </summary>
        public event DeleteLabelEventHandler DeleteLabelEvent;

        /// <summary>
        ///     激活删除Label控件
        /// </summary>
        private void OnDeleteLabelEvent()
        {
            if (DeleteLabelEvent != null)
            {
                DeleteLabelEvent();
            }
        }

        #endregion
    }

    /// <summary>
    ///     删除Label控件委托
    /// </summary>
    public delegate void DeleteLabelEventHandler();

    /// <summary>
    ///     添加Label控件委托
    /// </summary>
    /// <param name="labelControls"></param>
    public delegate void AddLabelEventHandler(List<RiskSituationLabelControl> labelControls);
}