﻿using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CMAPlatform.TimeLine
{
    [Serializable]
    public class TimelineChartDataViewModel : ChartDataViewModel
    {
        public TimelineChartDataViewModel()
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
    }

    [Serializable]
    public class GIS3DTimelineChartDataViewModel : TimelineChartDataViewModel
    {
        private string m_NecessaryField2 = "";

        public GIS3DTimelineChartDataViewModel()
        {
            m_CanSort = false;
        }

        /// <summary>
        ///     必填字段
        /// </summary>
        [Synchronous]
        [PropertyDescription("必填字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            IsNecessary = true, RefreshChartData = true)]
        public string NecessaryField2
        {
            get { return m_NecessaryField2; }
            set
            {
                m_NecessaryField2 = value;
                RaisePropertyChanged(() => NecessaryField);
            }
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
    }
}