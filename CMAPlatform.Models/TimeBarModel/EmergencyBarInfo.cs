using System;

namespace CMAPlatform.Models.TimeBarModel
{
    /// <summary>
    ///     突发事件Timebar信息
    /// </summary>
    public class EmergencyBarInfo
    {
        /// <summary>
        ///     时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     时间
        /// </summary>
        public DateTime BarTime { get; set; }

        /// <summary>
        ///     降水量
        /// </summary>
        public double RainNumber { get; set; }

        /// <summary>
        ///     预警等级
        /// </summary>
        public int AlarmLevel { get; set; }

        /// <summary>
        ///     阵风
        /// </summary>
        public double Wind { get; set; }

        /// <summary>
        ///     阵风风向
        /// </summary>
        public double WindDirection { get; set; }

        /// <summary>
        ///     温度
        /// </summary>
        public double Temperature { get; set; }
    }
}