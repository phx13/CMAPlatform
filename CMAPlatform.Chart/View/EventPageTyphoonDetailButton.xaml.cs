using System.Collections.Generic;
using System.Windows.Input;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CMAPlatform.Chart.View
{
    /// <summary>
    ///     EventPageTyphoonDetailButton.xaml 的交互逻辑
    /// </summary>
    public partial class EventPageTyphoonDetailButton : ChartViewBase
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public EventPageTyphoonDetailButton(ChartViewBaseModel model) : base(model)
        {
            InitializeComponent();
        }

        #endregion

        #region 控件事件

        // 弹出台风详情按钮事件
        private void OpenTyphoonDetail(object sender, MouseButtonEventArgs e)
        {
            //var typhoonDetailWindow = new TyphoonDetailWindow
            //{
            //    WindowStartupLocation = WindowStartupLocation.CenterScreen
            //};
            //typhoonDetailWindow.ShowDialog();
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