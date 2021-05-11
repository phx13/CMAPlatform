using System;
using System.Collections.ObjectModel;
using System.Windows;
using CMAPlatform.Chart.DVM;
using CMAPlatform.Chart.Models;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    public class RankingController : ChartControllerBase
    {
        public RankingController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            Dvm = dvm as RankingViewModel;
            if (Dvm != null)
            {
                ControlWidth = Dvm.ControlWidth;
                ControlHeight = Dvm.ControlHeight;
                ItemWidth = Dvm.ItemWidth;
                ItemMargin = Dvm.ItemMargin;
                ItemBackground = Dvm.ItemBackground;
                NameBackground = Dvm.NameBackground;
                NameFontSize = Dvm.NameFontsize;
                CountFontSize = Dvm.CountFontsize;
                IsCount = Dvm.IsCount;
                IsUnit = Dvm.IsUnit;
                IsRanking = Dvm.IsRanking;
                ImgWidth = Dvm.ImgWidth;
                RankingType = Dvm.RankingType;
                RankingAlign = Dvm.AlignType;
            }
        }

        private void GetRankingInfo(AdapterDataTable adt)
        {
            try
            {
                if (adt != null)
                {
                    if (adt.Rows != null && adt.Rows.Count > 0)
                    {
                        RankingInfoList.Clear();
                        var index = 0;
                        var maxValue = 0.0;

                        try
                        {
                            maxValue = Convert.ToDouble(adt.Rows[0][Dvm.RankingCount.AsName]);
                        }
                        catch (Exception)
                        {
                            maxValue = 0;
                        }

                        foreach (var row in adt.Rows)
                        {
                            var model = new RankingModel();

                            model.VIndex = index;
                            if (Dvm.RankingName != null)
                                model.RankingName = row[Dvm.RankingName.AsName].ToString();
                            if (Dvm.RankingCount != null)
                                model.RankingCount = Convert.ToInt32(row[Dvm.RankingCount.AsName]);
                            if (RankingType == "normal" || RankingType == "unnormal")
                                model.RankingColor = RankingType == "normal"
                                    ? (index >= 3 ? normal[2] : normal[index])
                                    : unnormal[index];
                            else
                            {
                                model.RankingColor = other[index];
                            }
                            model.NameBackground = NameBackground == "0" ? model.RankingColor : "#000000";
                            model.ItemBackground = ItemBackground;
                            model.ItemWidth = ItemWidth;
                            model.ItemMargin = ItemMargin;
                            model.NameFontsize = NameFontSize;
                            model.CountFontsize = CountFontSize;
                            model.IsCount = IsCount;
                            model.Unit = IsUnit;
                            model.Ranking = IsRanking;
                            model.ImgWidth = ImgWidth;
                            model.CountAlign = RankingAlign;
                            model.MaxValue = maxValue;
                            RankingInfoList.Add(model);
                            index += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region property Fields

        private RankingViewModel m_Dvm;

        /// <summary>
        ///     DVM
        /// </summary>
        public RankingViewModel Dvm
        {
            get { return m_Dvm; }
            set
            {
                m_Dvm = value;
                OnPropertyChanged("Dvm");
            }
        }

        private ObservableCollection<RankingModel> m_RankingInfoList = new ObservableCollection<RankingModel>();

        /// <summary>
        ///     List
        /// </summary>
        public ObservableCollection<RankingModel> RankingInfoList
        {
            get { return m_RankingInfoList; }
            set
            {
                m_RankingInfoList = value;
                OnPropertyChanged("RankingInfoList");
            }
        }

        #endregion

        #region Style Fields

        private int m_ControlWidth = 300;

        /// <summary>
        ///     控件宽度
        /// </summary>
        public int ControlWidth
        {
            get { return m_ControlWidth; }
            set
            {
                m_ControlWidth = value;
                OnPropertyChanged("ControlWidth");
            }
        }


        private int m_ControlHeight;

        /// <summary>
        ///     控件高度
        /// </summary>
        public int ControlHeight
        {
            get { return m_ControlHeight; }
            set
            {
                m_ControlHeight = value;
                OnPropertyChanged("ControlHeight");
            }
        }

        private int m_ItemWidth;

        /// <summary>
        ///     项宽度
        /// </summary>
        public int ItemWidth
        {
            get { return m_ItemWidth; }
            set
            {
                m_ItemWidth = value;
                OnPropertyChanged("ItemWidth");
            }
        }

        private string m_ItemMargin;

        /// <summary>
        ///     项Margin
        /// </summary>
        public string ItemMargin
        {
            get { return m_ItemMargin; }
            set
            {
                m_ItemMargin = value;
                OnPropertyChanged("ItemMargin");
            }
        }

        private int m_NameFontSize = 22;

        /// <summary>
        ///     数量字体大小
        /// </summary>
        public int NameFontSize
        {
            get { return m_NameFontSize; }
            set
            {
                m_NameFontSize = value;
                OnPropertyChanged("NameFontSize");
            }
        }

        private int m_CountFontSize = 30;

        /// <summary>
        ///     数量字体大小
        /// </summary>
        public int CountFontSize
        {
            get { return m_CountFontSize; }
            set
            {
                m_CountFontSize = value;
                OnPropertyChanged("CountFontSize");
            }
        }


        private string m_NameBackground = "#7FFFFFFF";

        /// <summary>
        ///     名称背景色
        /// </summary>
        public string NameBackground
        {
            get { return m_NameBackground; }
            set
            {
                m_NameBackground = value;
                OnPropertyChanged("NameBackground");
            }
        }

        private string m_ItemBackground = "#7FFFFFFF";

        /// <summary>
        ///     项背景底色
        /// </summary>
        public string ItemBackground
        {
            get { return m_ItemBackground; }
            set
            {
                m_ItemBackground = value;
                OnPropertyChanged("ItemBackground");
            }
        }

        private Visibility m_IsCount;

        /// <summary>
        ///     数量显示隐藏
        /// </summary>
        public Visibility IsCount
        {
            get { return m_IsCount; }
            set
            {
                m_IsCount = value;
                OnPropertyChanged("IsCount");
            }
        }

        private Visibility m_IsUnit;

        /// <summary>
        ///     单位显示隐藏
        /// </summary>
        public Visibility IsUnit
        {
            get { return m_IsUnit; }
            set
            {
                m_IsUnit = value;
                OnPropertyChanged("IsUnit");
            }
        }

        private Visibility m_IsRanking;

        /// <summary>
        ///     单位显示隐藏
        /// </summary>
        public Visibility IsRanking
        {
            get { return m_IsRanking; }
            set
            {
                m_IsRanking = value;
                OnPropertyChanged("IsRanking");
            }
        }

        private int m_ImgWidth;

        /// <summary>
        ///     排名背景图片宽度
        /// </summary>
        public int ImgWidth
        {
            get { return m_ImgWidth; }
            set
            {
                m_ImgWidth = value;
                OnPropertyChanged("ImgWidth");
            }
        }

        private string m_RankingType;

        /// <summary>
        ///     排名类型
        /// </summary>
        public string RankingType
        {
            get { return m_RankingType; }
            set
            {
                m_RankingType = value;
                OnPropertyChanged("RankingType");
            }
        }

        private string m_RankingAlign;

        /// <summary>
        ///     数值对齐方式
        /// </summary>
        public string RankingAlign
        {
            get { return m_RankingAlign; }
            set
            {
                m_RankingAlign = value;
                OnPropertyChanged("RankingAlign");
            }
        }

        private readonly string[] normal = {"#F5CD29", "#FFFFFF", "#0DE9FB"};

        private readonly string[] unnormal =
        {
            "#004860", "#395366", "#607382", "#BFFFFF", "#FFFF26", "#C0D83F",
            "#248435", "#1BB47A", "#4DD2FF", "#009ACF"
        };

        private readonly string[] other = {"#0DE9FB", "#4DAEFF", "#FF8000", "#F5CD02"};

        #endregion

        #region override Method

        public override void ReceiveData(AdapterDataTable adt)
        {
            GetRankingInfo(adt);
        }


        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        #endregion
    }
}