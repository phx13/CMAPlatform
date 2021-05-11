namespace CMAPlatform.Models.TimeLineModel
{
    /// <summary>
    ///     气象局类： 泳道图预警Json数据
    /// </summary>
    public class TimeLineWarningSource
    {
        public TimeLineWarningData[] souce { get; set; }
    }

    /// <summary>
    ///     气象局类： 泳道图预警Json数据
    /// </summary>
    public class TimeLineWarningData
    {
        public string severity { get; set; }
        public string identifier { get; set; }
        public string sender { get; set; }
        public string eventType { get; set; }
        public string senderCode { get; set; }
        public string headline { get; set; }
        public string sendTime { get; set; }
        public string level { get; set; }
    }
}