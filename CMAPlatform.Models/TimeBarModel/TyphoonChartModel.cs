using System;

namespace CMAPlatform.Models.TimeBarModel
{
    /// <summary>
    /// 台风图表数据Model
    /// </summary>
    public class TyphoonChartModel
    {
        /// <summary>
        /// 风速
        /// </summary>
        public double WindSpeed { get; set; }

        /// <summary>
        /// 气压
        /// </summary>
        public int Pressure { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 柱图时间
        /// </summary>
        public DateTime BarTime { get; set; }
    }
}