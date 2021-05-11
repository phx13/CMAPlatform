using System;

namespace CMAPlatform.Models.TimeBarModel
{
    public class SingleStationWindInfo
    {
        /// <summary>
        ///     时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     阵风
        /// </summary>
        public double Wind { get; set; }

        /// <summary>
        ///     阵风风向角
        /// </summary>
        public double WindAngle { get; set; }

        /// <summary>
        ///     平均风力
        /// </summary>
        public double AvrWind { get; set; }

        /// <summary>
        ///     平均风力风向角
        /// </summary>
        public double AvrWindAngle { get; set; }
    }

    public class SingleStationWindJson
    {
        /// <summary>
        ///     阵风
        /// </summary>
        public string[] maxWindSpeed { get; set; }

        /// <summary>
        ///     阵风风向角
        /// </summary>
        public string[] maxWindSpeedDirection { get; set; }

        /// <summary>
        ///     平均风力
        /// </summary>
        public string[] avgWindSpeed { get; set; }

        /// <summary>
        ///     平均风力风向角
        /// </summary>
        public string[] avgWindSpeedDirection { get; set; }
    }
}