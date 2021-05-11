namespace CMAPlatform.Models.TimeLineModel
{
    /// <summary>
    ///     极值请求参数类
    /// </summary>
    public class PeakDataParamsModel
    {
        /// <summary>
        ///     区域代码（预警详情中senderCode前六位）
        /// </summary>
        public string adminCodeChn { get; set; }

        /// <summary>
        ///     最大值类型
        /// </summary>
        public string peakName { get; set; }

        /// <summary>
        ///     开始时间（Simple:2019-05-0314:00:00）
        /// </summary>
        public string startTime { get; set; }

        /// <summary>
        ///     结束时间
        /// </summary>
        public string endTime { get; set; }
    }
}