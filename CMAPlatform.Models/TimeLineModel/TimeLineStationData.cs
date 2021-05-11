namespace CMAPlatform.Models.TimeLineModel
{
    /// <summary>
    ///     气象局类： 泳道图预警站弹窗Json数据
    /// </summary>
    public class TimeLineStationDataRoot
    {
        public TimeLineStationData[] blue { get; set; }
        public TimeLineStationData[] yellow { get; set; }
        public TimeLineStationData[] red { get; set; }
        public TimeLineStationData[] orange { get; set; }
    }

    /// <summary>
    ///     气象局类： 泳道图预警站弹窗Json数据
    /// </summary>
    public class TimeLineStationData
    {
        public string StationName { get; set; }
        public string StationId { get; set; }
        public string Value { get; set; }
    }
}