namespace CMAPlatform.Models.TimeLineModel
{
    /// <summary>
    ///     气象局类： 泳道图服务Json数据
    /// </summary>
    public class TimeLineServiceSource
    {
        public TimeLineService[] services { get; set; }
    }

    /// <summary>
    ///     气象局类： 泳道图服务Json数据
    /// </summary>
    public class TimeLineService
    {
        public string date { get; set; }
        public string unit { get; set; }
        public string cdate { get; set; }
        public string serverurl { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string url { get; set; }
    }
}