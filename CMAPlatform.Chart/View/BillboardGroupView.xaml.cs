using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Digihail.CCP.Models;
using Digihail.DAD3.DataAdapter.DataAdapters;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     BillboardGroupView.xaml 的交互逻辑
    /// </summary>
    [Export("IAVE3DInfoWindowContent", typeof (UserControl))]
    public partial class BillboardGroupView : UserControl, INotifyPropertyChanged
    {
        #region 构造

        public BillboardGroupView()
        {
            InitializeComponent();
            DataContextChanged += InfoWindowPanel_DataContextChanged;
        }

        #endregion

        #region Methods

        #endregion

        #region Fields

        private string m_Id;

        /// <summary>
        ///     编号
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set
            {
                if (m_Id != value)
                {
                    m_Id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        private string m_TyphoonName;

        /// <summary>
        ///     名称
        /// </summary>
        public string TyphoonName
        {
            get { return m_TyphoonName; }
            set
            {
                if (m_TyphoonName != value)
                {
                    m_TyphoonName = value;
                    NotifyPropertyChanged("TyphoonName");
                }
            }
        }

        private string m_EnglishName;

        /// <summary>
        ///     英文名称
        /// </summary>
        public string EnglishName
        {
            get { return m_EnglishName; }
            set
            {
                if (m_EnglishName != value)
                {
                    m_EnglishName = value;
                    NotifyPropertyChanged("EnglishName");
                }
            }
        }

        private string m_ArriveTime;

        /// <summary>
        ///     到达时间
        /// </summary>
        public string ArriveTime
        {
            get { return m_ArriveTime; }
            set
            {
                if (m_ArriveTime != value)
                {
                    m_ArriveTime = value;
                    NotifyPropertyChanged("ArriveTime");
                }
            }
        }

        private string m_CenterLocation;

        /// <summary>
        ///     中心位置
        /// </summary>
        public string CenterLocation
        {
            get { return m_CenterLocation; }
            set
            {
                if (m_CenterLocation != value)
                {
                    m_CenterLocation = value;
                    NotifyPropertyChanged("CenterLocation");
                }
            }
        }

        private string m_Wind;

        /// <summary>
        ///     风速风力
        /// </summary>
        public string Wind
        {
            get { return m_Wind; }
            set
            {
                if (m_Wind != value)
                {
                    m_Wind = value;
                    NotifyPropertyChanged("Wind");
                }
            }
        }

        private string m_Pascal;

        /// <summary>
        ///     中心气压
        /// </summary>
        public string Pascal
        {
            get { return m_Pascal; }
            set
            {
                if (m_Pascal != value)
                {
                    m_Pascal = value;
                    NotifyPropertyChanged("Pascal");
                }
            }
        }

        private string m_FutureSpeed;

        /// <summary>
        ///     未来移速
        /// </summary>
        public string FutureSpeed
        {
            get { return m_FutureSpeed; }
            set
            {
                if (m_FutureSpeed != value)
                {
                    m_FutureSpeed = value;
                    NotifyPropertyChanged("FutureSpeed");
                }
            }
        }

        private string m_FutureDirection;

        /// <summary>
        ///     未来移向
        /// </summary>
        public string FutureDirection
        {
            get { return m_FutureDirection; }
            set
            {
                if (m_FutureDirection != value)
                {
                    m_FutureDirection = value;
                    NotifyPropertyChanged("FutureDirection");
                }
            }
        }

        private string m_Conclusion;

        /// <summary>
        ///     预报结论
        /// </summary>
        public string Conclusion
        {
            get { return m_Conclusion; }
            set
            {
                if (m_Conclusion != value)
                {
                    m_Conclusion = value;
                    NotifyPropertyChanged("Conclusion");
                }
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     DataContext变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoWindowPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            root.Visibility = Visibility.Collapsed;
            var panelmodel = DataContext as InfoPanelModel;
            if (panelmodel != null)
            {
                //构建一个 列名=值的ACM 方便下面的查询
                var acm = new AdapterConditionModel();
                acm.Column = new DataColumnModel
                {
                    ColumnName = panelmodel.ColumnName
                };
                acm.JudgmentType = ConditionJudgmentTypes.Equal;
                acm.JudgmentObject = panelmodel.Key;

                //调用 GetConditionData 方法取到符合条件的数据，即当前选中的数据
                var result = AdapterManager.GetConditionData(panelmodel.DataSourceModel, acm);
                if (result != null && result.Rows != null)
                {
                    if (panelmodel.DataSourceModel.TableName == "typhoonpath")
                    {
                        root.Visibility = Visibility.Visible;
                        var row = result.Rows.FirstOrDefault();
                        if (row != null && row.Cells.Count > 0)
                        {
                            foreach (var cell in row.Cells)
                            {
                                if (cell.ColumnName.Name == "TyphoonId") //编号
                                {
                                    Id = cell.Content.ToString();
                                }
                                else if (cell.ColumnName.Name == "Name") //名字
                                {
                                    TyphoonName = cell.Content.ToString();
                                }
                                else if (cell.ColumnName.Name == "EnglishName") //英文名字
                                {
                                    EnglishName = cell.Content.ToString();
                                }
                                else if (cell.ColumnName.Name == "ArriveTime") //到达时间
                                {
                                    ArriveTime = cell.Content.ToString();
                                }
                                else if (cell.ColumnName.Name == "Center") //中心位置
                                {
                                    CenterLocation = cell.Content.ToString();
                                }
                                else if (cell.ColumnName.Name == "Wind") //风力风速
                                {
                                    Wind = cell.Content + "米/秒";
                                }
                                else if (cell.ColumnName.Name == "Pascal") //帕斯卡
                                {
                                    Pascal = cell.Content + "百帕";
                                }
                                else if (cell.ColumnName.Name == "FutureSpeed") //未来风速
                                {
                                    if (cell.Content.ToString().ToLower() == "null")
                                    {
                                        FutureSpeed = "";
                                    }
                                    else
                                    {
                                        FutureSpeed = cell.Content + "公里/小时";
                                    }
                                }
                                else if (cell.ColumnName.Name == "FutureDirection") //未来方向
                                {
                                    FutureDirection = cell.Content.ToString();
                                }
                                else if (cell.ColumnName.Name == "Conclusion") //结论
                                {
                                    Conclusion = cell.Content.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}