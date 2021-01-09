using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CMAPlatform.Chart.DVM
{
    [Serializable]
    public class BubbleViewModel : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();

            columns.Add(ItemName);
            columns.Add(ItemNumber);

            //清除多余列
            columns.RemoveAll(item => item == null);
            return columns;
        }

        #region 数据设置

        private DimensionColumnModel m_ItemName;

        /// <summary>
        ///     数据设置-项目名称
        /// </summary>
        [Synchronous]
        [PropertyDescription("项目名称", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public DimensionColumnModel ItemName
        {
            get { return m_ItemName; }
            set
            {
                m_ItemName = value;
                RaisePropertyChanged(() => ItemName);
            }
        }

        private DimensionColumnModel m_ItemNumber;

        /// <summary>
        ///     数据设置-项目名称
        /// </summary>
        [Synchronous]
        [PropertyDescription("项目数量", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public DimensionColumnModel ItemNumber
        {
            get { return m_ItemNumber; }
            set
            {
                m_ItemNumber = value;
                RaisePropertyChanged(() => ItemNumber);
            }
        }

        #endregion

        #region 数据设置

        /// <summary>
        ///     图表宽度
        /// </summary>
        private double m_RingWidth;

        [Synchronous]
        [PropertyDescription("环线宽", Category = "样式设置", SubCategory = "大小设置", PropertyType = EditorType.None)]
        public double RingWidth
        {
            get { return m_RingWidth; }
            set
            {
                m_RingWidth = value;
                RaisePropertyChanged(() => RingWidth);
            }
        }

        /// <summary>
        ///     图表宽度
        /// </summary>
        private double m_BiggerIndex = 5;

        [Synchronous]
        [PropertyDescription("差异系统", Category = "样式设置", SubCategory = "大小设置", PropertyType = EditorType.None)]
        public double BiggerIndex
        {
            get { return m_BiggerIndex; }
            set
            {
                m_BiggerIndex = value;
                RaisePropertyChanged(() => BiggerIndex);
            }
        }

        /// <summary>
        ///     图表宽度
        /// </summary>
        private double m_TextSize = 20;

        [Synchronous]
        [PropertyDescription("字体大小", Category = "样式设置", SubCategory = "大小设置", PropertyType = EditorType.None)]
        public double TextSize
        {
            get { return m_TextSize; }
            set
            {
                m_TextSize = value;
                RaisePropertyChanged(() => TextSize);
            }
        }

        private string m_TextColor;

        /// <summary>
        ///     样式设置- 折线颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("字体颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.颜色样式,
            PropertyType = EditorType.Color)]
        public string TextColor
        {
            get { return m_TextColor; }
            set
            {
                m_TextColor = value;
                RaisePropertyChanged(() => TextColor);
            }
        }

        /// <summary>
        ///     图表宽度
        /// </summary>
        private double m_ChartWidth = 200;

        [Synchronous]
        [PropertyDescription("图表宽度", Category = "样式设置", SubCategory = "大小设置", PropertyType = EditorType.None)]
        public double ChartWidth
        {
            get { return m_ChartWidth; }
            set
            {
                m_ChartWidth = value;
                RaisePropertyChanged(() => ChartWidth);
            }
        }

        /// <summary>
        ///     图表高度
        /// </summary>
        private double m_ChartHeight = 200;

        [Synchronous]
        [PropertyDescription("图表高度", Category = "样式设置", SubCategory = "大小设置", PropertyType = EditorType.None)]
        public double ChartHeight
        {
            get { return m_ChartHeight; }
            set
            {
                m_ChartHeight = value;
                RaisePropertyChanged(() => ChartHeight);
            }
        }

        #endregion
    }
}