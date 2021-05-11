namespace CMAPlatform.Models.TimeBarModel
{
    /// <summary>
    ///     突发事件json
    /// </summary>
    public class EmergencyBarInfoRoot
    {
        public EmergencyBarInfoJson[] liveDataList { get; set; }
    }

    public class EmergencyBarInfoJson
    {
        /// <summary>
        ///     站点名称
        /// </summary>
        public string stationName { get; set; }

        /// <summary>
        ///     温度
        /// </summary>
        public string tEM { get; set; }

        /// <summary>
        ///     降雨
        /// </summary>
        public string pRE_1h { get; set; }

        /// <summary>
        ///     风向
        /// </summary>
        public string wIN_D_Avg_10mi { get; set; }

        /// <summary>
        ///     风速
        /// </summary>
        public string wIN_S_Avg_10mi { get; set; }

        /// <summary>
        ///     时间
        /// </summary>
        public string nowTime { get; set; }
    }
}