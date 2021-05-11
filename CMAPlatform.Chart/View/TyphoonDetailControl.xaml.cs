using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CMAPlatform.Chart.Controller;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     TyphoonDetailControl.xaml 的交互逻辑
    /// </summary>
    public partial class TyphoonDetailControl : ChartViewBase
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public TyphoonDetailControl(ChartViewBaseModel model) : base(model)
        {
            InitializeComponent();

            Loaded += TyphoonDetailControl_Loaded;

            m_Controller = Controllers[0] as TyphoonDetailController;
            DataContext = m_Controller;
        }

        #endregion

        #region 成员变量

        /// <summary>
        ///     消息聚合器
        /// </summary>
        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly TyphoonDetailController m_Controller;

        #endregion

        #region 私有方法

        private void TyphoonDetailControl_Loaded(object sender, RoutedEventArgs e)
        {
            m_MessageAggregator.GetMessage<CMAScutcheonSelectGroupMessage>()
                .Subscribe(ReceiveCMAScutcheonSelectGroupMessage);
        }

        // 接收页面传来的告警消息
        private void ReceiveCMAScutcheonSelectGroupMessage(ScutcheonSelectGroupMessage obj)
        {
            // 判断
            if (obj == null) return;
            if (obj.typeName != "11B01") return;
            var typhoonDataList = new List<TyphoonDataInfo>();
            if (obj.id.Length == 4)
            {
                typhoonDataList = DataManager.TyphoonData(obj.id); //接口拉一下台风数据
            }
            else
            {
                typhoonDataList = DataManager.TyphoonData("1909"); //接口拉一下台风数据
            }
            //var typhoonDataList = DataManager.TyphoonData("1909");//接口拉一下台风数据

            if (typhoonDataList == null || !typhoonDataList.Any()) return;

            var typhoon = typhoonDataList.OrderByDescending(s => DateTime.Parse(s.bj_datetime)).First();
            m_Controller.TyphoonData = new TyphoonDataInfoModel
            {
                Name = typhoon.typhoon_name_cn,
                Id = typhoon.typhoon_id,
                Lon = typhoon.lon,
                Lat = typhoon.lat,
                Time = typhoon.bj_datetime,
                Intension = typhoon.trank,
                Speed = typhoon.windspeed,
                Pressure = typhoon.pressure,
                Gust = typhoon.gust,
                Direction = typhoon.direction
            };
            txtSpeed.Text = typhoon.windspeed + "米/秒";
            txtPressure.Text = typhoon.pressure + "百帕";
            if (typhoon.gust == null || double.IsNaN(double.Parse(typhoon.gust)))
            {
                txtGust.Text = typhoon.direction;
            }
            else
            {
                txtGust.Text = typhoon.gust + "公里/小时" + "  " + typhoon.direction;
            }
        }

        #endregion

        #region 重写基类方法

        public override void RefreshStyle()
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }

        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        public override void ExportChart(ExportType type)
        {
        }

        #endregion
    }
}