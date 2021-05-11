using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CMAPlatform.Chart.DVM;
using Digihail.AVE.Controls.GIS3D.OSG.Engine;
using Digihail.AVE.Controls.GIS3D.OSG.Utils;
using Digihail.AVE.Playback;
using Digihail.AVECLI.Controls.GIS3D.Core.EntityComponent.Visual;
using Digihail.AVECLI.Media3D.EntityFramework;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using OpenTK;

namespace CMAPlatform.Chart.Controller
{
    /// <summary>
    ///     风险态势控制器(落区)
    /// </summary>
    public class GIS3DRiskSituationController : GIS3DControllerBase
    {
        #region Private Methods

        /// <summary>
        ///     初始化地图控件相关内容
        /// </summary>
        private void InitGlobe()
        {
            if (EngineContainer == null) return;

            if (m_EngineContainer == null)
            {
                m_EngineContainer = (EngineContainer) EngineContainer;
            }
            if (m_ParentEntity == null)
            {
                m_ParentEntity = m_EngineContainer.GlobeWorld.World.AddEntity("RiskSituation_" + DataViewModel.Name);

                SetGlobeStyleConfig(DVM);
                SetGlobeStyle(DVM);
            }
        }

        #endregion

        /// <summary>
        ///     获取颜色级别
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private int GetColorLevel(string color)
        {
            var value = 0;
            switch (color)
            {
                case "red":
                    value = 4;
                    break;
                case "orange":
                    value = 3;
                    break;
                case "yellow":
                    value = 2;
                    break;
                case "blue":
                    value = 1;
                    break;
            }
            return value;
        }

        #region Properties & Fields

        /// <summary>
        ///     当前控制器的dvm
        /// </summary>
        public GIS3DRiskSituationDataViewModel DVM { get; private set; }

        /// <summary>
        ///     名称字段名称
        /// </summary>
        private string m_NameFieldName;

        /// <summary>
        ///     边线颜色列名称
        /// </summary>
        private string m_BorderBrushName;

        /// <summary>
        ///     多边形字段的名称
        /// </summary>
        private string m_PolygonName;

        /// <summary>
        ///     父节点
        /// </summary>
        private Entity3D m_ParentEntity;

        /// <summary>
        ///     key:name
        /// </summary>
        private readonly Dictionary<string, Entity3D> m_DicEntity = new Dictionary<string, Entity3D>();

        /// <summary>
        ///     用于存储区域数据
        /// </summary>
        private readonly Dictionary<string, AdapterDataRow> m_ZoneDataRows = new Dictionary<string, AdapterDataRow>();

        /// <summary>
        ///     引擎容器
        /// </summary>
        private EngineContainer m_EngineContainer;

        #endregion

        #region Constructor

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public GIS3DRiskSituationController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DVM = DataViewModel as GIS3DRiskSituationDataViewModel;

            SetBaseValue();
            InitGlobe();
        }

