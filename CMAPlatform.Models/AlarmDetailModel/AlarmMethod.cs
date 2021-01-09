namespace CMAPlatform.Models.AlarmDetailModel
{
    public class AlarmMethodModel
    {
        public AlarmMethodInfo[] result { get; set; }
    }

    /// <summary>
    ///     预警手段信息
    /// </summary>
    public class AlarmMethodInfo
    {
        /// <summary>
        ///     预警类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        ///     失败数量
        /// </summary>
        public int failcount { get; set; }

        /// <summary>
        ///     成功数量
        /// </summary>
        public int succcount { get; set; }
    }

    /// <summary>
    ///     预警手段
    /// </summary>
    public class AlarmMethod
    {
        /// <summary>
        ///     短信数量
        /// </summary>
        public int Message { get; set; }

        /// <summary>
        ///     网站数量
        /// </summary>
        public int Web { get; set; }

        /// <summary>
        ///     微博数量
        /// </summary>
        public int WeiBo { get; set; }

        /// <summary>
        ///     微信数量
        /// </summary>
        public int WeChat { get; set; }

        /// <summary>
        ///     邮件数量
        /// </summary>
        public int Mail { get; set; }

        /// <summary>
        ///     传真数量
        /// </summary>
        public int Fax { get; set; }

        /// <summary>
        ///     大喇叭数量
        /// </summary>
        public int Horn { get; set; }

        /// <summary>
        ///     显示屏数量
        /// </summary>
        public int Screen { get; set; }
    }
}