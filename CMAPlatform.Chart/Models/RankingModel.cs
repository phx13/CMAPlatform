using System.Windows;
using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Chart.Models
{
    public class RankingModel : NotificationObject
    {
        private string m_CountAlign;

        private int m_CountFontsize;

        private int m_ImgWidth;

        private Visibility m_IsCount;

        private string m_ItemBackground;

        private string m_ItemMargin;

        private int m_ItemWidth;

        private double m_MaxValue;

        private string m_NameBackground;

        private int m_NameFontsize;

        private Visibility m_Ranking;

        private string m_RankingColor;

        private int m_RankingCount;

        private string m_RankingName;

        private Visibility m_Unit;
        private int m_VIndex;

        /// <summary>
        ///     排名
        /// </summary>
        public int VIndex
        {
            get { return m_VIndex; }
            set
            {
                m_VIndex = value;
                RaisePropertyChanged("VIndex");
            }
        }

        /// <summary>
        ///     名称
        /// </summary>
        public string RankingName
        {
            get { return m_RankingName; }
            set
            {
                m_RankingName = value;
                RaisePropertyChanged("RankingName");
            }
        }

        /// <summary>
        ///     数量
        /// </summary>
        public int RankingCount
        {
            get { return m_RankingCount; }
            set
            {
                m_RankingCount = value;
                RaisePropertyChanged("RankingCount");
            }
        }

        /// <summary>
        ///     名称字体大小
        /// </summary>
        public int NameFontsize
        {
            get { return m_NameFontsize; }
            set
            {
                m_NameFontsize = value;
                RaisePropertyChanged(() => NameFontsize);
            }
        }

        /// <summary>
        ///     数量字体大小
        /// </summary>
        public int CountFontsize
        {
            get { return m_CountFontsize; }
            set
            {
                m_CountFontsize = value;
                RaisePropertyChanged(() => CountFontsize);
            }
        }

        /// <summary>
        ///     子项宽度
        /// </summary>
        public int ItemWidth
        {
            get { return m_ItemWidth; }
            set
            {
                m_ItemWidth = value;
                RaisePropertyChanged(() => ItemWidth);
            }
        }

        /// <summary>
        ///     子项Margin
        /// </summary>
        public string ItemMargin
        {
            get { return m_ItemMargin; }
            set
            {
                m_ItemMargin = value;
                RaisePropertyChanged(() => ItemMargin);
            }
        }

        /// <summary>
        ///     名称背景色
        /// </summary>
        public string NameBackground
        {
            get { return m_NameBackground; }
            set
            {
                m_NameBackground = value;
                RaisePropertyChanged("NameBackground");
            }
        }

        /// <summary>
        ///     项背景底色
        /// </summary>
        public string ItemBackground
        {
            get { return m_ItemBackground; }
            set
            {
                m_ItemBackground = value;
                RaisePropertyChanged("ItemBackground");
            }
        }

        /// <summary>
        ///     项颜色
        /// </summary>
        public string RankingColor
        {
            get { return m_RankingColor; }
            set
            {
                m_RankingColor = value;
                RaisePropertyChanged("RankingColor");
            }
        }

        /// <summary>
        ///     数量对齐方式
        /// </summary>
        public string CountAlign
        {
            get { return m_CountAlign; }
            set
            {
                m_CountAlign = value;
                RaisePropertyChanged("CountAlign");
            }
        }

        /// <summary>
        ///     数量单位显隐
        /// </summary>
        public Visibility IsCount
        {
            get { return m_IsCount; }
            set
            {
                m_IsCount = value;
                RaisePropertyChanged("IsCount");
            }
        }

        /// <summary>
        ///     项单位显隐
        /// </summary>
        public Visibility Unit
        {
            get { return m_Unit; }
            set
            {
                m_Unit = value;
                RaisePropertyChanged("Unit");
            }
        }

        /// <summary>
        ///     排名显隐
        /// </summary>
        public Visibility Ranking
        {
            get { return m_Ranking; }
            set
            {
                m_Ranking = value;
                RaisePropertyChanged("Ranking");
            }
        }

        /// <summary>
        ///     最大值
        /// </summary>
        public int ImgWidth
        {
            get { return m_ImgWidth; }
            set
            {
                m_ImgWidth = value;
                RaisePropertyChanged(() => ImgWidth);
            }
        }

        /// <summary>
        ///     最大值
        /// </summary>
        public double MaxValue
        {
            get { return m_MaxValue; }
            set
            {
                m_MaxValue = value;
                RaisePropertyChanged(() => MaxValue);
            }
        }
    }
}