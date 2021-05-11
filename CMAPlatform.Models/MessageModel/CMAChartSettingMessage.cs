using System;
using CMAPlatform.Models.Enum;
using Digihail.AVE.Launcher.Infrastructure.Communiction;

namespace CMAPlatform.Models.MessageModel
{
    /// <summary>
    /// 泳道图图表控件高度设置消息
    /// </summary>
    public class CMAChartSettingMessage : CompositeCommunictionMessage<ChartSettingParameter>
    {
    }

    [Serializable]
    public class ChartSettingParameter
    {
        /// <summary>
        /// 图表名称
        /// </summary>
        public ChartNameEnum ChartName { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public double MinimumValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaximumValue { get; set; }
    }
}