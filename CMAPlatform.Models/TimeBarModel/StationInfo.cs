namespace CMAPlatform.Models.TimeBarModel
{
    /// <summary>
    ///     观测站类
    /// </summary>
    public class StationInfo
    {
        /// <summary>
        ///     观测站名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        ///     观测站id
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        ///     观测站数值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     事件类型
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        ///     告警等级
        /// </summary>
        public string EventColor { get; set; }
    }
}