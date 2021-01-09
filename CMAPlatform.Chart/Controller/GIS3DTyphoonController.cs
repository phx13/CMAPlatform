using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Threading;
using CMAPlatform.Chart.DVM;
using CMAPlatform.Chart.Window;
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Controls.GIS3D.OSG.Engine;
using Digihail.AVE.Controls.GIS3D.OSG.Utils;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.AVE.Playback.Models;
using Digihail.AVECLI.Controls.GIS3D.Core;
using Digihail.AVECLI.Controls.GIS3D.Core.EntityComponent.Transform;
using Digihail.AVECLI.Controls.GIS3D.Core.EntityComponent.Visual;
using Digihail.AVECLI.Controls.GIS3D.Core.ImageOverlay;
using Digihail.AVECLI.Media3D.EntityFramework;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Transform;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.BillboardStyles;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.LineStyles;
using Digihail.CCP.Helper;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Charts.GIS3D.GIS3DControllers;
using Digihail.DAD3.Charts.GIS3D.Models;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Utils;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using Digihail.GOE.Models.Editor;
using Microsoft.Practices.Prism.Events;
using OpenTK;
using LayerModel = Digihail.AVE.Controls.GIS3D.OSG.Engine.Models.LayerModel;
using Point = System.Windows.Point;

namespace CMAPlatform.Chart.Controller
{
    /// <summary>
    ///     3D节点轨迹图控制器
    /// </summary>
    // Token: 0x02000011 RID: 17
    public class GIS3DPlaybackController : GIS3DControllerBase
    {
        /// <summary>
        ///     平移timer
        /// </summary>
        private readonly DispatcherTimer dt = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 0, 0, 20)
        };

        /// <summary>
        ///     24小时警戒线点集
        /// </summary>
        private readonly List<Point> m_24points;

        /// <summary>
        ///     48小时警戒线点集
        /// </summary>
        private readonly List<Point> m_48points;

        /// <summary>
        ///     key:name
        /// </summary>
        private readonly Dictionary<string, Entity3D> m_DicEntity = new Dictionary<string, Entity3D>();

        /// <summary>
        ///     图标材质
        /// </summary>
        private readonly Dictionary<string, DefaultStyleClass> m_IconNameToIconStyle =
            new Dictionary<string, DefaultStyleClass>();

        /// <summary>
        ///     图标材质（选中）
        /// </summary>
        private readonly Dictionary<string, DefaultStyleClass> m_IconNameToSelectedIconStyle =
            new Dictionary<string, DefaultStyleClass>();

        /// <summary>
        ///     图标材质（shinning）
        /// </summary>
        private readonly Dictionary<string, DefaultStyleClass> m_IconNameToShinningIconStyle =
            new Dictionary<string, DefaultStyleClass>();

        /// <summary>
        ///     进程间通信的消息聚合器对象
        /// </summary>
        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        /// <summary>
        ///     时间轴组件
        /// </summary>
        private readonly Playback m_Playback;

        /// <summary>
        ///     颜色字典
        /// </summary>
        private readonly Dictionary<string, Vector4> m_PolylineColorDir;

        /// <summary>
        ///     线实体集合
        /// </summary>
        private readonly List<Entity3D> m_PolylineEntityList = new List<Entity3D>();

        /// <summary>
        ///     随机数生成器
        /// </summary>
        private readonly Random m_Random = new Random();

        /// <summary>
        ///     材质的实体（shinning)
        /// </summary>
        private readonly Entity3D m_ShinningIconStyleEntity3D = null;

        /// <summary>
        ///     警戒线点集
        /// </summary>
        private readonly List<Entity3D> m_WarninglineList = new List<Entity3D>();

        /// <summary>
        ///     高度列名称
        /// </summary>
        private string m_AltName = string.Empty;

        /// <summary>
        ///     点缩放动画
        /// </summary>
        private PointAnimationController m_AnimationController;

        /// <summary>
        ///     动画定时器
        /// </summary>
        private DispatcherTimer m_AnimationTimer;

        /// <summary>
        ///     是否进行了清空对象操作
        /// </summary>
        private bool m_ClearObject;

        /// <summary>
        ///     数据集合
        /// </summary>
        private Dictionary<string, AdapterDataRow> m_Datas;

        /// <summary>
        ///     引擎容器
        /// </summary>
        protected EngineContainer m_EngineContainer;

        /// <summary>
        ///     批号列名称
        /// </summary>
        private string m_GroupKeyName;

        /// <summary>
        ///     材质的实体
        /// </summary>
        private Entity3D m_IconStyleEntity3D;

        /// <summary>
        ///     是否第一次加载
        /// </summary>
        private bool m_IsInit;

        /// <summary>
        ///     显示名称
        /// </summary>
        private string m_LabelName;

        private double m_Lat;

        /// <summary>
        ///     纬度列名称
        /// </summary>
        private string m_LatitudeName;

        /// <summary>
        ///     图例列名称
        /// </summary>
        private string m_LegendName;

        /// <summary>
        ///     台风风圈经度
        /// </summary>
        private double m_Lon;

        /// <summary>
        ///     经度列名称
        /// </summary>
        private string m_LongitudeName;

        /// <summary>
        ///     模型定义
        /// </summary>
        private GIS3DModelDefinition m_ModelDefinition;

        /// <summary>
        ///     父节点
        /// </summary>
        private Entity3D m_ParentEntity;

        /// <summary>
        ///     点材质
        /// </summary>
        private DefaultBillboardMaterialStyle m_PointStyle;

        /// <summary>
        ///     点材质存储实体
        /// </summary>
        private Entity3D m_PointStyleEntity3D;

        /// <summary>
        ///     时间轴步长
        /// </summary>
        private double m_PresentPlayStep;

        /// <summary>
        ///     材质的实体（选中）
        /// </summary>
        private Entity3D m_SelectedIconStyleEntity3D;

        /// <summary>
        ///     文字背景材质
        /// </summary>
        private DefaultBillboardMaterialStyle m_TextBackgroundStyle;

        /// <summary>
        ///     文字背景材质存储实体
        /// </summary>
        private Entity3D m_TextBackgroundStyleEntity3D;

        /// <summary>
        ///     尾迹材质
        /// </summary>
        private TrailMaterialStyle m_TrailStyle;

        /// <summary>
        ///     尾迹材质存储实体
        /// </summary>
        private Entity3D m_TrailStyleEntity3D;

        /// <summary>
        ///     台风实体类
        /// </summary>
        private Typhoon m_Typhoon = new Typhoon();

        /// <summary>
        ///     台风模型
        /// </summary>
        private Entity3D m_TyphoonModel;

        /// <summary>
        ///     台风图片图层
        /// </summary>
        private FileImageOverlay m_TyphoonOverlay;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public GIS3DPlaybackController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DVM = DataViewModel as GIS3DPlaybackDataViewModel;
            SetBaseValue();
            ReceiveMessages();
            m_PolylineColorDir = new Dictionary<string, Vector4>
            {
                {"TD", new Vector4(92f / 255f, 255f / 255f, 38f / 255f, 1)}, //热带低气压
                {"TS", new Vector4(0f / 255f, 64f / 255f, 255f / 255f, 1)}, //热带风暴
                {"STS", new Vector4(255f / 255f, 255f / 255f, 38f / 255f, 1)}, //强烈热带风暴
                {"TY", new Vector4(255f / 255f, 128f / 255f, 0f / 255f, 1)}, //台风
                {"STY", new Vector4(255f / 255f, 0f / 255f, 0f / 255f, 1)}, //强台风
                {"SuperTY", new Vector4(102f / 255f, 0f / 255f, 0f / 255f, 1)} //超强台风
            };
            m_24points = new List<Point>
            {
                new Point(127, 34),
                new Point(127, 22),
                new Point(119, 18),
                new Point(119, 11),
                new Point(113, 4.5),
                new Point(105, 0)
            };
            m_48points = new List<Point>
            {
                new Point(132, 34),
                new Point(132, 15),
                new Point(120, 0),
                new Point(105, 0)
            };
            m_PolylineEntityList = new List<Entity3D>();
            if (player != null) m_Playback = player as Playback;
            dt.Tick += Timer_Ticks;
        }

        /// <summary>
        ///     当前控制器的dvm
        /// </summary>
        public GIS3DPlaybackDataViewModel DVM { get; private set; }

        /// <summary>
        ///     要素点图层
        /// </summary>
        public LayerModel Layer { get; private set; }

        /// <summary>
        ///     当前管理的全部对象
        /// </summary>
        public ConcurrentDictionary<string, GIS3DPlaybackModel> CurrentModelDictionary =
            new ConcurrentDictionary<string, GIS3DPlaybackModel>();

        /// <summary>
        ///     接收数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            InitGlobe();
            InitOtherStyle();
            UpdateByDataTable(adt);
            if (DVM.IsAnimation) StartTimer();
        }

        /// <summary>
        ///     刷新图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void RefreshChart(ChartDataViewModel dvm)
        {
            var toRemoveModelKeyCollection = CurrentModelDictionary.Keys.ToList();
            RemoveModelByKeys(toRemoveModelKeyCollection);
            CurrentModelDictionary.Clear();
            SetBaseValue();
        }

        /// <summary>
        ///     清空图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void ClearChart(ChartDataViewModel dvm)
        {
            RemoveAll(false);
        }

        /// <summary>
        ///     属性变更设置
        /// </summary>
        public override void RefreshStyle()
        {
            SetGlobeStyleConfig(DVM);
            SetGlobeStyle(DVM);
            UpdateTextStyle();
            UpdateIconStyle();
            foreach (var model in CurrentModelDictionary.Values)
            {
                model.PostProcessThreshold = (float) DVM.PostProcessThreshold;
                model.UpdateHeightMap();
                model.UpdateGeographicCoordinateTransform();
                model.ImageWidth = m_IconNameToIconStyle[model.IconName].ImageWidth;
                model.ImageHeight = m_IconNameToIconStyle[model.IconName].ImageHeight;
                model.SelectedImageWidth = m_IconNameToSelectedIconStyle[model.IconName].ImageWidth;
                model.SelectedImageHeight = m_IconNameToSelectedIconStyle[model.IconName].ImageHeight;
                model.IsVisible = DVM.ShowLayer;
                model.ShowLabel = DVM.ShowLabel;
                model.UpdateBillboard();
                model.UpdateTrailComponent();
            }
        }

        /// <summary>
        ///     更新样式
        /// </summary>
        /// <param name="propertyDescription"></param>
        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
            if (propertyDescription.Category == "数据设置")
            {
                if (propertyDescription.SubCategory == "其他")
                {
                    if (propertyDescription.DisplayName == "启用贴地")
                        foreach (var model in CurrentModelDictionary.Values)
                            model.UpdateHeightMap();
                    else if (propertyDescription.DisplayName == "显示图层") SetShowLayer(DVM.ShowLayer);
                }
            }
            else if (propertyDescription.Category == "样式设置")
            {
                if (propertyDescription.SubCategory == "地图样式")
                {
                    SetGlobeStyleConfig(DVM);
                    SetGlobeStyle(DVM);
                }
                else if (propertyDescription.SubCategory == "位置偏移")
                {
                    foreach (var model in CurrentModelDictionary.Values) model.UpdateGeographicCoordinateTransform();
                }
                else if (propertyDescription.SubCategory == "常规动画")
                {
                    StopTimer();
                    if (DVM.IsAnimation)
                        StartTimer();
                    else
                        BeginAnimation();
                }
                else if (propertyDescription.SubCategory == "标识")
                {
                    if (propertyDescription.DisplayName == "类别图标")
                    {
                        UpdateIconStyle();
                        foreach (var model in CurrentModelDictionary.Values)
                        {
                            model.ImageWidth = m_IconNameToIconStyle[model.IconName].ImageWidth;
                            model.ImageHeight = m_IconNameToIconStyle[model.IconName].ImageHeight;
                            model.SelectedImageWidth = m_IconNameToSelectedIconStyle[model.IconName].ImageWidth;
                            model.SelectedImageHeight = m_IconNameToSelectedIconStyle[model.IconName].ImageHeight;
                            model.UpdateBillboard();
                        }
                    }
                    else if (propertyDescription.DisplayName == "后期特效过曝光度")
                    {
                        foreach (var model in CurrentModelDictionary.Values)
                            model.PostProcessThreshold = (float) DVM.PostProcessThreshold;
                    }
                    else if (propertyDescription.DisplayName == "重叠发光")
                    {
                        UpdateIconStyle();
                    }
                }
                else if (propertyDescription.SubCategory == "放缩显示")
                {
                    UpdateIconStyle();
                    UpdateTextStyle();
                }
                else
                {
                    foreach (var model in CurrentModelDictionary.Values)
                    {
                        model.ShowLabel = DVM.ShowLabel;
                        model.UpdateBillboard();
                        model.UpdateTrailComponent();
                    }
                }
            }
        }

        /// <summary>
        ///     时间轴停止时
        /// </summary>
        public override void OnAVEPlayerStoped()
        {
            RemoveAll(true);
        }

        /// <summary>
        ///     时间轴播放到结束
        /// </summary>
        public override void OnTimerEndStoped()
        {
            if (m_Playback != null && m_Playback.RepeatBehavior == Enums.RepeatBehaviorEnum.Forever)
            {
                var toRemoveModelKeyCollection = CurrentModelDictionary.Keys.ToList();
                RemoveModelByKeys(toRemoveModelKeyCollection);
            }
        }

        /// <summary>
        ///     设置搜索结果
        /// </summary>
        /// <param name="adt"></param>
        public override void SetSearchResult(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
                foreach (var model in CurrentModelDictionary.Values)
                    model.IsShinning = false;
            else
                foreach (var row in adt.Rows)
                {
                    var key = GetAdapterDataRowKey(row);
                    if (CurrentModelDictionary.ContainsKey(key)) CurrentModelDictionary[key].IsShinning = true;
                }
        }

        /// <summary>
        ///     更新dvm
        /// </summary>
        /// <param name="dvm"></param>
        public override void UpdateDataViewModel(ChartDataViewModel dvm)
        {
            DVM = dvm as GIS3DPlaybackDataViewModel;
            SetBaseValue();
            RefreshStyle();
        }

        /// <summary>
        ///     图表被联动时进行选中
        /// </summary>
        /// <param name="adt"></param>
        public override void SetSelectedItem(AdapterDataTable adt)
        {
            if (m_EngineContainer != null && DVM != null && adt != null && adt.Rows != null && adt.Rows.Count > 0)
            {
                var key = adt.Rows[0][DVM.GroupKeyField.AsName].ToString();
                if (CurrentModelDictionary.ContainsKey(key))
                {
                    CurrentModelDictionary[key].IsSelected = true;
                    m_EngineContainer.CurrentSelectedModel = CurrentModelDictionary[key];

                    var globalData = new GlobeTranslocationData();
                    globalData.PageId = CCPHelper.Instance.GetCurrentCCPPageModel().ID;
                    globalData.PageInstanceId = Guid.NewGuid();
                    globalData.IsFree = true;
                    globalData.CurLongitude = (m_EngineContainer.CurrentSelectedModel as GIS3DPlaybackModel).X;
                    globalData.CurLatitude = (m_EngineContainer.CurrentSelectedModel as GIS3DPlaybackModel).Y;
                    globalData.CurHeight = m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurHeight;
                    globalData.CurSurroundAngle =
                        m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurSurroundAngle;
                    globalData.CurPitch = m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurPitch;

                    m_MessageAggregator.GetMessage<GlobeTranslocationMessage>().Publish(globalData);
                }
            }
        }

        /// <summary>
        ///     图表取消联动时取消选中
        /// </summary>
        public override void ClearSelectedItem()
        {
            if (DVM != null && m_EngineContainer != null && m_EngineContainer.CurrentSelectedModel != null &&
                DVM.IsLinkage)
            {
                m_EngineContainer.CurrentSelectedModel.IsSelected = false;
                m_EngineContainer.CurrentSelectedModel = null;
            }
        }

        /// <summary>
        ///     界面进行更新
        /// </summary>
        /// <param name="adt"></param>
        private void UpdateByDataTable(AdapterDataTable adt)
        {
            var toRemoveModelKeyCollection = CurrentModelDictionary.Keys.ToList();
            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                RemoveModelByKeys(toRemoveModelKeyCollection);
            }
            else
            {
                foreach (var key in toRemoveModelKeyCollection)
                    if (CurrentModelDictionary.ContainsKey(key))
                    {
                        var model = CurrentModelDictionary[key];
                        if (model != null)
                            if (DataViewModel.DataTimeColumn != null)
                                RemoveModel(key, model);
                    }

                if (IsPlayerJump)
                {
                    RemoveModelByKeys(toRemoveModelKeyCollection);
                    IsPlayerJump = false;
                }

                m_Datas = new Dictionary<string, AdapterDataRow>();
                foreach (var row in adt.Rows)
                {
                    var key = GetAdapterDataRowKey(row);
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (m_Datas.ContainsKey(key))
                        {
                            if (DVM.DataTimeColumn != null)
                            {
                                var dt = Convert.ToDateTime(m_Datas[key][DVM.DataTimeColumn.AsName]);
                                var dt2 = Convert.ToDateTime(row[DVM.DataTimeColumn.AsName]);
                                var de = (long) (dt2 - dt).TotalSeconds;
                                if (de > 0L) m_Datas[key] = row;
                            }
                            else
                            {
                                m_Datas[key] = row;
                            }
                        }
                        else
                        {
                            m_Datas.Add(key, row);
                        }
                    }
                }

                m_PresentPlayStep = GetPlayStep();
                foreach (var pair in m_Datas)
                    if (CurrentModelDictionary.ContainsKey(pair.Key))
                    {
                        var model = CurrentModelDictionary[pair.Key];
                        if (DVM.DataTimeColumn != null &&
                            Math.Abs((Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds) >
                            m_PresentPlayStep * 100.0)
                        {
                            ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "实时模式下超时重新添加点",
                                string.Format("时间轴步长 {0} ， 时间轴时间与数据时间戳的差值为{1} , 发现时间为{2}", m_PresentPlayStep,
                                    Math.Abs((Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds),
                                    model.OccurDateTime.ToShortTimeString()));
                            toRemoveModelKeyCollection.Remove(pair.Key);
                            RemoveModel(pair.Key, model);
                            AddModel(pair.Key, pair.Value);
                        }
                        else
                        {
                            toRemoveModelKeyCollection.Remove(pair.Key);
                            SetPropertyValue(pair.Value, CurrentModelDictionary[pair.Key], true);
                        }
                    }
                    else
                    {
                        AddModel(pair.Key, pair.Value);
                    }

                RemoveModelByKeys(toRemoveModelKeyCollection);
                if (IsPlayerJump) IsPlayerJump = false;
                m_ClearObject = false;

                AddTyphoonPath(m_Typhoon);
            }
        }

        /// <summary>
        ///     获取数据唯一键值
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private string GetAdapterDataRowKey(AdapterDataRow dataRow)
        {
            string result;
            if (dataRow == null)
            {
                result = "";
            }
            else
            {
                var groupKeyValue = dataRow[m_GroupKeyName];
                if (groupKeyValue == null)
                    result = "";
                else
                    result = groupKeyValue.ToString();
            }

            return result;
        }

        /// <summary>
        ///     添加散点
        /// </summary>
        /// <param name="dataKey"></param>
        /// <param name="row"></param>
        private void AddModel(string dataKey, AdapterDataRow row)
        {
            var model = m_EngineContainer.CreateObjectModel(m_ModelDefinition) as GIS3DPlaybackModel;
            model.DataViewModel = DVM;
            if (!string.IsNullOrEmpty(m_LabelName)) model.DisplayName = row[m_LabelName].ToString();
            SetPropertyValue(row, model, false);
            Layer.AddObject(model);
            CurrentModelDictionary.AddOrUpdate(dataKey, model, (key, value) => value);
        }

        /// <summary>
        ///     设置图例图标
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private void SetLegendIcon(AdapterDataRow row, GIS3DPlaybackModel model)
        {
            var legendName = "";
            var iconName = "";
            if (!string.IsNullOrEmpty(m_LegendName))
            {
                if (row[m_LegendName] != null)
                {
                    legendName = row[m_LegendName].ToString().Trim();
                    iconName = legendName;
                }
            }
            else
            {
                legendName = DVM.GroupKeyField.ToString();
                iconName = "Default3DIcon";
            }

            model.LegendName = legendName;
            model.LegendIcon = GetIconUri(iconName);
            model.IconName = iconName;
        }

        /// <summary>
        ///     获取图标路径
        /// </summary>
        /// <param name="legendValue"></param>
        /// <returns></returns>
        private string GetIconPath(string legendValue)
        {
            return GetIconUri(legendValue).LocalPath;
        }

        /// <summary>
        ///     获取图标路径
        /// </summary>
        /// <param name="legendValue"></param>
        /// <returns></returns>
        private Uri GetIconUri(string legendValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(legendValue) && DVM.Icon != null && DVM.Icon.IconList != null)
                {
                    var itemModel =
                        DVM.Icon.IconList.FirstOrDefault(
                            item => item.IconName.ToLower() == legendValue.ToLower());
                    if (itemModel != null)
                    {
                        var fileUri = m_DataProxy.GetResourceFileUri(DataViewModel.ID, itemModel.IconPath.Substring(2));
                        return new Uri(Path.Combine(fileUri.AbsoluteUri, itemModel.IconPath.Substring(2)),
                            UriKind.Absolute);
                    }
                }
            }
            catch (Exception ex)
            {
                ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "GetIconUri",
                    string.Format("获取 {0} 图标路径出现问题", legendValue));
                ChartLogManager.WriteDadChartsError(ex);
                return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DIcon.png"));
            }

            return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DIcon.png"));
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="toRemoveModelKeyCollection"></param>
        private void RemoveModelByKeys(List<string> toRemoveModelKeyCollection)
        {
            foreach (var key in toRemoveModelKeyCollection)
                if (CurrentModelDictionary.ContainsKey(key))
                {
                    var model = CurrentModelDictionary[key];
                    if (model != null)
                    {
                        if (DataViewModel.DataTimeColumn != null)
                        {
                            if (model.OccurDateTime > Player.CurrentAbsoluteTime) RemoveModel(key, model);
                            if (Player.CurrentAbsoluteTime == Player.StopTime &&
                                m_Playback.RepeatBehavior == Enums.RepeatBehaviorEnum.Forever)
                                RemoveModel(key, model);
                            else if (DVM.EnableAutoRemoveTrack &&
                                     (Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds >
                                     DVM.AutoRemoveTrackDeferred)
                                RemoveModel(key, model);
                        }
                        else
                        {
                            RemoveModel(key, model);
                        }
                    }
                }

            if (IsPlayerJump) IsPlayerJump = false;
        }

        /// <summary>
        ///     删除散点
        /// </summary>
        /// <param name="dataKey"></param>
        /// <param name="model"></param>
        private void RemoveModel(string dataKey, GIS3DPlaybackModel model)
        {
            CurrentModelDictionary.TryRemove(dataKey, out model);
            if (m_EngineContainer.SurfaceView.Globe3DControler.World.CameraMode == GlobeWorld.eCameraMode.Follow &&
                m_EngineContainer.GlobeWorld.FollowGlobeCameraController.TargetEntity == model.Entity3D)
            {
                m_EngineContainer.GlobeWorld.FollowGlobeCameraController.TargetEntity = null;
                m_EngineContainer.GlobeWorld.CameraMode = GlobeWorld.eCameraMode.Free;
            }

            Layer.RemoveObject(model);
        }

        /// <summary>
        ///     设置轨迹点的属性
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private void SetPropertyValue(AdapterDataRow row, GIS3DPlaybackModel model, bool isUpdate)
        {
            model.Row = row;
            model.X = Convert.ToDouble(row[m_LongitudeName]);
            model.Y = Convert.ToDouble(row[m_LatitudeName]);
            if (!string.IsNullOrEmpty(m_LabelName)) model.DisplayName = row[m_LabelName].ToString();
            if (string.IsNullOrEmpty(m_AltName))
            {
                if (DVM.IsOverlapAltitude)
                    model.Z = DVM.AltitudeOffset;
                else
                    model.Z = GIS3DConfigurationValue.DefaultHeight;
            }
            else
            {
                model.Z = Convert.ToDouble(row[m_AltName]);
            }

            SetLegendIcon(row, model);
            InitIconStyle(model.IconName);
            if (!m_IsInit)
            {
                InitTextStyle();
                m_IsInit = true;
            }

            SetMaterialStyle(model);
            model.TrailColor = GetBrushHelper.GetColorString(DVM.TrailColor, model.LegendName);
            model.TextKey = DataViewModel.Name;
            if (Player != null && Player.State != Enums.PlayState.Stopped)
            {
                if ((model.OccurDateTime - DateTime.MinValue).TotalSeconds > 0.0)
                    model.Duration = (Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds;
                else
                    model.Duration = Player.Interval / 1000.0;
            }

            model.PostProcessThreshold = (float) DVM.PostProcessThreshold;
            if (!m_ClearObject)
                model.UpdateTail();
            else
                model.ClearTail();
            if (DVM.DataTimeColumn != null)
            {
                ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "m_DVM.DataTimeColumn不为空 ",
                    string.Format("散点时间列的AsName为 {0} ， 值为{1} , 转换成时间为{2}", DVM.DataTimeColumn.AsName,
                        row[DVM.DataTimeColumn.AsName], Convert.ToDateTime(row[DVM.DataTimeColumn.AsName])));
                model.OccurDateTime = Convert.ToDateTime(row[DVM.DataTimeColumn.AsName]);
                if (Player.CurrentAbsoluteTime == Player.StartTime) model.ClearTail();
            }
            else
            {
                ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "m_DVM.DataTimeColumn为空 ",
                    "m_DVM.DataTimeColumn为空");
                if (Player != null)
                {
                    ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "m_DVM.DataTimeColumn为空 ",
                        "model.OccurDateTime使用了时间轴当前时间");
                    model.OccurDateTime = Player.CurrentAbsoluteTime;
                }
            }

            if (Player != null)
            {
                model.SpeedRatio = Player.SpeedRatio;
                model.PlayStep = GetPlayStep();
            }

            model.IsVisible = DVM.ShowLayer;
            model.ShowLabel = DVM.ShowLabel;
        }

        /// <summary>
        ///     override 清除图层
        /// </summary>
        /// <param name="isPlayerStoped">true:点击时间轴停止</param>
        private void RemoveAll(bool isPlayerStoped)
        {
            if (isPlayerStoped)
            {
                base.OnAVEPlayerStoped();
                if (DVM.IsGetFirstDataImmediate)
                {
                    if (DVM.EnableTrackingPoint)
                        foreach (var key in CurrentModelDictionary.Keys)
                        {
                            var model = CurrentModelDictionary[key];
                            model.ClearTail();
                        }
                }
                else
                {
                    foreach (var key in CurrentModelDictionary.Keys)
                        if (CurrentModelDictionary.ContainsKey(key))
                        {
                            var model = CurrentModelDictionary[key];
                            RemoveModel(key, model);
                        }

                    CurrentModelDictionary.Clear();
                }
            }
            else
            {
                var toRemoveModelKeyCollection = CurrentModelDictionary.Keys.ToList();
                RemoveModelByKeys(toRemoveModelKeyCollection);
            }
        }

        /// <summary>
        ///     开启动画
        /// </summary>
        private void BeginAnimation()
        {
            StopAnimation();
            var currentTime = 0.0;
            foreach (var gis3dPointModel in CurrentModelDictionary.Values)
            {
                if (DVM.IsRandom) currentTime = m_Random.NextDouble() * DVM.RandomAddedDuration;
                if (DVM.AnimationType == 0)
                {
                    var animations = GetScaleAnimation(gis3dPointModel, currentTime);
                    m_AnimationController.AddAnimations(animations);
                }
                else if (DVM.AnimationType == (PointAnimationType) 3)
                {
                    var animations = new List<AnimationStatus>();
                    var flyAnimation =
                        GetFlyAnimation(gis3dPointModel, currentTime, -DVM.FlyDistance, DVM.AnimationType);
                    var opacityAnimation = GetOpacityAnimation(gis3dPointModel, currentTime);
                    animations.Add(flyAnimation);
                    animations.AddRange(opacityAnimation);
                    m_AnimationController.AddAnimations(animations);
                }
                else if (DVM.AnimationType == (PointAnimationType) 2)
                {
                    var animations = new List<AnimationStatus>();
                    var flyAnimation =
                        GetFlyAnimation(gis3dPointModel, currentTime, DVM.FlyDistance, DVM.AnimationType);
                    var opacityAnimation = GetOpacityAnimation(gis3dPointModel, currentTime);
                    animations.Add(flyAnimation);
                    animations.AddRange(opacityAnimation);
                    m_AnimationController.AddAnimations(animations);
                }
                else if (DVM.AnimationType == (PointAnimationType) 1)
                {
                    var animations = GetOpacityAnimation(gis3dPointModel, currentTime);
                    m_AnimationController.AddAnimations(animations);
                }
            }

            m_AnimationController.IsActive = true;
        }

        /// <summary>
        ///     获取缩放动画
        /// </summary>
        /// <param name="gis3dPointModel"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        private List<AnimationStatus> GetScaleAnimation(GIS3DPointModel gis3dPointModel, double currentTime)
        {
            var animations = new List<AnimationStatus>();
            var txt = gis3dPointModel.Entity3D.Children[2].FindComponent<BatchedTextComponent>();
            var animation0 = new AnimationStatus(0, gis3dPointModel.Entity3D, txt, DVM.ScaleMin, 1.0,
                DVM.AnimationDuration, -currentTime, currentTime);
            animations.Add(animation0);
            var billboard = gis3dPointModel.Entity3D.Children[0].FindComponent<BillboardComponent>();
            var animation = new AnimationStatus(0, gis3dPointModel.Entity3D, billboard, DVM.ScaleMin, 1.0,
                DVM.AnimationDuration, -currentTime, currentTime);
            animations.Add(animation);
            return animations;
        }

        /// <summary>
        ///     获取飞入动画
        /// </summary>
        /// <param name="gis3dPointModel"></param>
        /// <param name="currentTime"></param>
        /// <param name="from"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private AnimationStatus GetFlyAnimation(GIS3DPointModel gis3dPointModel, double currentTime, double from,
            PointAnimationType type)
        {
            var geo = gis3dPointModel.Entity3D.FindComponent<GeographicCoordinateTransform>();
            return new AnimationStatus(type, gis3dPointModel.Entity3D, geo, from, 0.0, DVM.AnimationDuration,
                -currentTime, currentTime);
        }

        /// <summary>
        ///     获取透明度变化动画
        /// </summary>
        /// <param name="gis3dPointModel"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        private List<AnimationStatus> GetOpacityAnimation(GIS3DPointModel gis3dPointModel, double currentTime)
        {
            var animations = new List<AnimationStatus>();
            BaseVisualComponent cpn0 = gis3dPointModel.Entity3D.Children[2].FindComponent<BatchedTextComponent>();
            var animation0 = new AnimationStatus((PointAnimationType) 1, gis3dPointModel.Entity3D, cpn0, 0.0,
                cpn0.Color.W, DVM.AnimationDuration, -currentTime, currentTime);
            animations.Add(animation0);
            var cpn = gis3dPointModel.Entity3D.Children[0].FindComponent<BaseVisualComponent>();
            var animation = new AnimationStatus((PointAnimationType) 1, gis3dPointModel.Entity3D, cpn, 0.0,
                cpn0.Color.W,
                DVM.AnimationDuration, -currentTime, currentTime);
            animations.Add(animation);
            return animations;
        }

        /// <summary>
        ///     停止动画
        /// </summary>
        private void StopAnimation()
        {
            if (m_AnimationController != null) m_AnimationController.StopAnimation();
        }

        /// <summary>
        ///     启动timer
        /// </summary>
        private void StartTimer()
        {
            if (m_AnimationTimer == null)
            {
                m_AnimationTimer = new DispatcherTimer();
                m_AnimationTimer.Interval = TimeSpan.FromSeconds(DVM.AnimationInterval);
                m_AnimationTimer.Tick += m_AnimationTimer_Tick;
                m_AnimationTimer.Start();
            }
        }

        /// <summary>
        ///     停止timer
        /// </summary>
        private void StopTimer()
        {
            if (m_AnimationTimer != null)
            {
                m_AnimationTimer.Tick -= m_AnimationTimer_Tick;
                m_AnimationTimer.Stop();
                m_AnimationTimer = null;
            }
        }

        /// <summary>
        ///     执行动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_AnimationTimer_Tick(object sender, EventArgs e)
        {
            BeginAnimation();
        }

        /// <summary>
        ///     设置基础值
        /// </summary>
        private void SetBaseValue()
        {
            if (DVM == null || DVM.LonField == null || DVM.LatField == null || DVM.GroupKeyField == null)
            {
                if (Layer != null) Layer.RemoveAll();
            }
            else
            {
                m_LongitudeName = DVM.LonField.AsName;
                m_LatitudeName = DVM.LatField.AsName;
                m_GroupKeyName = DVM.GroupKeyField.AsName;
                if (DVM.AltField != null)
                    m_AltName = DVM.AltField.AsName;
                else
                    m_AltName = string.Empty;
                if (DVM.LegendField != null)
                    m_LegendName = DVM.LegendField.AsName;
                else
                    m_LegendName = string.Empty;
                if (DVM.LabelField != null)
                    m_LabelName = DVM.LabelField.AsName;
                else
                    m_LabelName = string.Empty;
            }
        }

        /// <summary>
        ///     初始化地图控件相关内容
        /// </summary>
        private void InitGlobe()
        {
            if (EngineContainer != null)
            {
                if (DVM == null) DVM = DataViewModel as GIS3DPlaybackDataViewModel;
                if (m_EngineContainer == null) m_EngineContainer = (EngineContainer) EngineContainer;
                if (m_EngineContainer.ModelDefinitionList != null)
                    m_ModelDefinition = (from item in m_EngineContainer.ModelDefinitionList
                        where item.DisplayName == "节点轨迹模型"
                        select item).FirstOrDefault();
                if (Layer == null)
                {
                    Layer = new LayerModel();
                    Layer.Name = DataViewModel.Name;
                    m_EngineContainer.SurfaceModel.Layers.Add(Layer);
                    SetGlobeStyleConfig(DataViewModel as IGlobeStyle);
                    SetGlobeStyle(DataViewModel as IGlobeStyle);
                }

                if (m_ParentEntity == null)
                    m_ParentEntity = m_EngineContainer.GlobeWorld.World.AddEntity("Typhoon_" + DataViewModel.Name);
                m_TyphoonModel = m_EngineContainer.FindEntity("9b3dfa70-c99e-4224-a68a-ae4b3325978b");
            }
        }

        /// <summary>
        ///     初始化Other材质
        /// </summary>
        private void InitOtherStyle()
        {
            if (m_EngineContainer != null && m_EngineContainer.SurfaceView != null &&
                m_EngineContainer.SurfaceView.Globe3DControler != null)
            {
                if (null == m_TrailStyleEntity3D)
                {
                    m_TrailStyle = new TrailMaterialStyle(m_EngineContainer.GlobeWorld.World.ContentManager);
                    m_TrailStyleEntity3D =
                        m_EngineContainer.GlobeWorld.World.AddEntity("m_TrailStyleEntity3D" + DataViewModel.Name);
                    m_TrailStyleEntity3D.Visible = false;
                    var trailComponent = new TrailComponent();
                    trailComponent.Style = m_TrailStyle;
                    m_TrailStyleEntity3D.AddComponent(trailComponent);
                }

                if (null == m_TextBackgroundStyleEntity3D)
                {
                    m_TextBackgroundStyle =
                        new DefaultBillboardMaterialStyle(m_EngineContainer.GlobeWorld.World.ContentManager);
                    m_TextBackgroundStyle.InitializeWithMaterialFile(m_EngineContainer.GlobeWorld.World.ContentManager,
                        ".\\Resources\\System\\9GridsBillboardMaterial.xml");
                    m_TextBackgroundStyle.ClipRange = (float) DVM.MaxVisibleDistance;
                    m_TextBackgroundStyle.NearFactor = new Vector2((float) DVM.NearFactor);
                    m_TextBackgroundStyle.FarFactor = new Vector2((float) DVM.FarFactor);
                    m_TextBackgroundStyle.IsPerspective = false;
                    m_TextBackgroundStyle.TexturePath = ".\\Resources\\System\\WhiteTextBG.dds";
                    m_TextBackgroundStyleEntity3D =
                        m_EngineContainer.GlobeWorld.World.AddEntity("m_TextBackgroundStyleEntity3D" +
                                                                     DataViewModel.Name);
                    m_TextBackgroundStyleEntity3D.Visible = false;
                    var billboardComponent = new BillboardComponent();
                    billboardComponent.MaterialStyle = m_TextBackgroundStyle;
                    m_TextBackgroundStyleEntity3D.AddComponent(billboardComponent);
                }

                if (null == m_PointStyleEntity3D)
                {
                    m_PointStyle = new DefaultBillboardMaterialStyle(m_EngineContainer.GlobeWorld.World.ContentManager);
                    m_PointStyle.ClipRange = 5000f;
                    m_PointStyle.NearFactor = new Vector2(1f, 1f);
                    m_PointStyle.FarFactor = new Vector2(0.2f, 0.2f);
                    m_PointStyle.IsPerspective = false;
                    m_PointStyle.Texture =
                        m_EngineContainer.GlobeWorld.World.ContentManager.LoadTexture(
                            ".\\Resources\\Textures\\Point.png");
                    m_PointStyleEntity3D =
                        m_EngineContainer.GlobeWorld.World.AddEntity("m_PointStyleEntity3D" + DataViewModel.Name);
                    m_PointStyleEntity3D.Visible = false;
                    var billboardComponent = new BillboardComponent();
                    billboardComponent.MaterialStyle = m_PointStyle;
                    m_PointStyleEntity3D.AddComponent(billboardComponent);
                    var entity3D =
                        m_EngineContainer.GlobeWorld.World.AddEntity(string.Format("{0}_Animation", DVM.Name));
                    m_AnimationController = new PointAnimationController();
                    m_AnimationController.IsActive = false;
                    entity3D.AddComponent(m_AnimationController);
                }
            }
        }

        /// <summary>
        ///     初始化Icon材质
        /// </summary>
        private void InitIconStyle(string iconName)
        {
            if (!m_IconNameToIconStyle.ContainsKey(iconName))
            {
                var iconPath = GetIconPath(iconName);
                var meterialPath = string.Empty;
                if (DVM.IsOverlapShine)
                    meterialPath = ".\\Resources\\System\\TrailHeadMaterial.xml";
                else
                    meterialPath = ".\\Resources\\System\\DefaultBillboardMaterial.xml";
                var defaultStyleClass = InitIconDefaultStyleClass(iconPath, meterialPath);
                m_IconNameToIconStyle.Add(iconName, defaultStyleClass);
                CreateIconEntity(m_IconStyleEntity3D, "m_IconStyleEntity3D_" + iconName + DataViewModel.Name,
                    defaultStyleClass);
            }

            if (!m_IconNameToSelectedIconStyle.ContainsKey(iconName))
            {
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DBg.png");
                var meterialPath = ".\\Resources\\System\\TrailHeadMaterialWhite.xml";
                var defaultStyleClass = InitIconDefaultStyleClass(iconPath, meterialPath);
                m_IconNameToSelectedIconStyle.Add(iconName, defaultStyleClass);
                CreateIconEntity(m_SelectedIconStyleEntity3D,
                    "m_SelectedIconStyleEntity3D_" + iconName + DataViewModel.Name, defaultStyleClass);
            }

            if (!m_IconNameToShinningIconStyle.ContainsKey(iconName))
            {
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DShinningBg.png");
                var meterialPath = ".\\Resources\\System\\TrailHeadMaterialWhite.xml";
                var defaultStyleClass = InitIconDefaultStyleClass(iconPath, meterialPath);
                m_IconNameToShinningIconStyle.Add(iconName, defaultStyleClass);
                CreateIconEntity(m_ShinningIconStyleEntity3D,
                    "m_ShinningIconStyleEntity3D_" + iconName + DataViewModel.Name, defaultStyleClass);
            }
        }

        /// <summary>
        ///     初始化IconStyle
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="meterialPath"></param>
        /// <returns></returns>
        private DefaultStyleClass InitIconDefaultStyleClass(string iconPath, string meterialPath)
        {
            var defaultStyleClass = new DefaultStyleClass();
            defaultStyleClass.BillboardStyle = new DefaultBillboardMaterialStyle();
            defaultStyleClass.BillboardStyle.InitializeWithMaterialFile(
                m_EngineContainer.GlobeWorld.World.ContentManager, meterialPath);
            UpdateDefaultStyleClass(iconPath, defaultStyleClass);
            return defaultStyleClass;
        }

        /// <summary>
        ///     更新IconStyle
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="defaultStyleClass"></param>
        private void UpdateDefaultStyleClass(string iconPath, DefaultStyleClass defaultStyleClass)
        {
            defaultStyleClass.BillboardStyle.Texture =
                m_EngineContainer.GlobeWorld.World.ContentManager.LoadTexture(iconPath);
            defaultStyleClass.BillboardStyle.ClipRange = (float) DVM.MaxVisibleDistance;
            defaultStyleClass.BillboardStyle.NearFactor = new Vector2((float) DVM.NearFactor);
            defaultStyleClass.BillboardStyle.FarFactor = new Vector2((float) DVM.FarFactor);
            defaultStyleClass.BillboardStyle.IsPerspective = false;
            InitIconWidthHeight(iconPath, defaultStyleClass);
        }

        /// <summary>
        ///     初始化IconEntity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityName"></param>
        /// <param name="defaultStyleClass"></param>
        private void CreateIconEntity(Entity3D entity, string entityName, DefaultStyleClass defaultStyleClass)
        {
            entity = m_EngineContainer.GlobeWorld.World.AddEntity(entityName);
            entity.Visible = false;
            entity.AddComponent(new BillboardComponent
            {
                MaterialStyle = defaultStyleClass.BillboardStyle
            });
        }

        /// <summary>
        ///     更新Icon材质
        /// </summary>
        private void UpdateIconStyle()
        {
            string meterialPath;
            if (DVM.IsOverlapShine)
                meterialPath = ".\\Resources\\System\\TrailHeadMaterial.xml";
            else
                meterialPath = ".\\Resources\\System\\DefaultBillboardMaterial.xml";
            foreach (var item in m_IconNameToIconStyle)
            {
                m_IconNameToIconStyle[item.Key].BillboardStyle.InitializeWithMaterialFile(
                    m_EngineContainer.GlobeWorld.World.ContentManager, meterialPath);
                var iconPath = GetIconPath(item.Key);
                UpdateDefaultStyleClass(iconPath, item.Value);
            }

            foreach (var item in m_IconNameToSelectedIconStyle)
            {
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DBg.png");
                UpdateDefaultStyleClass(iconPath, item.Value);
            }

            foreach (var item in m_IconNameToShinningIconStyle)
            {
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DShinningBg.png");
                UpdateDefaultStyleClass(iconPath, item.Value);
            }
        }

        /// <summary>
        ///     初始化字体设置
        /// </summary>
        private void InitTextStyle()
        {
            if (m_EngineContainer != null)
            {
                if (DVM.LabelFontFamily == "微软雅黑") DVM.LabelFontFamily = "MSYaHei_GBK";
                m_EngineContainer.GlobeWorld.RegisterTextSystem(DataViewModel.Name,
                    ".\\Resources\\Fonts\\" + DVM.LabelFontFamily + ".fnt", GlobeWorld.SceneGroupNearMiddle,
                    new Vector2((float) DVM.NearFactor), new Vector2((float) DVM.FarFactor),
                    (float) DVM.MaxVisibleDistance, false, true, -6E+07f);
            }
        }

        /// <summary>
        ///     更新字体设置
        /// </summary>
        private void UpdateTextStyle()
        {
            if (m_EngineContainer != null)
                m_EngineContainer.GlobeWorld.ModifyTextSystem(DataViewModel.Name, new Vector2((float) DVM.NearFactor),
                    new Vector2((float) DVM.FarFactor), (float) DVM.MaxVisibleDistance);
        }

        /// <summary>
        ///     初始化图标大小
        /// </summary>
        private void InitIconWidthHeight(string iconPath, DefaultStyleClass defaultStyleClass)
        {
            if (iconPath.StartsWith("."))
                iconPath = AppDomain.CurrentDomain.BaseDirectory + iconPath.Substring(2, iconPath.Length - 2);
            if (!File.Exists(iconPath))
                iconPath = AppDomain.CurrentDomain.BaseDirectory +
                           GIS3DConfigurationValue.PointPath.Substring(2, GIS3DConfigurationValue.PointPath.Length - 2);
            using (var image = Image.FromFile(iconPath))
            {
                defaultStyleClass.ImageWidth = image.Width;
                defaultStyleClass.ImageHeight = image.Height;
            }
        }

        /// <summary>
        ///     设置材质
        /// </summary>
        private void SetMaterialStyle(GIS3DPlaybackModel model)
        {
            model.TrailStyle = m_TrailStyle;
            model.TextBackgroundStyle = m_TextBackgroundStyle;
            model.PointStyle = m_PointStyle;
            model.IconStyle = m_IconNameToIconStyle[model.IconName].BillboardStyle;
            model.ImageWidth = m_IconNameToIconStyle[model.IconName].ImageWidth;
            model.ImageHeight = m_IconNameToIconStyle[model.IconName].ImageHeight;
            model.SelectedIconStyle = m_IconNameToSelectedIconStyle[model.IconName].BillboardStyle;
            model.SelectedImageWidth = m_IconNameToSelectedIconStyle[model.IconName].ImageWidth;
            model.SelectedImageHeight = m_IconNameToSelectedIconStyle[model.IconName].ImageHeight;
            model.ShinningIconStyle = m_IconNameToShinningIconStyle[model.IconName].BillboardStyle;
            model.ShinningImageWidth = m_IconNameToShinningIconStyle[model.IconName].ImageWidth;
            model.ShinningImageHeight = m_IconNameToShinningIconStyle[model.IconName].ImageHeight;
            model.UpdateBillboard();
        }

        /// <summary>
        ///     地球范围改变事件
        /// </summary>
        /// <param name="minLon"></param>
        /// <param name="maxLon"></param>
        /// <param name="minLat"></param>
        /// <param name="maxLat"></param>
        public override void MapExtentChanged(double minLon, double maxLon, double minLat, double maxLat)
        {
            if (DVM != null && DVM.EnableSpatialQuery)
            {
                SetAdditionCondition(minLon, maxLon, minLat, maxLat);
                ReloadByAdditionCondition();
            }
        }

        /// <summary>
        ///     创建附加筛选条件
        /// </summary>
        /// <param name="minLon"></param>
        /// <param name="maxLon"></param>
        /// <param name="minLat"></param>
        /// <param name="maxLat"></param>
        /// <returns></returns>
        public override AdapterConditionModel CreateAdditionConditions(double minLon, double maxLon, double minLat,
            double maxLat)
        {
            var conditions = new AdapterConditionModel();
            var lonGreater = AdapterConditionModelHelper.GetCondition(DVM.LonField, minLon,
                ConditionJudgmentTypes.GreaterThaOrEqual);
            var lonLess = AdapterConditionModelHelper.GetCondition(DVM.LonField, maxLon,
                ConditionJudgmentTypes.LessThaOrEqual);
            var latGreater = AdapterConditionModelHelper.GetCondition(DVM.LatField, minLat,
                ConditionJudgmentTypes.GreaterThaOrEqual);
            var latLess = AdapterConditionModelHelper.GetCondition(DVM.LatField, maxLat,
                ConditionJudgmentTypes.LessThaOrEqual);
            conditions.CompoundConditions.Add(lonGreater);
            conditions.CompoundConditions.Add(lonLess);
            conditions.CompoundConditions.Add(latGreater);
            conditions.CompoundConditions.Add(latLess);
            return conditions;
        }

        /// <summary>
        ///     设置后期特效曝光亮度门限
        /// </summary>
        /// <param name="value"></param>
        private void SetProcessThreshold(double value)
        {
            m_EngineContainer.PostProcessThreshold = value;
        }

        /// <summary>
        ///     图层显隐控制
        /// </summary>
        /// <param name="showLayer"></param>
        public override void SetShowLayer(bool showLayer)
        {
            if (Layer != null)
            {
                Layer.IsVisible = showLayer;
                foreach (var pointModel in Layer.ObjectModels.OfType<GIS3DPlaybackModel>())
                    pointModel.IsVisible = showLayer;
                if (showLayer)
                    BeginAnimation();
                else
                    StopAnimation();
            }
        }

        /// <summary>
        ///     订阅消息
        /// </summary>
        private void ReceiveMessages()
        {
            m_MessageAggregator.GetMessage<ClearObjectMessage>().Subscribe(ReceivedClearObjectMessage);
            m_MessageAggregator.GetMessage<CMATyphoonMessage>()
                .Subscribe(ReceiveTyphoonMessage, ThreadOption.UIThread);
            m_MessageAggregator.GetMessage<StateChangedMessage>()
                .Subscribe(ReceiveStateChangedMessage, ThreadOption.UIThread);
            m_MessageAggregator.GetMessage<CMAGceLayerControlMessage>()
                .Subscribe(ReceiveGceLayerControlMessage, ThreadOption.UIThread);
            m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>()
                .Subscribe(ReceiveScutcheonSelectGroupMessage, ThreadOption.UIThread);
        }

        /// <summary>
        ///     接收预警消息体
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveScutcheonSelectGroupMessage(ScutcheonSelectGroupMessage obj)
        {
            if (obj.typeName != "11B01")
            {
                var data = new ObjectShowOrHideOneData
                {
                    From = Guid.Parse("8428502d-b1f4-4288-9113-16a170002728"),
                    IsShow = false,
                    DvpObjId = new List<Guid> {Guid.Parse("b7b545f5-a4eb-4de8-96a9-3c5252e3929b")},
                    LayerGroupName = new List<string>(),
                    PageInstanceId = Guid.NewGuid()
                };
                m_MessageAggregator.GetMessage<ObjectShowOrHideOneMessage>().Publish(data);
            }
        }

        /// <summary>
        ///     接收图层控制消息
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveGceLayerControlMessage(GceLayer obj)
        {
            if (obj.LayerName == "全球高程")
            {
                if (obj.LayerVisiable)
                {
                    var elevationLayerNames =  m_EngineContainer.SurfaceView.Globe3DControler.GetElevationLayerNames();
                    if (elevationLayerNames.Contains(obj.LayerName))
                    {
                        
                    }
                    else
                    {
                        m_EngineContainer.SurfaceView.Globe3DControler.AddElevationLayer(obj.LayerName,
                        "http://10.0.0.116:8082/WorldElevation_30M/tms.xml", Globe3DControler.MapTileDriver.TMS);
                    }
                }
                else
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveElevationLayer(obj.LayerName);
                }
            }
            else
            {
                m_EngineContainer.SurfaceView.Globe3DControler.SetImageLayerVisable(obj.LayerName, obj.LayerVisiable);
            }
        }

        /// <summary>
        ///     接收状态改变消息
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveStateChangedMessage(StateChangedModel obj)
        {
            if (obj.StateName != "首页默认") return;
            m_EngineContainer.GlobeWorld.ImageOverlays.ClearOverlays();
            m_TyphoonModel.Visible = false;
            Layer.IsVisible = false;
            if (m_PolylineEntityList.Count > 0)
            {
                foreach (var entity3D in m_PolylineEntityList)
                {
                    entity3D.Visible = false;
                    entity3D.FindComponent<OsgPolylineComponent>().Visible = false;
                    m_EngineContainer.GlobeWorld.World.RemoveEntity(entity3D);
                }

                m_PolylineEntityList.Clear();
            }

            if (m_WarninglineList.Count > 0)
            {
                foreach (var entity3D in m_WarninglineList)
                {
                    entity3D.Visible = false;
                    entity3D.FindComponent<OsgPolylineComponent>().Visible = false;
                    m_EngineContainer.GlobeWorld.World.RemoveEntity(entity3D);
                }

                m_WarninglineList.Clear();
            }

            if (m_DicEntity.Count > 0)
            {
                foreach (var key in m_DicEntity.Keys)
                {
                    var entity = m_DicEntity[key];
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(entity);
                }

                m_DicEntity.Clear();
            }

            var layer1 = m_EngineContainer.SurfaceModel.Layers.First(e => e.Name == "突发事件_小范围标签");
            layer1.RemoveAll();
            var layer2 = m_EngineContainer.SurfaceModel.Layers.First(e => e.Name == "突发事件气象站");
            layer2.RemoveAll();
        }

        /// <summary>
        ///     接收台风消息
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveTyphoonMessage(Typhoon obj)
        {
            m_Typhoon = obj;
            if (m_Typhoon.IsShort) AddTyphoonCircle(m_Typhoon);

            AddWarningLine();

            // 删除落区
            foreach (var key in m_DicEntity.Keys)
            {
                var entity = m_DicEntity[key];
                m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(entity);
            }

            m_DicEntity.Clear();

            if (obj.TyphoonRegionModels == null) obj.TyphoonRegionModels = new List<TyphoonRegionModel>();

            if (obj.TyphoonRegionModels.Count >= 5)
                for (var i = 0; i < 5; i++)
                    AddRegion(obj.TyphoonRegionModels[obj.TyphoonRegionModels.Count - i - 1].code,
                        obj.TyphoonRegionModels[obj.TyphoonRegionModels.Count - i - 1].severity);
            else
                foreach (var regionModel in obj.TyphoonRegionModels)
                    AddRegion(regionModel.code, regionModel.severity);
        }

        /// <summary>
        ///     控制台风视角移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Ticks(object sender, EventArgs e)
        {
            if (m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurLongitude == m_Lon &&
                m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurLatitude == m_Lat)
                dt.Stop();
            m_EngineContainer.PanTo(m_Lon, m_Lat,
                m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurHeight,
                m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurSurroundAngle,
                m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurPitch);
        }

        /// <summary>
        ///     绘制台风风圈
        /// </summary>
        /// <param name="obj"></param>
        private void AddTyphoonCircle(Typhoon obj)
        {
            if (obj.IsShort)
                lock (new object())
                {
                    if (Math.Abs(m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurLongitude - obj.CenterLon) >
                        0 &&
                        Math.Abs(m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurLatitude - obj.CenterLat) >
                        0)
                    {
                        m_EngineContainer.PanTo(obj.CenterLon, obj.CenterLat,
                            m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurHeight,
                            m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurSurroundAngle,
                            m_EngineContainer.GlobeWorld.FreeGlobeCameraController.CurPitch);
                        m_Lon = obj.CenterLon;
                        m_Lat = obj.CenterLat;
                        dt.Start();
                    }
                }

            var yellowArr = new int[4]
            {
                int.Parse(obj.YellowRadius1.ToString()), int.Parse(obj.YellowRadius2.ToString()),
                int.Parse(obj.YellowRadius3.ToString()), int.Parse(obj.YellowRadius4.ToString())
            };
            var redArr = new int[4]
            {
                int.Parse(obj.RedRadius1.ToString()), int.Parse(obj.RedRadius2.ToString()),
                int.Parse(obj.RedRadius3.ToString()), int.Parse(obj.RedRadius4.ToString())
            };
            var index = 0;
            for (var i = 0; i < yellowArr.Length; i++)
                if (yellowArr[i] > yellowArr[index])
                    index = i;
            var degree = (double) yellowArr[index] / 110;
            Form typhoon = new TyphoonForm(yellowArr, redArr);
            typhoon.Show();
            typhoon.Close();
            typhoon.Dispose();

            Dispatcher.Invoke(() =>
            {
                var m_Determination = @".\Data\Determination.png";
                var globeWorld = m_EngineContainer.GlobeWorld;
                if (m_TyphoonOverlay != null) globeWorld.ImageOverlays.RemoveOverlay(m_TyphoonOverlay);

                m_TyphoonOverlay = new FileImageOverlay(globeWorld);
                m_TyphoonOverlay.Placement = BaseImageOverlay.Placements.Ground;
                m_TyphoonOverlay.ImagePath = m_Determination;
                m_TyphoonOverlay.EnableSunLight = false;

                m_TyphoonOverlay.MinX = obj.CenterLon - degree;
                m_TyphoonOverlay.MinY = obj.CenterLat - degree;
                m_TyphoonOverlay.MaxX = obj.CenterLon + degree;
                m_TyphoonOverlay.MaxY = obj.CenterLat + degree;

                globeWorld.ImageOverlays.AddOverlay(m_TyphoonOverlay);

                if (obj.IsShort)
                {
                    // 放缩大小
                    var typhoonModel =
                        CCPHelper.Instance.GetCurrentPageGoeObjectModels().Find(x => x.DisplayName == "台风模型");

                    switch (obj.TyphoonIntensity)
                    {
                        case "TD":
                            (typhoonModel as BasicPoint).PointOffset.ZoomOffset = 3;
                            break;
                        case "TS":
                            (typhoonModel as BasicPoint).PointOffset.ZoomOffset = 4;
                            break;
                        case "STS":
                            (typhoonModel as BasicPoint).PointOffset.ZoomOffset = 5;
                            break;
                        case "TY":
                            (typhoonModel as BasicPoint).PointOffset.ZoomOffset = 6;
                            break;
                        case "STY":
                            (typhoonModel as BasicPoint).PointOffset.ZoomOffset = 7;
                            break;
                        case "SuperTY":
                            (typhoonModel as BasicPoint).PointOffset.ZoomOffset = 8;
                            break;
                    }

                    // 移动位置
                    (typhoonModel as BasicPoint).DVPPoint.X = obj.CenterLon;
                    (typhoonModel as BasicPoint).DVPPoint.Y = obj.CenterLat;
                    typhoonModel.IsObjectDisplay = true;
                }
            });
        }

        /// <summary>
        ///     添加台风路径
        /// </summary>
        /// <param name="obj"></param>
        private void AddTyphoonPath(Typhoon obj)
        {
            foreach (var entity3D in m_PolylineEntityList) m_EngineContainer.GlobeWorld.World.RemoveEntity(entity3D);
            m_PolylineEntityList.Clear();

            var points = new Dictionary<string, KeyValuePair<Point, Vector4>>();
            var sortString = new List<string>();

            foreach (var osgObjectModel in Layer.ObjectModels)
            {
                var playback = osgObjectModel as GIS3DPlaybackModel;
                if (playback != null)
                {
                    var pressure = playback.Row[DVM.ColorFields.First().AsName].ToString();
                    var color = m_PolylineColorDir[pressure]; //取到每一行的颜色

                    var billboard =
                        playback.Entity3D.Children.First(e => e.Name.EndsWith("Point"))
                            .FindComponent<BillboardComponent>();
                    billboard.Color = color; //更新散点颜色

                    var point = new Point(playback.X, playback.Y);

                    var typhoonKey = GetAdapterDataRowKey(playback.Row);
                    var tempTyphoonId = typhoonKey.Substring(0, 4);
                    if (points.ContainsKey(typhoonKey))
                    {
                        points[typhoonKey] = new KeyValuePair<Point, Vector4>(point, color);
                    }
                    else
                    {
                        points.Add(typhoonKey, new KeyValuePair<Point, Vector4>(point, color));
                        sortString.Add(typhoonKey);
                    }
                }
            }

            var groupBy = sortString.GroupBy(e => e.Substring(0, 4));
            foreach (var grouping in groupBy)
            {
                var list = grouping.ToList();
                list.Sort();
                var tempPoints = new Dictionary<string, KeyValuePair<Point, Vector4>>();
                for (var i = 0; i < list.Count; i++) tempPoints.Add(list[i], points[list[i]]);

                for (var i = 0; i < tempPoints.Count - 1; i++)
                {
                    var entityName = Guid.NewGuid().ToString();
                    var entity = CreatePolylineEntity(entityName);
                    var polyline = entity.FindComponent<OsgPolylineComponent>();
                    polyline.LineColor = tempPoints.ElementAt(i).Value.Value;
                    polyline.LineWidth = 5;
                    polyline.WidthUnit = OsgPolylineComponent.WidthUnitEnum.Pixels;
                    polyline.StipplePattern = 0xffff;
                    polyline.AddPoint(new Vector3d(tempPoints.ElementAt(i).Value.Key.X,
                        tempPoints.ElementAt(i).Value.Key.Y, 0));
                    polyline.AddPoint(new Vector3d(tempPoints.ElementAt(i + 1).Value.Key.X,
                        tempPoints.ElementAt(i + 1).Value.Key.Y, 0));
                }
            }

            if (!obj.IsShort)
            {
                var data = new ObjectShowOrHideOneData
                {
                    From = Guid.Parse("8428502d-b1f4-4288-9113-16a170002728"),
                    IsShow = false,
                    DvpObjId = new List<Guid> {Guid.Parse("b7b545f5-a4eb-4de8-96a9-3c5252e3929b")},
                    LayerGroupName = new List<string>(),
                    PageInstanceId = Guid.NewGuid()
                };

                m_MessageAggregator.GetMessage<ObjectShowOrHideOneMessage>().Publish(data);
            }
        }

        /// <summary>
        ///     创建台风路径实体
        /// </summary>
        /// <returns></returns>
        private Entity3D CreatePolylineEntity(string entityName)
        {
            var Entity = m_EngineContainer.GlobeWorld.World.FindEntity(entityName);
            if (Entity == null)
            {
                var entity = m_EngineContainer.GlobeWorld.World.AddEntity(entityName);
                entity.AddComponent(new SRTTransformComponent());
                entity.AddComponent(new OsgPolylineComponent());
                m_PolylineEntityList.Add(entity);
                Entity = entity;
            }

            return Entity;
        }

        /// <summary>
        ///     创建警戒线实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        private Entity3D CreateWarningline(string entityName)
        {
            var Entity = m_EngineContainer.GlobeWorld.World.FindEntity(entityName);
            if (Entity == null)
            {
                var entity = m_EngineContainer.GlobeWorld.World.AddEntity(entityName);
                entity.AddComponent(new SRTTransformComponent());
                entity.AddComponent(new OsgPolylineComponent());
                m_WarninglineList.Add(entity);
                Entity = entity;
            }

            return Entity;
        }

        /// <summary>
        ///     添加警戒线
        /// </summary>
        private void AddWarningLine()
        {
            foreach (var entity3D in m_WarninglineList) m_EngineContainer.GlobeWorld.World.RemoveEntity(entity3D);
            m_WarninglineList.Clear();

            for (var i = 0; i < m_24points.Count - 1; i++)
            {
                var entityName = "Warningline24" + i;
                var entity = CreateWarningline(entityName);
                var polyline = entity.FindComponent<OsgPolylineComponent>();
                polyline.LineColor = new Vector4(255f / 255f, 255f / 255f, 38f / 255f, 1);
                polyline.LineWidth = 5;
                polyline.WidthUnit = OsgPolylineComponent.WidthUnitEnum.Pixels;
                polyline.StipplePattern = 0xffff;
                polyline.AddPoint(new Vector3d(m_24points[i].X, m_24points[i].Y, 0));
                polyline.AddPoint(new Vector3d(m_24points[i + 1].X, m_24points[i + 1].Y, 0));
            }

            for (var i = 0; i < m_48points.Count - 1; i++)
            {
                var entityName = "Warningline48" + i;
                var entity = CreateWarningline(entityName);
                var polyline = entity.FindComponent<OsgPolylineComponent>();
                polyline.LineColor = new Vector4(0f / 255f, 64f / 255f, 255f / 255f, 1);
                polyline.LineWidth = 5;
                polyline.WidthUnit = OsgPolylineComponent.WidthUnitEnum.Pixels;
                polyline.StipplePattern = 0xf0f0;
                polyline.StippleFactor = 4;
                polyline.AddPoint(new Vector3d(m_48points[i].X, m_48points[i].Y, 0));
                polyline.AddPoint(new Vector3d(m_48points[i + 1].X, m_48points[i + 1].Y, 0));
            }

            var globeWorld = m_EngineContainer.GlobeWorld;

            var m_Determination24 = @".\Data\24小时警戒线.png";
            var imageOverlay24 = new FileImageOverlay(globeWorld);
            imageOverlay24.Placement = BaseImageOverlay.Placements.Ground;
            imageOverlay24.ImagePath = m_Determination24;
            imageOverlay24.EnableSunLight = false;

            imageOverlay24.MinX = 126.85 - 1;
            imageOverlay24.MinY = 34 - 6;
            imageOverlay24.MaxX = 126.85 + 1;
            imageOverlay24.MaxY = 34 + 6;

            var m_Determination48 = @".\Data\48小时警戒线.png";
            var imageOverlay48 = new FileImageOverlay(globeWorld);
            imageOverlay48.Placement = BaseImageOverlay.Placements.Ground;
            imageOverlay48.ImagePath = m_Determination48;
            imageOverlay48.EnableSunLight = false;

            imageOverlay48.MinX = 131.85 - 1;
            imageOverlay48.MinY = 34 - 6;
            imageOverlay48.MaxX = 131.85 + 1;
            imageOverlay48.MaxY = 34 + 6;

            globeWorld.ImageOverlays.AddOverlay(imageOverlay24);
            globeWorld.ImageOverlays.AddOverlay(imageOverlay48);
        }

        /// <summary>
        ///     接收清空对象消息
        /// </summary>
        /// <param name="clearModel"></param>
        private void ReceivedClearObjectMessage(ClearObjectModel clearModel)
        {
            if (DVM != null && !string.IsNullOrEmpty(DVM.Name))
                if (clearModel.DVMIdList.Contains(DVM.ID.ToString()))
                {
                    m_ClearObject = true;
                    if (DVM.EnableTrackingPoint)
                    {
                        if (clearModel.ClearTrail)
                            foreach (var key in CurrentModelDictionary.Keys)
                            {
                                var model = CurrentModelDictionary[key];
                                model.ClearTail();
                            }

                        if (clearModel.ClearModel)
                        {
                            var toRemoveModelKeyCollection = CurrentModelDictionary.Keys.ToList();
                            RemoveModelByKeys(toRemoveModelKeyCollection);
                        }
                    }
                }
        }

        /// <summary>
        ///     销毁
        /// </summary>
        public override void Dispose()
        {
            StopAnimation();
            StopTimer();
            if (m_EngineContainer != null && m_EngineContainer.SurfaceView != null &&
                m_EngineContainer.SurfaceView.Globe3DControler != null)
            {
                if (Layer != null) m_EngineContainer.SurfaceModel.Layers.Remove(Layer);
                if (m_PointStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_PointStyleEntity3D);
                    m_PointStyleEntity3D = null;
                }

                if (m_TrailStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_TrailStyleEntity3D);
                    m_TrailStyleEntity3D = null;
                }

                if (m_TextBackgroundStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_TextBackgroundStyleEntity3D);
                    m_TextBackgroundStyleEntity3D = null;
                }

                if (m_IconStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_IconStyleEntity3D);
                    m_IconStyleEntity3D = null;
                }

                if (m_SelectedIconStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_SelectedIconStyleEntity3D);
                    m_SelectedIconStyleEntity3D = null;
                }

                if (m_ParentEntity != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_ParentEntity);
                    m_ParentEntity = null;
                }
            }

            if (CurrentModelDictionary != null) CurrentModelDictionary.Clear();

            if (m_DicEntity != null) m_DicEntity.Clear();
            base.Dispose();
        }

        /// <summary>
        ///     添加落区
        /// </summary>
        /// <param name="areaCode">行政区划码</param>
        private void AddRegion(string areaCode, string color)
        {
            var guid = Guid.NewGuid().ToString();
            var polygonEntity = m_EngineContainer.GlobeWorld.World.AddEntity(guid);
            polygonEntity.Parent = m_ParentEntity;

            m_DicEntity.Add(guid, polygonEntity);

            var path = Path.Combine(Environment.CurrentDirectory, "CMARiskSituation", areaCode);
            if (File.Exists(path))
            {
                var polygonString = File.ReadAllText(path);

                ConvertStringToMultiPolygon(polygonEntity, guid, polygonString, color);
            }
        }

        /// <summary>
        ///     把Polygon字符串转换为Polygon对象
        /// </summary>
        /// <param name="polygonEntity"></param>
        /// <param name="dataKey"></param>
        /// <param name="polygonString"></param>
        /// <param name="fillColor"></param>
        private void ConvertStringToMultiPolygon(Entity3D polygonEntity, string dataKey, string polygonString,
            string fillColor)
        {
            if (polygonString.Length <= 14) return;
            if (!polygonString.StartsWith("MULTIPOLYGON")) return;

            polygonString = polygonString.Substring(13, polygonString.Length - 14);
            var multiPolygons = polygonString.Split(new[] {")),(("}, StringSplitOptions.RemoveEmptyEntries).ToArray();
            for (var i = 0; i < multiPolygons.Length; i++)
            {
                var polygonsString = multiPolygons[i]; // 多个多边形
                if (i == 0) polygonsString = polygonsString.Substring(2, polygonsString.Length - 2);
                if (i == multiPolygons.Length - 1)
                    polygonsString = polygonsString.Substring(0, polygonsString.Length - 2);

                var polygons = polygonsString.Split(new[] {"),("}, StringSplitOptions.RemoveEmptyEntries).ToArray();
                for (var j = 0; j < polygons.Length; j++)
                {
                    var singlePolygon = polygons[j];
                    ChartLogManager.PrintDebugMesage("落区控件", "ConvertStringToMultiPolygon方法",
                        string.Format("多边形点集:{0}", singlePolygon));

                    // 创建子entity
                    var entity =
                        m_EngineContainer.GlobeWorld.World.AddEntity(string.Format("{0}_{1}_{2}", dataKey, i, j));
                    entity.Parent = polygonEntity;
                    // 添加 PolygonComponent
                    var osgPolygonCpn = new OsgPolygonComponent();
                    var polygonColor = "#00FFFFFF";
                    if (fillColor == "orange")
                        //30_4C   70_B2   50_80  40_66  60_3C  65_41
                        polygonColor = "#B2FF8F00";
                    if (fillColor == "red") polygonColor = "#B2FF0000";

                    osgPolygonCpn.FillColor = ColorHelper.GetOpenTKColor4FromHexString(polygonColor);
                    entity.AddComponent(osgPolygonCpn);

                    var points = singlePolygon.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).ToArray();

                    //消除点逻辑
                    for (var k = 0; k < points.Length; k++)
                    {
                        var lla = points[k];
                        var point = lla.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).ToArray();
                        var lon = double.Parse(point[0]);
                        var lat = double.Parse(point[1]);
                        osgPolygonCpn.AddPoint(new Vector3d(lon, lat, 0));
                    }
                }
            }
        }
    }

    public class GIS3DConfigurationValue
    {
        /// <summary>
        ///     默认模型标牌
        /// </summary>
        public static string PointPath = ".\\Resources\\Textures\\None.png";

        /// <summary>
        ///     默认背景模型标牌
        /// </summary>
        public static string BgIconPath = ".\\Icons\\Bg.png";

        /// <summary>
        ///     默认高度
        /// </summary>
        public static double DefaultHeight = 0.0;
    }
}