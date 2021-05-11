namespace CMAPlatform.Models.TimeLineModel
{
    /// <summary>
    ///     气象局类： 泳道图预警站数统计Json数据
    /// </summary>
    public class TimeLineWarningCountRoot
    {
        public TimeLineWarningCount[] WarningsCount { get; set; }
    }

    /// <summary>
    ///     气象局类： 泳道图预警站数统计Json数据
    /// </summary>
    public class TimeLineWarningCount
    {
        public int YellowNumber { get; set; }
        public string StationsYellow { get; set; }
        public string StationsBlue { get; set; }
        public int RedNumber { get; set; }
        public int OrangeNumber { get; set; }
        public int BlueNumber { get; set; }
        public string StationsOrange { get; set; }
        public string Time { get; set; }
        public string StationsRed { get; set; }
    }
}