namespace CMAPlatform.Models
{
    public class EventPageMessageModel
    {
        /// <summary>
        ///     id
        /// </summary>
        public string identifier { get; set; }

        /// <summary>
        ///     发布单位名称
        /// </summary>
        public string sender { get; set; }

        /// <summary>
        ///     发布单位编码
        /// </summary>
        public string senderCode { get; set; }

        /// <summary>
        ///     预警时间
        /// </summary>
        public string sendTime { get; set; }

        /// <summary>
        ///     生效时间
        /// </summary>
        public string effective { get; set; }

        /// <summary>
        ///     预警事件类型
        /// </summary>
        public string msgType { get; set; }

        /// <summary>
        ///     预警类别
        /// </summary>
        public string eventType { get; set; }

        /// <summary>
        ///     预警级别
        /// </summary>
        public string severity { get; set; }

        /// <summary>
        ///     预警标题
        /// </summary>
        public string headline { get; set; }

        /// <summary>
        ///     预警内容
        /// </summary>
        public string description { get; set; }
    }
}