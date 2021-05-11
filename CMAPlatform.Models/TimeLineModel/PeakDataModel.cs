namespace CMAPlatform.Models.TimeLineModel
{
    /// <summary>
    ///     气象极值返回Model
    /// </summary>
    public class PeakDataModel
    {
        /// <summary>
        ///     查询省份区划代码
        /// </summary>
        public string adminCodeChn { get; set; }

        /// <summary>
        ///     未启用
        /// </summary>
        public string city { get; set; }

        /// <summary>
        ///     未启用
        /// </summary>
        public string cnty { get; set; }

        /// <summary>
        ///     气象极值信息集合
        /// </summary>
        public PeakDataInfo[] data { get; set; }

        /// <summary>
        ///     查询极值类型
        /// </summary>
        public string peakName { get; set; }

        /// <summary>
        ///     查询省份
        /// </summary>
        public string province { get; set; }
    }

    /// <summary>
    ///     气象极值信息
    /// </summary>
    public class PeakDataInfo
    {
        /// <summary>
        ///     时间
        /// </summary>
        public string datetime { get; set; }

        /// <summary>
        ///     极值
        /// </summary>
        public float peakValue { get; set; }

        /// <summary>
        ///     气象站ID
        /// </summary>
        public string stationId { get; set; }

        /// <summary>
        ///     气象站名称
        /// </summary>
        public string stationName { get; set; }
    }
}