namespace CMAPlatform.Models.AlarmDetailModel
{
    public class AlarmInformation
    {
        /// <summary>
        ///     发送数量
        /// </summary>
        public int SendNumber { get; set; }

        /// <summary>
        ///     浏览数量
        /// </summary>
        public int BrowseNumber { get; set; }

        /// <summary>
        ///     转发数量
        /// </summary>
        public int RelayNumber { get; set; }
    }
}