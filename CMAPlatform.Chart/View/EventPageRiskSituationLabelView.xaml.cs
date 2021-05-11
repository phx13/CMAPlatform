using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using CMAPlatform.Chart.Controller;
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Controls.GIS3D.OSG.Engine;
using Digihail.AVE.Controls.Infrastructure;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback.Models;
using Digihail.AVECLI.Controls.GIS3D.Core;
using Digihail.AVECLI.Controls.GIS3D.Core.EntityComponent.Transform;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS3D;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Charts.GIS3D.Utils;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Charts.Utils;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using Digihail.DAD3.Models.Utils;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     风险态势标牌视图(落区标牌)
    /// </summary>
    public partial class EventPageRiskSituationLabelView : ChartViewBase
    {
        #region Private Methods

        /// <summary>
        ///     更新样式设置中所有
        /// </summary>
        private void UpdateAllStyle()
        {
            foreach (GIS3DControllerBase controller in Controllers)
            {
                controller.RefreshStyle();
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        ///     释放资源
        /// </summary>
        public override void Dispose()
        {
            if (Controllers != null)
            {
                foreach (var controller in Controllers)
                {
                    var eventPageRiskSituationController = controller as GIS3DEventPageRiskSituationLabelController;
                    m_EventPageRiskSituationController = eventPageRiskSituationController;
                    if (eventPageRiskSituationController != null)
                    {
                        eventPageRiskSituationController.AddEventPageLabelEvent -= riskSituationController_AddLabelEvent;
                        eventPageRiskSituationController.DeleteEventPageLabelEvent -=
                            riskSituationController_DeleteLabelEvent;
                    }

                    controller.Dispose();
                }
                Controllers = null;
            }
            if (EngineContainer != null)
            {
                if (EngineContainer.SurfaceView != null)
                {
                    EngineContainer.SurfaceView.PropertyChanged -= SurfaceView_PropertyChanged;
                }
                EngineContainer.Dispose();
                EngineContainer = null;
            }
            if (m_GIS3DGlobe != null)
            {
                if (m_GIS3DGlobe.EngineContainer != null)
                {
                    m_GIS3DGlobe.EngineContainer.Dispose();
                }
                m_GIS3DGlobe = null;
            }
            eventPageContentControl.Content = null;

            riskSituationController_DeleteLabelEvent();

            base.Dispose();
        }

        #endregion

        #region Propertie & Fields

        /// <summary>
        ///     引擎
        /// </summary>
        public EngineContainer EngineContainer { get; private set; }

        /// <summary>
        ///     三维球
        /// </summary>
        private GIS3DGlobe m_GIS3DGlobe;

        /// <summary>
        ///     风险态势标牌控制器(落区标牌)
        /// </summary>
        private GIS3DEventPageRiskSituationLabelController m_EventPageRiskSituationController;

        /// <summary>
        ///     消息聚合器
        /// </summary>
        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        #endregion

        #region Constructor

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="model"></param>
        public EventPageRiskSituationLabelView(ChartViewBaseModel model) : base(model)
        {
            InitializeComponent();

            if (model.Engine != null)
            {
                ImportEngineContainer(model.Engine);
            }
            else
            {
                m_GIS3DGlobe = new GIS3DGlobe();
                eventPageContentControl.Content = m_GIS3DGlobe;
                m_GIS3DGlobe.EngineInitialized += GIS3DGlobe_OnEngineInitialized;
            }
            m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>()
                .Subscribe(ReceiveCMAScutcheonSelectGroupMessage);
            //EventDictionary = DataManager.Instance.EventDictiinaryType();
        }

        private ObservableCollection<EventDictionary> m_EventDictionary = new ObservableCollection<EventDictionary>();

        /// <summary>
        ///     突发事件_灾害类型
        /// </summary>
        public ObservableCollection<EventDictionary> EventDictionary
        {
            get { return m_EventDictionary; }
            set
            {
                m_EventDictionary = value;
                OnPropertyChanged("EventDictionary");
            }
        }

        private void ReceiveCMAScutcheonSelectGroupMessage(ScutcheonSelectGroupMessage obj)
        {
            if (obj != null)
            {
                var id = obj.id;
                var lon = obj.lon;
                var lat = obj.lat;
                var typeName = obj.typeName;
                var colour = obj.colour;
                m_EventPageRiskSituationController.UpdateByDataTable(id, lon, lat, typeName, colour);
            }
        }

        /// <summary>
        ///     引擎初始化结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GIS3DGlobe_OnEngineInitialized(object sender, EventArgs e)
        {
            EngineContainer = m_GIS3DGlobe.EngineContainer;
            foreach (GIS3DControllerBase controller in Controllers)
            {
                controller.EngineContainer = EngineContainer;
                controller.UseMapConfig = Model.UseMapConfig;

                var eventPageRiskSituationController = controller as GIS3DEventPageRiskSituationLabelController;
                if (eventPageRiskSituationController != null)
                {
                    m_EventPageRiskSituationController = eventPageRiskSituationController;
                    eventPageRiskSituationController.AddEventPageLabelEvent += riskSituationController_AddLabelEvent;
                    eventPageRiskSituationController.DeleteEventPageLabelEvent +=
                        riskSituationController_DeleteLabelEvent;
                }
            }


            InitBaseSetting();

            OnDadChartLoaded();
        }


        /// <summary>
        ///     导入enginecontainer时
        /// </summary>
        /// <param name="container"></param>
        private void ImportEngineContainer(IEngineContainer container)
        {
            if (container is EngineContainer)
            {
                EngineContainer = (EngineContainer) container;

                InitContainer();

                foreach (GIS3DControllerBase controller in Controllers)
                {
                    controller.EngineContainer = EngineContainer;
                    controller.UseMapConfig = Model.UseMapConfig;

                    var eventPageRiskSituationController = controller as GIS3DEventPageRiskSituationLabelController;
                    if (eventPageRiskSituationController != null)
                    {
                        m_EventPageRiskSituationController = eventPageRiskSituationController;
                        eventPageRiskSituationController.AddEventPageLabelEvent += riskSituationController_AddLabelEvent;
                        eventPageRiskSituationController.DeleteEventPageLabelEvent +=
                            riskSituationController_DeleteLabelEvent;
                    }
                }

                InitBaseSetting();

                Loaded += ComplexView_Loaded;
            }
            else
            {
                throw new Exception("地图控件只接收含有三维地信引擎的控件");
            }
        }

        /// <summary>
        ///     控件加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComplexView_Loaded(object sender, RoutedEventArgs e)
        {
            OnDadChartLoaded();
        }

        /// <summary>
        ///     初始化容器，加载模型定义等
        /// </summary>
        private void InitContainer()
        {
            if (EngineContainer == null)
            {
                throw new Exception("三维地信引擎初始化未完成，请在引擎初始化之后为Container赋值");
            }
            if (EngineContainer.ModelDefinitionList == null)
            {
                throw new Exception("三维地信引擎初始化模型定义集合时出现错误");
            }
            var m1 = EngineContainer.ModelDefinitionList.FirstOrDefault(item => item.DisplayName == "节点轨迹模型");
            if (m1 != null)
            {
                return; // 防止多个dvm复合，加载多次
            }

            // 当前程序集模型定义导入
            var dadModelDefinitionImporter = new GIS3DModelDefinitionImporter();
            var dadModelDefinitionList = dadModelDefinitionImporter.GetModelDefinitions();
            var gisDadModelDefinitionList = dadModelDefinitionList.Cast<GIS3DModelDefinition>();

            EngineContainer.ModelDefinitionList.AddRange(gisDadModelDefinitionList);
        }

        /// <summary>
        ///     初始化基础设置
        /// </summary>
        private void InitBaseSetting()
        {
            if (EngineContainer == null) return;

            EngineContainer.SurfaceView.PropertyChanged += SurfaceView_PropertyChanged;
        }

        /// <summary>
        ///     上一次的free相机高度值
        /// </summary>
        private double m_PrevFreeHeight;

        /// <summary>
        ///     上一次的free相机经度值
        /// </summary>
        private double m_PrevFreeLon;

        /// <summary>
        ///     上一次的free相机维度值
        /// </summary>
        private double m_PrevFreeLat;

        /// <summary>
        ///     上一次的follow相机高度值
        /// </summary>
        private double m_PrevFollowHeight;

        /// <summary>
        ///     地球属性改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SurfaceView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (EngineContainer.SurfaceView.CameraMode == GlobeWorld.eCameraMode.Free)
            {
                if (e.PropertyName == "CurrentHeight") // free相机
                {
                    if (Math.Abs(m_PrevFreeHeight - EngineContainer.SurfaceView.CurrentHeight) < 0.001
                        && Math.Abs(m_PrevFreeLon - EngineContainer.SurfaceView.CurrentLongitude) < 0.001
                        && Math.Abs(m_PrevFreeLat - EngineContainer.SurfaceView.CurrentLatitude) < 0.001) return;

                    double centerLon = 0;
                    double minLat = 0;
                    double maxLat = 0;
                    EngineContainer.GlobeWorld.FreeGlobeCameraController.GetViewRangeOnGround(ref centerLon,
                        ref minLat, ref maxLat);

                    var delta = MapHelper.Meter2DecimalDegree(EngineContainer.SurfaceView.CurrentHeight);

                    if (Controllers != null && Controllers.Count > 0)
                    {
                        foreach (GIS3DControllerBase controller in Controllers)
                        {
                            var dvm = controller.DataViewModel as I3DSpatialQuery;
                            if (dvm == null) continue;

                            var minLon = centerLon - delta*dvm.SpatialQueryRatio;
                            var maxLon = centerLon + delta*dvm.SpatialQueryRatio;
                            if (minLon < -180)
                            {
                                minLon = -180;
                            }
                            if (maxLon > 180)
                            {
                                maxLon = 180;
                            }
                            controller.MapExtentChanged(minLon, maxLon, minLat, maxLat);
                        }
                    }

                    m_PrevFreeHeight = EngineContainer.SurfaceView.CurrentHeight;
                    m_PrevFreeLon = EngineContainer.SurfaceView.CurrentLongitude;
                    m_PrevFreeLat = EngineContainer.SurfaceView.CurrentLatitude;
                }
            }
            else if (EngineContainer.SurfaceView.CameraMode == GlobeWorld.eCameraMode.Follow)
            {
                if (EngineContainer.GlobeWorld.FollowGlobeCameraController.TargetEntity == null) return;

                var geo =
                    EngineContainer.GlobeWorld.FollowGlobeCameraController.TargetEntity.FindComponent(
                        typeof (GeographicCoordinateTransform)) as GeographicCoordinateTransform;

                if (Math.Abs(m_PrevFollowHeight - geo.Height) < 0.001) return;

                var delta = MapHelper.Meter2DecimalDegree(geo.Height);

                if (Controllers != null && Controllers.Count > 0)
                {
                    foreach (GIS3DControllerBase controller in Controllers)
                    {
                        var dvm = controller.DataViewModel as I3DSpatialQuery;
                        if (dvm == null) continue;

                        var minLon = geo.Longitude - delta*dvm.SpatialQueryRatio;
                        var maxLon = geo.Longitude + delta*dvm.SpatialQueryRatio;
                        var minLat = geo.Latitude - delta*dvm.SpatialQueryRatio;
                        var maxLat = geo.Latitude + delta*dvm.SpatialQueryRatio;
                        if (minLon < -180)
                        {
                            minLon = -180;
                        }
                        if (maxLon > 180)
                        {
                            maxLon = 180;
                        }
                        if (minLat < -90)
                        {
                            minLat = -90;
                        }
                        if (maxLat > 90)
                        {
                            maxLat = 90;
                        }
                        controller.MapExtentChanged(minLon, maxLon, minLat, maxLat);
                    }
                }
                m_PrevFollowHeight = geo.Height;
            }
        }

        /// <summary>
        ///     获得视野经纬度范围
        /// </summary>
        /// <param name="centerLongitude">视野中心的经度</param>
        /// <param name="minLatitude">视野范围最小纬度</param>
        /// <param name="maxLatitude">视野范围最大纬度</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public void GetViewRangeOnGround(ref double centerLongitude, ref double minLatitude, ref double maxLatitude)
        {
        }

        #endregion

        #region Override

        /// <summary>
        ///     更新样式
        /// </summary>
        public override void RefreshStyle()
        {
            UpdateAllStyle();
        }

        /// <summary>
        ///     更新样式
        /// </summary>
        /// <param name="propertyDescription"></param>
        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
            if (Controllers != null && Controllers.Count > 0)
            {
                foreach (GIS3DControllerBase controller in Controllers)
                {
                    controller.RefreshStyle(propertyDescription);
                }
            }
        }

        /// <summary>
        ///     外部直接传入图表数据来更新图表
        /// </summary>
        /// <param name="adtList"></param>
        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        /// <summary>
        ///     图表被联动时更新图表
        /// </summary>
        /// <param name="selectedModel"></param>
        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
            Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
            {
                foreach (GIS3DControllerBase controller in Controllers)
                {
                    if (controller.DataViewModel != null
                        && controller.DataViewModel.IsLinkage
                        &&
                        selectedModel.LinkageGroupName.ToLower() == controller.DataViewModel.LinkageGroupName.ToLower())
                    {
                        var conditionList = new List<AdapterConditionModel>();
                        conditionList.Add(selectedModel.Condition);
                        conditionList.Add(controller.Condition);
                        var conditions = AdapterConditionModelHelper.GetConditions(conditionList);

                        if (Player == null)
                        {
                            m_DataProxy.QueryAsync("", controller.DataViewModel.ID.ToString(), conditions)
                                .ContinueWith(
                                    t =>
                                    {
                                        Dispatcher.Invoke(DispatcherPriority.Send,
                                            new Action(delegate { controller.SetSelectedItem(t.Result); }));
                                    });
                        }
                        else
                        {
                            if (Player.State == Enums.PlayState.Stopped)
                            {
                                m_DataProxy.QueryAsync("", controller.DataViewModel.ID.ToString(), conditions)
                                    .ContinueWith(
                                        t =>
                                        {
                                            Dispatcher.Invoke(DispatcherPriority.Send,
                                                new Action(delegate { controller.SetSelectedItem(t.Result); }));
                                        });
                            }
                            else
                            {
                                if (controller.DataViewModel.DataTimeColumn != null)
                                {
                                    var condition =
                                        DataUtils.GetStepCondition(Player.CurrentAbsoluteTime,
                                            Player.StartTime,
                                            Player.PlayStepSize,
                                            Player.PlayStepGranularity,
                                            Player.StartTime,
                                            Player.StopTime,
                                            controller.DataViewModel,
                                            conditions);
                                    m_DataProxy.QueryAsync("", controller.DataViewModel.ID.ToString(), condition)
                                        .ContinueWith(
                                            t =>
                                            {
                                                Dispatcher.Invoke(DispatcherPriority.Send,
                                                    new Action(delegate { controller.SetSelectedItem(t.Result); }));
                                            });
                                }
                                else
                                {
                                    m_DataProxy.QueryAsync("", controller.DataViewModel.ID.ToString(), conditions)
                                        .ContinueWith(
                                            t =>
                                            {
                                                Dispatcher.Invoke(DispatcherPriority.Send,
                                                    new Action(delegate { controller.SetSelectedItem(t.Result); }));
                                            });
                                }
                            }
                        }
                    }
                }
            }));
        }

        /// <summary>
        ///     图表取消联动时更新图表
        /// </summary>
        /// <param name="clearModel"></param>
        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
            Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
            {
                foreach (GIS3DControllerBase controller in Controllers)
                {
                    if (controller.DataViewModel != null
                        && controller.DataViewModel.IsLinkage
                        && clearModel.LinkageGroupName.ToLower() == controller.DataViewModel.LinkageGroupName.ToLower())
                    {
                        controller.ClearSelectedItem();
                    }
                }
            }));
        }

        /// <summary>
        ///     导出图表
        /// </summary>
        /// <param name="type"></param>
        public override void ExportChart(ExportType type)
        {
        }

        /// <summary>
        ///     更新dvm
        /// </summary>
        /// <param name="dvms"></param>
        public override void UpdateDataViewModels(List<ChartDataViewModel> dvms)
        {
        }

        #endregion

        #region Override

        /// <summary>
        ///     释放container
        /// </summary>
        public override void DisposeContainer()
        {
            for (var i = Controllers.Count - 1; i >= 0; i--)
            {
                var controller = Controllers[i];
                controller.ClearChart(controller.DataViewModel);
                controller.Dispose();
                controller = null;
            }
            Controllers.Clear();
            Controllers = null;
        }

        /// <summary>
        ///     是否显示图层
        /// </summary>
        /// <param name="dvmId"></param>
        /// <param name="showLayer"></param>
        public override void ShowLayer(string dvmId, bool showLayer)
        {
            foreach (GIS3DControllerBase controller in Controllers)
            {
                if (controller.DataViewModel.ID.ToString().ToLower() == dvmId.ToLower())
                {
                    (controller.DataViewModel as IGIS3DDataViewModel).ShowLayer = showLayer;
                    controller.SetShowLayer(showLayer);
                    break;
                }
            }
        }

        /// <summary>
        ///     显示搜索结果
        /// </summary>
        /// <param name="dvmId"></param>
        /// <param name="adt"></param>
        public override void ShowSearchResult(string dvmId, AdapterDataTable adt)
        {
            foreach (GIS3DControllerBase controller in Controllers)
            {
                if (controller.DataViewModel.ID.ToString().ToLower() == dvmId.ToLower())
                {
                    controller.SetSearchResult(adt);
                    break;
                }
            }
        }

        #endregion

        #region 标签控件相关代码

        /// <summary>
        ///     清空Label控件
        /// </summary>
        private void riskSituationController_DeleteLabelEvent()
        {
            foreach (var child in eventPagegrdHidden.Children)
            {
                var control = child as EventPageRiskSituationLabelControl;
                if (control != null)
                {
                    control.Loaded -= control_Loaded;
                }
            }
            eventPagegrdHidden.Children.Clear();
        }

        /// <summary>
        ///     将标签控件添加到界面
        /// </summary>
        /// <param name="labelControls"></param>
        private void riskSituationController_AddLabelEvent(List<EventPageRiskSituationLabelControl> labelControls)
        {
            foreach (var control in labelControls)
            {
                eventPagegrdHidden.Children.Add(control);
                control.Loaded += control_Loaded;
            }
        }

        /// <summary>
        ///     标签控件加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_EventPageRiskSituationController == null) return;
            m_EventPageRiskSituationController.AddToMap(sender as EventPageRiskSituationLabelControl);
        }

        #endregion
    }
}