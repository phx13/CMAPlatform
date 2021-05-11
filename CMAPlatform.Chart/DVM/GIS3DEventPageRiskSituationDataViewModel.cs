using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.DVM
{
    /// <summary>
    ///     风险态势DVM(落区)
    /// </summary>
    [Serializable]
    public class GIS3DEventPageRiskSituationDataViewModel : ChartDataViewModel, IGIS3DDataViewModel, IGlobeStyle
    {
        /// <summary>
        ///     构造
        /// </summary>
        public GIS3DEventPageRiskSituationDataViewModel()
        {
            m_CanSort = false;
        }

        /// <summary>
        ///     获取列方法
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.RemoveAll(item => item == null);
            return columns;
        }

        #region 字段

        private DataSourceModel m_DataSourceModel;

        /// <summary>
        ///     当前数据视图的数据源模型
        /// </summary>
        [Synchronous]
        [PropertyDescription("数据源", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.DataSource, IsNecessary = false, IsEnable = false)]
        public new DataSourceModel DataSourceModel
        {
            get { return base.DataSourceModel; }
            set { base.DataSourceModel = value; }
        }


        private TimeColumnModel m_DataTimeColumn;

        /// <summary>
        ///     时间字段
        /// </summary>
        [Synchronous]
        [PropertyDescription("时间字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, IsNecessary = false, RefreshChartData = false, IsEnable = false)]
        public override TimeColumnModel DataTimeColumn
        {
            get { return base.DataTimeColumn; }
            set { base.DataTimeColumn = value; }
        }

        private DimensionColumnModel m_ChartSortField;

        /// <summary>
        ///     数据设置 - 数据设置 - 排序
        /// </summary>
        [Synchronous]
        [PropertyDescription("排序", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, IsNecessary = false, RefreshChartData = false, IsEnable = false)]
        public override DimensionColumnModel ChartSortField
        {
            get { return base.ChartSortField; }
            set { base.ChartSortField = value; }
        }


        private string m_NecessaryField = "";

        /// <summary>
        ///     必填字段
        /// </summary>
        [Synchronous]
        [PropertyDescription("必填字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            IsNecessary = true, RefreshChartData = true)]
        public string NecessaryField
        {
            get { return m_NecessaryField; }
            set
            {
                m_NecessaryField = value;
                RaisePropertyChanged(() => NecessaryField);
            }
        }

        // DimensionColumnModel

        #endregion

        #region 数据设置 - 其他

        private bool m_ShowLayer = true;

        /// <summary>
        ///     数据设置 - 其他 - 显示图层
        /// </summary>
        [Synchronous]
        [PropertyDescription(DescriptionEnum.显示图层, Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.其他)]
        public bool ShowLayer
        {
            get { return m_ShowLayer; }
            set
            {
                m_ShowLayer = value;
                RaisePropertyChanged(() => ShowLayer);
            }
        }

        private string m_LayerGroupName = "";

        /// <summary>
        ///     数据设置 - 其他 - 图层组名称
        /// </summary>
        [Synchronous]
        [PropertyDescription("图层组名称", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.其他)]
        public string LayerGroupName
        {
            get { return m_LayerGroupName; }
            set
            {
                m_LayerGroupName = value;
                RaisePropertyChanged(() => LayerGroupName);
            }
        }

        #region 样式设置 - 地图样式

        private string m_GlobeConfigPath = "";

        /// <summary>
        ///     样式设置 - 地图样式 - 样式配置
        /// </summary>
        [Synchronous]
        [PropertyDescription(DescriptionEnum.样式配置, Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式,
            PropertyType = EditorType.GCE)]
        public string GlobeConfigPath
        {
            get { return m_GlobeConfigPath; }
            set
            {
                m_GlobeConfigPath = value;
                RaisePropertyChanged(() => GlobeConfigPath);
            }
        }

        private double m_InitLon = 116.23;

        /// <summary>
        ///     样式设置 - 地图样式 - 起始视点经度
        /// </summary>
        [Synchronous]
        [PropertyDescription("起始视点经度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式,
            MinValue = -180, MaxValue = 180, DefaultValue = 116.23)]
        public double InitLon
        {
            get { return m_InitLon; }
            set
            {
                m_InitLon = value;
                RaisePropertyChanged(() => InitLon);
            }
        }

        private double m_InitLat = 39.54;

        /// <summary>
        ///     样式设置 - 地图样式 - 起始视点纬度
        /// </summary>
        [Synchronous]
        [PropertyDescription("起始视点纬度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式,
            MinValue = -85, MaxValue = 85, DefaultValue = 39.54)]
        public double InitLat
        {
            get { return m_InitLat; }
            set
            {
                m_InitLat = value;
                RaisePropertyChanged(() => InitLat);
            }
        }

        private double m_InitAlt = 30000000;

        /// <summary>
        ///     样式设置 - 地图样式 - 起始视点距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("起始视点距离(m)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式,
            MinValue = 10, MaxValue = 90000000, DefaultValue = 30000000)]
        public double InitAlt
        {
            get { return m_InitAlt; }
            set
            {
                m_InitAlt = value;
                RaisePropertyChanged(() => InitAlt);
            }
        }

        private double m_SurroundOffset;

        /// <summary>
        ///     样式设置 - 地图样式 - 起始水平旋转角度
        /// </summary>
        [Synchronous]
        [PropertyDescription("摄像机初始水平角度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式,
            MinValue = 0, MaxValue = 360, DefaultValue = 0)]
        public double SurroundOffset
        {
            get { return m_SurroundOffset; }
            set
            {
                m_SurroundOffset = value;
                RaisePropertyChanged(() => SurroundOffset);
            }
        }

        private double m_PitchOffset;

        /// <summary>
        ///     样式设置 - 地图样式 - 摄像机初始垂直角度
        /// </summary>
        [Synchronous]
        [PropertyDescription("摄像机初始垂直角度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式,
            MinValue = -90, MaxValue = 0, DefaultValue = 0)]
        public double PitchOffset
        {
            get { return m_PitchOffset; }
            set
            {
                m_PitchOffset = value;
                RaisePropertyChanged(() => PitchOffset);
            }
        }

        private double m_NearDistance;

        /// <summary>
        ///     样式设置 - 地图样式 - 最小视点距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("最小视点距离(m)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式)]
        public double NearDistance
        {
            get { return m_NearDistance; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value > m_FarDistance)
                {
                    value = m_FarDistance;
                }
                m_NearDistance = value;
                RaisePropertyChanged(() => NearDistance);
            }
        }

        private double m_FarDistance = 600000000;

        /// <summary>
        ///     样式设置 - 地图样式 - 最大视点距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("最大视点距离(m)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式)]
        public double FarDistance
        {
            get { return m_FarDistance; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value < m_NearDistance)
                {
                    value = m_NearDistance;
                }
                m_FarDistance = value;
                RaisePropertyChanged(() => FarDistance);
            }
        }


        private double m_StartLeanHeight = 100;

        /// <summary>
        ///     样式设置 - 地图样式 - 起始倾斜高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("自动倾斜开始距离(m)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式,
            MinValue = 1, MaxValue = 90000000, DefaultValue = 100)]
        public double StartLeanHeight
        {
            get { return m_StartLeanHeight; }
            set
            {
                m_StartLeanHeight = value;
                RaisePropertyChanged(() => StartLeanHeight);
            }
        }

        private double m_MinLeanRadius = 45;

        /// <summary>
        ///     样式设置 - 地图样式 - 自动倾斜最小垂角
        /// </summary>
        [Synchronous]
        [PropertyDescription("自动倾斜最小垂角", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式,
            MinValue = 0, MaxValue = 90, DefaultValue = 45)]
        public double MinLeanRadius
        {
            get { return m_MinLeanRadius; }
            set
            {
                m_MinLeanRadius = value;
                RaisePropertyChanged(() => MinLeanRadius);
            }
        }

        #endregion

        #endregion

        #region

        //#region 数据设置 - 数据设置

        //private DimensionColumnModel m_NameField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 名称字段
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("名称字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel NameField
        //{
        //    get { return m_NameField; }
        //    set
        //    {
        //        m_NameField = value;
        //        SmartSetColumn();
        //        RaisePropertyChanged(() => this.NameField);
        //    }
        //}

        //private DimensionColumnModel m_PolygonField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 多边形字段
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("多边形字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel PolygonField
        //{
        //    get { return m_PolygonField; }
        //    set
        //    {
        //        m_PolygonField = value;
        //        SmartSetColumn();
        //        RaisePropertyChanged(() => this.PolygonField);
        //    }
        //}

        //private DimensionColumnModel m_BorderBrushField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 颜色字段
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("边线颜色字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel BorderBrushField
        //{
        //    get { return m_BorderBrushField; }
        //    set
        //    {
        //        m_BorderBrushField = value;
        //        SmartSetColumn();
        //        RaisePropertyChanged(() => this.BorderBrushField);
        //    }
        //}

        //#endregion

        //#region 数据设置 - 其他

        //private bool m_ShowLayer = true;
        ///// <summary>
        ///// 数据设置 - 其他 - 显示图层
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription(DescriptionEnum.显示图层, Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.其他)]
        //public bool ShowLayer
        //{
        //    get { return m_ShowLayer; }
        //    set
        //    {
        //        m_ShowLayer = value;
        //        RaisePropertyChanged(() => this.ShowLayer);
        //    }
        //}

        //private string m_LayerGroupName = "";
        ///// <summary>
        ///// 数据设置 - 其他 - 图层组名称
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("图层组名称", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.其他)]
        //public string LayerGroupName
        //{
        //    get { return m_LayerGroupName; }
        //    set
        //    {
        //        m_LayerGroupName = value;
        //        RaisePropertyChanged(() => this.LayerGroupName);
        //    }
        //}

        //#endregion

        //#region 样式设置 - 颜色样式

        //private string m_FillColor = "#FF00FF00";
        ///// <summary>
        ///// 样式设置 - 颜色样式 - 填充颜色
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("填充颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.颜色样式, PropertyType = EditorType.Color)]
        //public string FillColor
        //{
        //    get { return m_FillColor; }
        //    set
        //    {
        //        m_FillColor = value;
        //        RaisePropertyChanged(() => FillColor);
        //    }
        //}

        //private double m_BorderThickness = 1;
        ///// <summary>
        ///// 样式设置 - 颜色样式 - 边线宽度
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription(DescriptionEnum.边线宽度, Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.颜色样式, MinValue = 0, MaxValue = 200, DefaultValue = 1)]
        //public double BorderThickness
        //{
        //    get { return m_BorderThickness; }
        //    set
        //    {
        //        m_BorderThickness = value;
        //        RaisePropertyChanged(() => BorderThickness);
        //    }
        //}

        //#endregion

        #region Override

        ///// <summary>
        ///// 获取所有用于查询分组的列
        ///// </summary>
        ///// <returns></returns>
        //public override List<DataColumnModel> GetColumns()
        //{
        //    List<DataColumnModel> columns = new List<DataColumnModel>();
        //    columns.Add(NameField);
        //    columns.Add(PolygonField);
        //    columns.Add(BorderBrushField);
        //    columns.Add(ChartSortField);
        //    columns.RemoveAll(item => item == null);
        //    return columns;
        //}

        #endregion

        ///// <summary>
        ///// 根据选择的变化，修改DVM的列属性（智能处理）
        ///// </summary>
        //private void SmartSetColumn()
        //{
        //    if (NameField != null)
        //    {
        //        NameField.OrderType = OrderTypes.None;
        //        NameField.CountMode = CountModes.None;
        //        NameField.ColumnQueryType = QueryTypes.Measure;
        //    }
        //    if (PolygonField != null)
        //    {
        //        PolygonField.OrderType = OrderTypes.None;
        //        PolygonField.CountMode = CountModes.None;
        //        PolygonField.ColumnQueryType = QueryTypes.Measure;
        //    }
        //    if (BorderBrushField != null)
        //    {
        //        BorderBrushField.OrderType = OrderTypes.None;
        //        BorderBrushField.CountMode = CountModes.None;
        //        BorderBrushField.ColumnQueryType = QueryTypes.Measure;
        //    }
        //}

        #endregion
    }
}