using System;
using System.Collections.Generic;
using System.Windows;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CMAPlatform.Chart.DVM
{
    [Serializable]
    public class RankingViewModel : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var column = new List<DataColumnModel>();
            if (RankingName != null)
                column.Add(RankingName);
            if (RankingCount != null)
                column.Add(RankingCount);
            return column;
        }

        #region MeasureColumnModel Fields

        private DimensionColumnModel m_RankingName;

        /// <summary>
        ///     名称
        /// </summary>
        [Synchronous]
        [PropertyDescription("名称", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, RefreshChartData = true)]
        public DimensionColumnModel RankingName
        {
            get { return m_RankingName; }
            set
            {
                m_RankingName = value;
                RaisePropertyChanged(() => RankingName);
            }
        }

        private MeasureColumnModel m_RankingCount;

        /// <summary>
        ///     数量
        /// </summary>
        [Synchronous]
        [PropertyDescription("数量", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, RefreshChartData = true)]
        public MeasureColumnModel RankingCount
        {
            get { return m_RankingCount; }
            set
            {
                m_RankingCount = value;
                RaisePropertyChanged(() => RankingCount);
            }
        }

        #endregion

        #region style Fields

        private int m_ControlWidth;

        /// <summary>
        ///     控件宽度
        /// </summary>
        [Synchronous]
        [PropertyDescription("控件宽度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public int ControlWidth
        {
            get { return m_ControlWidth; }
            set
            {
                m_ControlWidth = value;
                RaisePropertyChanged(() => ControlWidth);
            }
        }

        private int m_ControlHeight;

        /// <summary>
        ///     控件高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("控件高度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public int ControlHeight
        {
            get { return m_ControlHeight; }
            set
            {
                m_ControlHeight = value;
                RaisePropertyChanged(() => ControlHeight);
            }
        }

        private int m_ItemWidth;

        /// <summary>
        ///     控件子项宽度
        /// </summary>
        [Synchronous]
        [PropertyDescription("控件子项宽度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public int ItemWidth
        {
            get { return m_ItemWidth; }
            set
            {
                m_ItemWidth = value;
                RaisePropertyChanged(() => ItemWidth);
            }
        }


        private string m_ItemMargin;

        /// <summary>
        ///     控件子项宽度
        /// </summary>
        [Synchronous]
        [PropertyDescription("控件子项Margin", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public string ItemMargin
        {
            get { return m_ItemMargin; }
            set
            {
                m_ItemMargin = value;
                RaisePropertyChanged(() => ItemMargin);
            }
        }

        private int m_NameFontsize = 22;

        /// <summary>
        ///     名称字体大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("名称字体大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public int NameFontsize
        {
            get { return m_NameFontsize; }
            set
            {
                m_NameFontsize = value;
                RaisePropertyChanged(() => NameFontsize);
            }
        }

        private int m_CountFontsize = 30;

        /// <summary>
        ///     数量字体大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("数量字体大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public int CountFontsize
        {
            get { return m_CountFontsize; }
            set
            {
                m_CountFontsize = value;
                RaisePropertyChanged(() => CountFontsize);
            }
        }

        private string m_NameBackground = "0";

        /// <summary>
        ///     项背景底色
        /// </summary>
        [Synchronous]
        [PropertyDescription("名称背景色（0：使用背景色；1：不使用背景色）", Category = DescriptionEnum.样式设置,
            SubCategory = DescriptionEnum.样式设置, IsNecessary = true)]
        public string NameBackground
        {
            get { return m_NameBackground; }
            set
            {
                m_NameBackground = value;
                RaisePropertyChanged(() => NameBackground);
            }
        }


        private string m_ItemBackground = "#7FFFFFFF";

        /// <summary>
        ///     项背景底色
        /// </summary>
        [Synchronous]
        [PropertyDescription("项背景底色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public string ItemBackground
        {
            get { return m_ItemBackground; }
            set
            {
                m_ItemBackground = value;
                RaisePropertyChanged(() => ItemBackground);
            }
        }

        private string m_AlignType;

        /// <summary>
        ///     对齐方式
        /// </summary>
        [Synchronous]
        [PropertyDescription("数量对齐方式(left/Right)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public string AlignType
        {
            get { return m_AlignType; }
            set
            {
                m_AlignType = value;
                RaisePropertyChanged(() => AlignType);
            }
        }


        private string m_RankingType;

        /// <summary>
        ///     对齐方式
        /// </summary>
        [Synchronous]
        [PropertyDescription("排名类型(标准、非标准颜色、其他)填写normal、unnormal、other", Category = DescriptionEnum.样式设置,
            SubCategory = DescriptionEnum.样式设置, IsNecessary = true)]
        public string RankingType
        {
            get { return m_RankingType; }
            set
            {
                m_RankingType = value;
                RaisePropertyChanged(() => RankingType);
            }
        }

        private Visibility m_IsCount = Visibility.Collapsed;

        /// <summary>
        ///     数量显隐
        /// </summary>
        [Synchronous]
        [PropertyDescription("数量显隐", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public Visibility IsCount
        {
            get { return m_IsCount; }
            set
            {
                m_IsCount = value;
                RaisePropertyChanged(() => IsCount);
            }
        }

        private Visibility m_IsUnit = Visibility.Collapsed;

        /// <summary>
        ///     对齐方式
        /// </summary>
        [Synchronous]
        [PropertyDescription("单位显隐", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public Visibility IsUnit
        {
            get { return m_IsUnit; }
            set
            {
                m_IsUnit = value;
                RaisePropertyChanged(() => IsUnit);
            }
        }

        private Visibility m_IsRanking = Visibility.Collapsed;

        /// <summary>
        ///     排名显隐
        /// </summary>
        [Synchronous]
        [PropertyDescription("排名显隐", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public Visibility IsRanking
        {
            get { return m_IsRanking; }
            set
            {
                m_IsRanking = value;
                RaisePropertyChanged(() => IsRanking);
            }
        }

        private int m_ImgWidth = 61;

        /// <summary>
        ///     排名背景图片宽度
        /// </summary>
        [Synchronous]
        [PropertyDescription("排名背景图片宽度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.样式设置,
            IsNecessary = true)]
        public int ImgWidth
        {
            get { return m_ImgWidth; }
            set
            {
                m_ImgWidth = value;
                RaisePropertyChanged(() => ImgWidth);
            }
        }

        #endregion
    }
}