        /// <summary>
        ///     基础字段设置
        /// </summary>
        private void SetBaseValue()
        {
            m_NameFieldName = DVM.NameField.AsName;
            m_BorderBrushName = DVM.BorderBrushField.AsName;
            m_PolygonName = DVM.PolygonField.AsName;
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
            else if (propertyDescription.Category == DescriptionEnum.样式设置
                     && propertyDescription.SubCategory == DescriptionEnum.颜色样式)
            {
                //UpdateBorderThicknessAndFill(m_DVM.BorderThickness, m_DVM.FillColor);
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
            DVM = dvm as GIS3DRiskSituationDataViewModel;
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
            if (m_ParentEntity == null) return;
            m_ParentEntity.Visible = showLayer;
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
            // 删除
            foreach (var key in m_DicEntity.Keys)
            {
                var entity = m_DicEntity[key];
                m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(entity);
            }
            m_DicEntity.Clear();
            m_ZoneDataRows.Clear();
            // 添加对象
            foreach (var adtRow in adt.Rows) // 遍历所有需要初始化的对象
            {
                var dataKey = adtRow[m_NameFieldName].ToString();
                AddModel(adtRow, dataKey); // 添加
            }
        }

        /// <summary>
        ///     添加对象
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="dataKey"></param>
        private void AddModel(AdapterDataRow dataRow, string dataKey)
        {
            if (m_ZoneDataRows.ContainsKey(dataKey))
            {
                var oldrow = m_ZoneDataRows[dataKey];

                //获取颜色级别
                var oldcolorlv = GetColorLevel(oldrow[m_BorderBrushName].ToString().Trim().ToLower());
                var newcolorlv = GetColorLevel(dataRow[m_BorderBrushName].ToString().Trim().ToLower());

                //如果旧颜色级别高,不必更新
                if (oldcolorlv >= newcolorlv)
                {
                }
                else
                {
                    //更新级别
                    m_ZoneDataRows[dataKey] = dataRow;
                    //更新颜色
                    UpdateZoneFill(dataKey, dataRow);
                }
            }
            else
            {
                var polygonEntity = m_EngineContainer.GlobeWorld.World.AddEntity(dataKey);
                polygonEntity.Parent = m_ParentEntity;

                m_DicEntity.Add(dataKey, polygonEntity);
                m_ZoneDataRows.Add(dataKey, dataRow);

                var borderBrush = dataRow[m_BorderBrushName].ToString();

                // 将时间轴推送的数据转为三维组件需要的形式
                //string polygonString = dataRow[m_PolygonName].ToString();
                var polygonId = dataRow[m_PolygonName].ToString();
                var path = Path.Combine(Environment.CurrentDirectory, "CMARiskSituation", polygonId);
                if (File.Exists(path))
                {
                    var polygonString = File.ReadAllText(path);

                    //ConvertStringToPolygon(polygonEntity, dataKey, polygonString, borderBrush, m_DVM.FillColor, m_DVM.BorderThickness);
                    ConvertStringToMultiPolygon(polygonEntity, dataKey, polygonString, borderBrush, DVM.FillColor,
                        DVM.BorderThickness);
                }
                else
                {
                    ChartLogManager.PrintDebugMesage("落区控件GIS3DRiskSituationController", "AddModel方法",
                        string.Format("未找到落区:{0}", polygonId));
                    //   MessageBox.Show("未找到落区{0}", polygonId);
                }
            }
        }

        /// <summary>
        ///     把Polygon字符串转换为Polygon对象
        /// </summary>
        /// <param name="polygonEntity"></param>
        /// <param name="dataKey"></param>
        /// <param name="polygonString"></param>
        /// <param name="borderBrush"></param>
        /// <param name="fill"></param>
        /// <param name="borderThickness"></param>
        private void ConvertStringToPolygon(Entity3D polygonEntity, string dataKey, string polygonString,
            string borderBrush, string fill, double borderThickness)
        {
            if (polygonString.Length <= 9) return;
            if (!polygonString.StartsWith("POLYGON")) return;

            polygonString = polygonString.Substring(9, polygonString.Length - 11);
            var polygons = polygonString.Split(new[] {"), ("}, StringSplitOptions.RemoveEmptyEntries).ToArray();
            for (var i = 0; i < polygons.Length; i++)
            {
                var singlePolygonString = polygons[i];

                // 创建子entity
                var entity = m_EngineContainer.GlobeWorld.World.AddEntity(dataKey + "_" + i);
                entity.Parent = polygonEntity;
                // 添加 PolygonComponent
                var osgPolygonCpn = new OsgPolygonComponent();
                osgPolygonCpn.FillColor = ColorHelper.GetOpenTKColor4FromHexString(borderBrush);
                entity.AddComponent(osgPolygonCpn);
                //// 添加 PolylineComponent
                //OsgPolylineComponent osgPolylineCpn = new OsgPolylineComponent();
                //osgPolylineCpn.LineWidth = (float)borderThickness;
                //osgPolylineCpn.LineColor = ColorHelper.GetOpenTKColor4FromHexString(borderBrush);
                //entity.AddComponent(osgPolylineCpn);

                var points = singlePolygonString.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).ToArray();
                foreach (var lla in points)
                {
                    var point = lla.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    var lon = double.Parse(point[0]);
                    var lat = double.Parse(point[1]);
                    osgPolygonCpn.AddPoint(new Vector3d(lon, lat, 0));
                    //osgPolylineCpn.AddPoint(new Vector3d(lon, lat, 0));
                }
            }
        }

