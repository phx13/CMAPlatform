using System.Windows;
using System.Windows.Controls;

namespace CMAPlatform.Chart.RankingItemControl
{
    /// <summary>
    ///     RankingItem.xaml 的交互逻辑
    /// </summary>
    public partial class RankingItem : UserControl
    {
        public RankingItem()
        {
            InitializeComponent();
        }

        #region DependencyProperty

        public static readonly DependencyProperty ItRankingIndexProperty = DependencyProperty.Register(
            "ItRankingIndex", typeof (int), typeof (RankingItem), new PropertyMetadata(default(int)));

        public int ItRankingIndex
        {
            get { return (int) GetValue(ItRankingIndexProperty); }
            set { SetValue(ItRankingIndexProperty, value); }
        }

        public static readonly DependencyProperty ItRankingNameProperty = DependencyProperty.Register(
            "ItRankingName", typeof (string), typeof (RankingItem), new PropertyMetadata(default(string)));

        public string ItRankingName
        {
            get { return (string) GetValue(ItRankingNameProperty); }
            set { SetValue(ItRankingNameProperty, value); }
        }

        public static readonly DependencyProperty ItRankingCountProperty = DependencyProperty.Register(
            "ItRankingCount", typeof (int), typeof (RankingItem), new PropertyMetadata(default(int)));

        public int ItRankingCount
        {
            get { return (int) GetValue(ItRankingCountProperty); }
            set { SetValue(ItRankingCountProperty, value); }
        }

        #endregion

        #region ItemStyle Fields

        public static readonly DependencyProperty ItColorProperty = DependencyProperty.Register(
            "ItColor", typeof (string), typeof (RankingItem), new PropertyMetadata(default(string)));

        /// <summary>
        ///     项颜色、数值颜色 、排名颜色
        /// </summary>
        public string ItColor
        {
            get { return (string) GetValue(ItColorProperty); }
            set { SetValue(ItColorProperty, value); }
        }

        public static readonly DependencyProperty ItNameBackgroundProperty = DependencyProperty.Register(
            "ItNameBackground", typeof (string), typeof (RankingItem), new PropertyMetadata(default(string)));

        /// <summary>
        ///     名称背景色
        /// </summary>
        public string ItNameBackground
        {
            get { return (string) GetValue(ItNameBackgroundProperty); }
            set { SetValue(ItNameBackgroundProperty, value); }
        }


        public static readonly DependencyProperty ItItemBackgroundProperty = DependencyProperty.Register(
            "ItItemBackground", typeof (string), typeof (RankingItem), new PropertyMetadata(default(string)));

        /// <summary>
        ///     项背景底色
        /// </summary>
        public string ItItemBackground
        {
            get { return (string) GetValue(ItItemBackgroundProperty); }
            set { SetValue(ItItemBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ItItemWidthProperty = DependencyProperty.Register(
            "ItItemWidth", typeof (int), typeof (RankingItem), new PropertyMetadata(default(int)));

        public int ItItemWidth
        {
            get { return (int) GetValue(ItItemWidthProperty); }
            set { SetValue(ItItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItItemMarginProperty = DependencyProperty.Register(
            "ItItemMargin", typeof (string), typeof (RankingItem), new PropertyMetadata(default(string)));

        /// <summary>
        ///     子项Margin
        /// </summary>
        public string ItItemMargin
        {
            get { return (string) GetValue(ItItemMarginProperty); }
            set { SetValue(ItItemMarginProperty, value); }
        }


        public static readonly DependencyProperty ItNameFontsizeProperty = DependencyProperty.Register(
            "ItNameFontsize", typeof (int), typeof (RankingItem), new PropertyMetadata(default(int)));

        /// <summary>
        ///     名称字体大小
        /// </summary>
        public int ItNameFontsize
        {
            get { return (int) GetValue(ItNameFontsizeProperty); }
            set { SetValue(ItNameFontsizeProperty, value); }
        }


        public static readonly DependencyProperty ItCountFontsizeProperty = DependencyProperty.Register(
            "ItCountFontsize", typeof (int), typeof (RankingItem), new PropertyMetadata(default(int)));

        /// <summary>
        ///     数值字体大小
        /// </summary>
        public int ItCountFontsize
        {
            get { return (int) GetValue(ItCountFontsizeProperty); }
            set { SetValue(ItCountFontsizeProperty, value); }
        }


        public static readonly DependencyProperty CountAlginProperty = DependencyProperty.Register(
            "CountAlgin", typeof (string), typeof (RankingItem), new PropertyMetadata(default(string)));

        /// <summary>
        ///     数字对齐方式
        /// </summary>
        public string CountAlgin
        {
            get { return (string) GetValue(CountAlginProperty); }
            set { SetValue(CountAlginProperty, value); }
        }


        public static readonly DependencyProperty ItCountShowProperty = DependencyProperty.Register(
            "ItCountShow", typeof (Visibility), typeof (RankingItem), new PropertyMetadata(default(Visibility)));

        /// <summary>
        ///     数量 显隐
        /// </summary>
        public Visibility ItCountShow
        {
            get { return (Visibility) GetValue(ItCountShowProperty); }
            set { SetValue(ItCountShowProperty, value); }
        }

        public static readonly DependencyProperty ItRankingProperty = DependencyProperty.Register(
            "ItRanking", typeof (Visibility), typeof (RankingItem), new PropertyMetadata(default(Visibility)));

        /// <summary>
        ///     排名 显示、隐藏
        /// </summary>
        public Visibility ItRanking
        {
            get { return (Visibility) GetValue(ItRankingProperty); }
            set { SetValue(ItRankingProperty, value); }
        }

        public static readonly DependencyProperty ItImgWidthProperty = DependencyProperty.Register(
            "ItImgWidth", typeof (int), typeof (RankingItem), new PropertyMetadata(default(int)));

        /// <summary>
        ///     排名图片背景宽度
        /// </summary>
        public int ItImgWidth
        {
            get { return (int) GetValue(ItImgWidthProperty); }
            set { SetValue(ItImgWidthProperty, value); }
        }

        public static readonly DependencyProperty UnitShowProperty = DependencyProperty.Register(
            "UnitShow", typeof (Visibility), typeof (RankingItem), new PropertyMetadata(default(Visibility)));

        /// <summary>
        ///     单位 显示、隐藏
        /// </summary>
        public Visibility UnitShow
        {
            get { return (Visibility) GetValue(UnitShowProperty); }
            set { SetValue(UnitShowProperty, value); }
        }

        public static readonly DependencyProperty CountShowProperty = DependencyProperty.Register(
            "CountShow", typeof (Visibility), typeof (RankingItem), new PropertyMetadata(default(Visibility)));

        /// <summary>
        ///     数值 显示、隐藏
        /// </summary>
        public Visibility CountShow
        {
            get { return (Visibility) GetValue(CountShowProperty); }
            set { SetValue(CountShowProperty, value); }
        }

        #endregion

        #region Progressbar Fields

        public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register(
            "ProgressValue", typeof (int), typeof (RankingItem), new PropertyMetadata(default(int)));

        public int ProgressValue
        {
            get { return (int) GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        public static readonly DependencyProperty ProgressMaxValueProperty = DependencyProperty.Register(
            "ProgressMaxValue", typeof (double), typeof (RankingItem), new PropertyMetadata(default(double)));

        public double ProgressMaxValue
        {
            get { return (double) GetValue(ProgressMaxValueProperty); }
            set { SetValue(ProgressMaxValueProperty, value); }
        }

        #endregion
    }
}