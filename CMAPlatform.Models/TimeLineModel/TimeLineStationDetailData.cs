namespace CMAPlatform.Models.TimeLineModel
{
    /// <summary>
    ///     气象局类： 单站点详细数据
    /// </summary>
    public class TimeLineStationDetailDataRoot
    {
        public string[] pre_1h { get; set; }
        public string[] pre_3h { get; set; }
        public string[] pre_6h { get; set; }
        public string[] pre_12h { get; set; }
        public string[] pre_24h { get; set; }
    }
}