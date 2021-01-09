using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CMAPlatform.Chart.DVM
{
    [Serializable]
    public class EventListDataViewModel : ChartDataViewModel
    {
        private DimensionColumnModel m_ChartSortField;

        private DataSourceModel m_DataSourceModel;


        private TimeColumnModel m_DataTimeColumn;

        private string m_NecessaryField = "";

        public EventListDataViewModel()
        {
            m_CanSort = false;
        }

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

        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.RemoveAll(item => item == null);
            return columns;
        }
    }
}