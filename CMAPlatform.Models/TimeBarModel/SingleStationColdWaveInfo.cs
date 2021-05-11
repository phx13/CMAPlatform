using System;

namespace CMAPlatform.Models.TimeBarModel
{
    /// <summary>
    ///     单站寒潮信息
    /// </summary>
    public class SingleStationColdWaveInfo
    {
        /// <summary>
        ///     时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     温度值
        /// </summary>
        public double TemValue { get; set; }

        /// <summary>
        ///     温度箭头方向
        /// </summary>
        public double TemAngle { get; set; }

        /// <summary>
        ///     平均风力
        /// </summary>
        public double WindValue { get; set; }

        /// <summary>
        ///     风向
        /// </summary>
        public double WindAngle { get; set; }
    }

    /// <summary>
    ///     单站寒潮信息Json
    /// </summary>
    public class SingleStationColdWaveJson
    {
        /// <summary>
        ///     48小时降水
        /// </summary>
        public string[] data_48 { get; set; }

        /// <summary>
        ///     24小时降水
        /// </summary>
        public string[] data_24 { get; set; }

        /// <summary>
        ///     平均风力
        /// </summary>
        public string[] windSpeed { get; set; }

        /// <summary>
        ///     风向
        /// </summary>
        public string[] windSpeedDirection { get; set; }
    }
}