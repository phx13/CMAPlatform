using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.DVM
{
    /// <summary>
    ///     风险态势标牌DVM(落区标牌)
    /// </summary>
    [Serializable]
    public class GIS3DEventPageRiskSituationLabelDataViewModel : ChartDataViewModel, IGIS3DDataViewModel, IGlobeStyle
    {
        /// <summary>
        ///     构造
        /// </summary>
        public GIS3DEventPageRiskSituationLabelDataViewModel()
        {
            m_CanSort = true;
        }

        #region Override

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

        ///// <summary>
        ///// 获取所有用于查询分组的列
        ///// </summary>
        ///// <returns></returns>
        //public override List<DataColumnModel> GetColumns()
        //{
        //    List<DataColumnModel> columns = new List<DataColumnModel>();
        //    columns.Add(GroupKeyField);
        //    columns.Add(LonField);
        //    columns.Add(LatField);
        //    columns.Add(AltField);
        //    columns.Add(LevelField);
        //    columns.Add(TypeNameField);
        //    columns.Add(RedCountField);
        //    columns.Add(YellowCountField);
        //    columns.Add(OrangeCountField);
        //    columns.Add(BlueCountField);
        //    columns.RemoveAll(item => item == null);
        //    return columns;
        //}

        #endregion

        ///// 数据设置 - 数据设置 - 批号

        ///// <summary>

        //#region 数据设置 - 数据设置

        //private DimensionColumnModel m_GroupKeyField;

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
        //private DimensionColumnModel m_LonField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 经度
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("经度字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel LonField
        //{
        //    get { return m_LonField; }
        //    set
        //    {
        //        m_LonField = value;
        //        RaisePropertyChanged(() => this.LonField);
        //    }
        //}

        //private DimensionColumnModel m_LatField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 纬度
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("纬度字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel LatField
        //{
        //    get { return m_LatField; }
        //    set
        //    {
        //        m_LatField = value;
        //        RaisePropertyChanged(() => this.LatField);
        //    }
        //}

        //private DimensionColumnModel m_AltField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 高度
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("高度字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = false, RefreshChartData = true)]
        //public DimensionColumnModel AltField
        //{
        //    get { return m_AltField; }
        //    set
        //    {
        //        m_AltField = value;
        //        RaisePropertyChanged(() => this.AltField);
        //    }
        //}

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

        #endregion

        #region 样式设置 - 颜色样式

        private double m_ClipRange = 10000000000;

        /// <summary>
        ///     大小图标过渡距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("大小图标过渡距离", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示,
            MinValue = 0, MaxValue = 100000000000, DefaultValue = 10000000000)]
        public double ClipRange
        {
            get { return m_ClipRange; }
            set
            {
                m_ClipRange = value;
                RaisePropertyChanged(() => ClipRange);
            }
        }

        private double m_FarFactor = 0.3;

        /// <summary>
        ///     小图标倍数
        /// </summary>
        [Synchronous]
        [PropertyDescription("小图标倍数", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示, MinValue = 0,
            MaxValue = 10, DefaultValue = 0.5)]
        public double FarFactor
        {
            get { return m_FarFactor; }
            set
            {
                m_FarFactor = value;
                RaisePropertyChanged(() => FarFactor);
            }
        }

        private double m_NearFactor = 1;

        /// <summary>
        ///     大图标倍数
        /// </summary>
        [Synchronous]
        [PropertyDescription("大图标倍数", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示, MinValue = 0,
            MaxValue = 10, DefaultValue = 1)]
        public double NearFactor
        {
            get { return m_NearFactor; }
            set
            {
                m_NearFactor = value;
                RaisePropertyChanged(() => NearFactor);
            }
        }

        private double m_MinVisibleDistance;

        /// <summary>
        ///     样式设置 - 放缩显示 - 最小可见距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("最小可见距离", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示, MinValue = 0,
            MaxValue = 100000000000, DefaultValue = 10000000000)]
        public double MinVisibleDistance
        {
            get { return m_MinVisibleDistance; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                m_MinVisibleDistance = value;
                RaisePropertyChanged(() => MinVisibleDistance);
            }
        }

        private double m_MaxVisibleDistance = 100000000000;

        /// <summary>
        ///     样式设置 - 放缩显示 - 最大可见距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("最大可见距离", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示, MinValue = 0,
            MaxValue = 1000000000000, DefaultValue = 100000000000)]
        public double MaxVisibleDistance
        {
            get { return m_MaxVisibleDistance; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value < m_MinVisibleDistance)
                {
                    value = m_MinVisibleDistance;
                }
                m_MaxVisibleDistance = value;
                RaisePropertyChanged(() => MaxVisibleDistance);
            }
        }

        #endregion

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

        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("批号字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel GroupKeyField
        //{
        //    get { return m_GroupKeyField; }
        //    set
        //    {
        //        m_GroupKeyField = value;
        //        RaisePropertyChanged(() => this.GroupKeyField);
        //    }
        //}

        //private DimensionColumnModel m_LonField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 经度
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("经度字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel LonField
        //{
        //    get { return m_LonField; }
        //    set
        //    {
        //        m_LonField = value;
        //        RaisePropertyChanged(() => this.LonField);
        //    }
        //}

        //private DimensionColumnModel m_LatField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 纬度
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("纬度字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel LatField
        //{
        //    get { return m_LatField; }
        //    set
        //    {
        //        m_LatField = value;
        //        RaisePropertyChanged(() => this.LatField);
        //    }
        //}

        //private DimensionColumnModel m_AltField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 高度
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("高度字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = false, RefreshChartData = true)]
        //public DimensionColumnModel AltField
        //{
        //    get { return m_AltField; }
        //    set
        //    {
        //        m_AltField = value;
        //        RaisePropertyChanged(() => this.AltField);
        //    }
        //}

        //private DimensionColumnModel m_TypeNameField;
        ///// <summary>
        ///// 类型名称字段
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("类型名称字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel TypeNameField
        //{
        //    get { return m_TypeNameField; }
        //    set
        //    {
        //        m_TypeNameField = value;
        //        RaisePropertyChanged(() => this.TypeNameField);
        //    }
        //}

        //private DimensionColumnModel m_LevelField;
        ///// <summary>
        ///// 告警级别字段
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("告警级别字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel LevelField
        //{
        //    get { return m_LevelField; }
        //    set
        //    {
        //        m_LevelField = value;
        //        RaisePropertyChanged(() => this.LevelField);
        //    }
        //}

        //private DimensionColumnModel m_RedCountField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 红色告警数量
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("红色告警数量", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel RedCountField
        //{
        //    get { return m_RedCountField; }
        //    set
        //    {
        //        m_RedCountField = value;
        //        RaisePropertyChanged(() => this.RedCountField);
        //    }
        //}

        //private DimensionColumnModel m_YellowCountField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 黄色告警数量
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("黄色告警数量", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel YellowCountField
        //{
        //    get { return m_YellowCountField; }
        //    set
        //    {
        //        m_YellowCountField = value;
        //        RaisePropertyChanged(() => this.YellowCountField);
        //    }
        //}

        //private DimensionColumnModel m_OrangeCountField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 橙色告警数量
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("橙色告警数量", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        //public DimensionColumnModel OrangeCountField
        //{
        //    get { return m_OrangeCountField; }
        //    set
        //    {
        //        m_OrangeCountField = value;
        //        RaisePropertyChanged(() => this.OrangeCountField);
        //    }
        //}

        //private DimensionColumnModel m_BlueCountField;
        ///// <summary>
        ///// 数据设置 - 数据设置 - 蓝色告警数量
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("蓝色告警数量", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]

        //public DimensionColumnModel BlueCountField
        //{
        //    get { return m_BlueCountField; }
        //    set
        //    {
        //        m_BlueCountField = value;
        //        RaisePropertyChanged(() => this.BlueCountField);
        //    }
        //}

        //#endregion
    }
}