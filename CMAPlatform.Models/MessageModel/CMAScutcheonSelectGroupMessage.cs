using System;
using Digihail.AVE.Launcher.Infrastructure.Communiction;

namespace CMAPlatform.Models.MessageModel
{
    /// <summary>
    ///     主页点击预警进事件页消息
    /// </summary>
    public class CMAScutcheonSelectGroupMessage : CompositeCommunictionMessage<ScutcheonSelectGroupMessage>
    {
    }

    /// <summary>
    ///     主页点击预警进事件页消息体
    /// </summary>
    [Serializable]
    public class ScutcheonSelectGroupMessage
    {
        /// <summary>
        ///     id 字段 标牌唯一id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        ///     行政区码
        /// </summary>
        public string areaCode { get; set; }

        /// <summary>
        ///     纬度
        /// </summary>
        public double lat { get; set; }

        /// <summary>
        ///     经度
        /// </summary>
        public double lon { get; set; }

        /// <summary>
        ///     类型名称
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     类型编码
        /// </summary>
        public string typeName { get; set; }

        /// <summary>
        ///     预警颜色
        /// </summary>
        public string colour { get; set; }

        /// <summary>
        ///     预警时间
        /// </summary>
        public string DateTime { get; set; }

        /// <summary>
        /// 台风名称
        /// </summary>
        public string TyphoonName { get; set; }
    }

    /// <summary>
    ///     主页点击预警标牌置顶预警列表消息
    /// </summary>
    public class CMAClickWarningBillboardMessage : CompositeCommunictionMessage<string>
    {
    }
}