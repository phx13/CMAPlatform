using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CMAPlatform.Chart.DVM;
using CMAPlatform.Models;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.CCP.Helper;
using Digihail.CCP.Models.LauncherMessage;
using Digihail.CCP.UserControls;
using Digihail.CCP.Utilities;
using Digihail.CCPSOE.Models.PageModel;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CMAPlatform.Chart.Controller
{
    public class LayerControlController : ChartControllerBase
    {
        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();


        private readonly CCPPageBaseModel m_PageModel;
        private string m_CurrentLayer;
        private LayerControlDataViewModel m_DVM;

        private ObservableCollection<LayerModel> m_LayerCollection = new ObservableCollection<LayerModel>();

        private Guid m_PageId = Guid.Empty;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public LayerControlController(LayerControlDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_DVM = DataViewModel as LayerControlDataViewModel;

            m_PageModel = CCPHelper.Instance.GetCurrentCCPPageModel();
        }

        /// <summary>
        ///     当前显示图层
        /// </summary>
        public string CurrentLayer
        {
            get { return m_CurrentLayer; }
            set
            {
                m_CurrentLayer = value;
                OnPropertyChanged("CurrentLayer");
            }
        }

        /// <summary>
        ///     DVM
        /// </summary>
        public LayerControlDataViewModel DVM
        {
            get { return m_DVM; }
            set
            {
                m_DVM = value;
                OnPropertyChanged("DVM");
            }
        }

        /// <summary>
        ///     CategoryCollection
        /// </summary>
        public ObservableCollection<LayerModel> LayerCollection
        {
            get { return m_LayerCollection; }
            set
            {
                m_LayerCollection = value;
                OnPropertyChanged("LayerCollection");
            }
        }

        public Guid PageId
        {
            get { return m_PageId; }
            set
            {
                m_PageId = value;
                OnPropertyChanged("PageId");
            }
        }

        public Guid PageInstanceId
        {
            get { return Guid.NewGuid(); }
        }

        public override void ReceiveData(AdapterDataTable adt)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        #region 图层控制控件逻辑

        #region 相关图层

        /// <summary>
        ///     突发事件散点
        /// </summary>
        public readonly List<Guid> EmergengcyPointLayer = new List<Guid>
        {
            Guid.Parse("a347dbfb-4378-44f4-9bf2-4faf4ae61f9a")
        };

        /// <summary>
        ///     突发事件气泡
        /// </summary>
        public readonly List<Guid> EmergengcBubbleLayer = new List<Guid>
        {
            Guid.Parse("3d0c5698-ad2c-4a4b-9bcc-3533df753abf"),
            Guid.Parse("873191c1-88a1-4d10-887e-297fed2e4d70")
        };

        /// <summary>
        ///     预警态势标签
        /// </summary>
        public readonly List<Guid> WarningLabelLayer = new List<Guid>
        {
            Guid.Parse("14a6e7bd-6298-4c4d-940e-2db6b8569b1a")
        };

        /// <summary>
        ///     预警态势落区
        /// </summary>
        public readonly List<Guid> WarningZoneLayer = new List<Guid>
        {
            Guid.Parse("035ecfce-1476-4029-8cb7-318265d5c453")
        };

        /// <summary>
        ///     基础信息
        /// </summary>
        public readonly List<Guid> BasicPoiLayer = new List<Guid>
        {
            Guid.Parse("92b1a90d-b53c-4ad3-a48e-f4f6e974af3d"),
            Guid.Parse("2a9521b2-ce9f-4abf-8122-928ee7eba332"),
            Guid.Parse("45994233-5378-4b77-b96c-ebb7d16209b8"),
            Guid.Parse("3ec86f2a-6770-4b9b-8762-eeae0e35203f")
        };

        /// <summary>
        ///     隐患点
        /// </summary>
        public readonly List<Guid> YinhuandianLayer = new List<Guid>();

        public readonly List<Guid> TextGuids = new List<Guid>
        {
            Guid.Parse("e17153ac-39a8-4c31-80c9-84258e1a21fc"),
            Guid.Parse("e60f829d-8292-4570-a15d-32100d8858ea"),
            Guid.Parse("5eb2d668-feec-43b6-babc-abedbab5ed14")
        };

        #endregion

        /// <summary>
        ///     图层显隐控制 外部自定义控件控制图层或对象显隐
        /// </summary>
        /// <param name="obj">消息对象</param>
        public void LayerVisibilityControl(List<Guid> layerGuids, bool showOrNot)
        {
            var data = new ObjectShowOrHideOneData
            {
                From = m_PageId,
                IsShow = showOrNot,
                DvpObjId = layerGuids,
                LayerGroupName = new List<string>(),
                PageInstanceId = PageInstanceId
            };

            m_MessageAggregator.GetMessage<ObjectShowOrHideOneMessage>().Publish(data);
        }

        public void LayerFilterControl(FrameworkElement fe, string type)
        {
            var commonfilters = m_PageModel.CommonDataFliterCollection;
            if (commonfilters != null)
            {
                var typefilter = commonfilters.FirstOrDefault();
                if (typefilter != null && typefilter.ColumnName == "Type")
                {
                    var selectedvalue = typefilter.Values.FirstOrDefault(t => t.ColumnValue == type);

                    if (selectedvalue != null)
                    {
                        typefilter.Values.ForEach(t => t.IsSelect = false);
                        selectedvalue.IsSelect = true;
                        typefilter.UpdateSelectedValues();
                    }
                }
            }

            var outgrid = fe.GetParentByName<Grid>("outgrid");
            if (outgrid == null)
                return;
            var grid = fe.GetChildByName<Grid>("grid");
            if (grid == null)
                return;
            var ccc = outgrid.GetChildsByType<ComponentControl>();
            foreach (var componentControl in ccc)
            {
                componentControl.ApplyFilter();
            }
        }

        public async void LayerFilter(FrameworkElement fe, string type)
        {
            await Task.Run(() => { LayerFilterControl(fe, type); });
        }

        #endregion
    }
}