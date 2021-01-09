using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Digihail.DAD3.Models.DataAdapter;
using Telerik.Windows.Controls;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     风险态势标签控件(落区标签)
    /// </summary>
    public partial class EventPageRiskSituationLabelControl : UserControl
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public EventPageRiskSituationLabelControl()
        {
            InitializeComponent();
        }

        #region DependencyProperty

        /// <summary>
        ///     类型名称hanzi
        /// </summary>
        public string Type
        {
            get { return (string) GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        ///     类型名称
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof (string), typeof (EventPageRiskSituationLabelControl),
                new PropertyMetadata("暴雨"));

        /// <summary>
        ///     类型名称
        /// </summary>
        public string TypeName
        {
            get { return (string) GetValue(TypeNameProperty); }
            set { SetValue(TypeNameProperty, value); }
        }

        /// <summary>
        ///     类型名称
        /// </summary>
        public static readonly DependencyProperty TypeNameProperty =
            DependencyProperty.Register("TypeName", typeof (string), typeof (EventPageRiskSituationLabelControl),
                new PropertyMetadata("11B03"));

        /// <summary>
        ///     告警颜色
        /// </summary>
        public string Color
        {
            get { return (string) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        ///     告警颜色
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof (string), typeof (EventPageRiskSituationLabelControl),
                new PropertyMetadata("Red"));

        //----------------------------
        /// <summary>
        ///     经度
        /// </summary>
        public string Lon
        {
            get { return (string) GetValue(LonProperty); }
            set { SetValue(LonProperty, value); }
        }

        /// <summary>
        ///     经度
        /// </summary>
        public static readonly DependencyProperty LonProperty =
            DependencyProperty.Register("Lon", typeof (string), typeof (EventPageRiskSituationLabelControl),
                new PropertyMetadata("1"));

        /// <summary>
        ///     纬度
        /// </summary>
        public string Lat
        {
            get { return (string) GetValue(LatProperty); }
            set { SetValue(LatProperty, value); }
        }

        /// <summary>
        ///     纬度
        /// </summary>
        public static readonly DependencyProperty LatProperty =
            DependencyProperty.Register("Lat", typeof (string), typeof (EventPageRiskSituationLabelControl),
                new PropertyMetadata("1"));


        /// <summary>
        ///     告警值集合
        /// </summary>
        public ObservableCollection<EventPageRiskSituationLabelModel> Items
        {
            get { return (ObservableCollection<EventPageRiskSituationLabelModel>) GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        /// <summary>
        ///     告警值集合
        /// </summary>
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof (ObservableCollection<EventPageRiskSituationLabelModel>),
                typeof (EventPageRiskSituationLabelControl),
                new PropertyMetadata(new ObservableCollection<EventPageRiskSituationLabelModel>()));

        #endregion

        #region Properties

        /// <summary>
        ///     key:GroupKey
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        ///     该控件对应的数据
        /// </summary>
        public AdapterDataRow Row { get; set; }

        #endregion
    }

    /// <summary>
    ///     风险态势标签控件(落区标签)使用的模型
    /// </summary>
    public class EventPageRiskSituationLabelModel : ViewModelBase
    {
        private string m_Foreground = "White";
        private string m_Name;

        private double m_UiWidth;

        private double m_Value;

        /// <summary>
        ///     名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        ///     值
        /// </summary>
        public double Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        ///     宽度
        /// </summary>
        public double UiWidth
        {
            get { return m_UiWidth; }
            set
            {
                m_UiWidth = value;
                OnPropertyChanged("UiWidth");
            }
        }

        /// <summary>
        ///     文字颜色
        /// </summary>
        public string Foreground
        {
            get { return m_Foreground; }
            set
            {
                m_Foreground = value;
                OnPropertyChanged("Foreground");
            }
        }
    }
}