        /// <summary>
        ///     把Polygon字符串转换为Polygon对象
        /// </summary>
        /// <param name="polygonEntity"></param>
        /// <param name="dataKey"></param>
        /// <param name="polygonString"></param>
        /// <param name="borderBrush"></param>
        /// <param name="fill"></param>
        /// <param name="borderThickness"></param>
        private void ConvertStringToMultiPolygon(Entity3D polygonEntity, string dataKey, string polygonString,
            string borderBrush, string fill, double borderThickness)
        {
            if (polygonString.Length <= 14) return;
            if (!polygonString.StartsWith("MULTIPOLYGON")) return;

            polygonString = polygonString.Substring(13, polygonString.Length - 14);
            var multiPolygons = polygonString.Split(new[] {")),(("}, StringSplitOptions.RemoveEmptyEntries).ToArray();
            for (var i = 0; i < multiPolygons.Length; i++)
            {
                var polygonsString = multiPolygons[i]; // 多个多边形
                if (i == 0)
                {
                    polygonsString = polygonsString.Substring(2, polygonsString.Length - 2);
                }
                if (i == multiPolygons.Length - 1)
                {
                    polygonsString = polygonsString.Substring(0, polygonsString.Length - 2);
                }

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
                    var polygonColor = "#4CFFFFFF";
                    if (borderBrush == "orange")
                    {
//30_4C   70_B2   50_80
                        polygonColor = "#B2FF8F00";
                    }
                    if (borderBrush == "red")
                    {
                        polygonColor = "#B2FF0000";
                    }

                    osgPolygonCpn.FillColor = ColorHelper.GetOpenTKColor4FromHexString(polygonColor);
                    entity.AddComponent(osgPolygonCpn);

                    //OsgPolylineComponent osgPolylineCpn = new OsgPolylineComponent();
                    //osgPolylineCpn.LineWidth = (float)borderThickness;
                    //osgPolylineCpn.LineColor = ColorHelper.GetOpenTKColor4FromHexString(borderBrush);
                    //entity.AddComponent(osgPolylineCpn);

                    var points = singlePolygon.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).ToArray();


                    //消除点逻辑
                    for (var k = 0; k < points.Length; k++)
                    {
                        var lla = points[k];


                        var point = lla.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).ToArray();
                        var lon = double.Parse(point[0]);
                        var lat = double.Parse(point[1]);
                        osgPolygonCpn.AddPoint(new Vector3d(lon, lat, 0));

                        //osgPolylineCpn.AddPoint(new Vector3d(lon, lat, 0));
                    }
                }
            }
        }

        /// <summary>
        ///     更新边线宽度
        /// </summary>
        /// <param name="borderThickness"></param>
        /// <param name="fill"></param>
        private void UpdateBorderThicknessAndFill(double borderThickness, string fill)
        {
            foreach (var entity3D in m_DicEntity.Values)
            {
                foreach (var child in entity3D.Children)
                {
                    //OsgPolylineComponent polylineCpn = child.FindComponent<OsgPolylineComponent>();
                    //if (polylineCpn != null)
                    //{
                    //    polylineCpn.LineWidth = (float)borderThickness;
                    //}

                    var polygonCpn = child.FindComponent<OsgPolygonComponent>();
                    if (polygonCpn != null)
                    {
                        polygonCpn.FillColor = ColorHelper.GetOpenTKColor4FromHexString(fill);
                    }
                }
            }
        }


        /// <summary>
        ///     更新区域对象颜色
        /// </summary>
        /// <param name="datakey"></param>
        /// <param name="dataRow"></param>
        private void UpdateZoneFill(string datakey, AdapterDataRow dataRow)
        {
            if (m_DicEntity.ContainsKey(datakey))
            {
                var zoneRoot = m_DicEntity[datakey];

                foreach (var childZone in zoneRoot.Children)
                {
                    var polygonCpn = childZone.FindComponent<OsgPolygonComponent>();
                    if (polygonCpn != null)
                    {
                        polygonCpn.FillColor =
                            ColorHelper.GetOpenTKColor4FromHexString(
                                dataRow[m_BorderBrushName].ToString().Trim().ToLower());
                    }
                }
            }
        }

        /// <summary>
        ///     清除图层上所有对象
        /// </summary>
        private void RemoveAll()
        {
            if (DVM == null) return;

            if (m_ParentEntity != null && m_EngineContainer != null)
            {
                m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_ParentEntity);
                m_ParentEntity = null;
            }

            m_DicEntity.Clear();
        }

        #endregion
    }
}