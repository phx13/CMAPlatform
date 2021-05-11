using System;
using Digihail.AVE.Launcher.Infrastructure.Communiction;

namespace CMAPlatform.Models.MessageModel
{
    /// <summary>
    ///     事件页突发事件消息
    /// </summary>
    public class CMAEventPageIntoMessage : CompositeCommunictionMessage<EventPageInto>
    {
    }

    [Serializable]
    public class EventPageInto
    {
        /// <summary>
        ///     事件标题
        /// </summary>
        public string EventTitle { get; set; }

        /// <summary>
        ///     事件概要描述
        /// </summary>
        public string EventDescription { get; set; }

        /// <summary>
        ///     事件发生地点
        /// </summary>
        public string EventPlace { get; set; }

        /// <summary>
        ///     事件经度
        /// </summary>
        public double EventLon { get; set; }

        /// <summary>
        ///     事件纬度
        /// </summary>
        public double EventLat { get; set; }

        /// <summary>
        ///     事件开始时间
        /// </summary>
        public string EventBeginTime { get; set; }

        /// <summary>
        ///     事件结束时间
        /// </summary>
        public string EventEndTIme { get; set; }

        /// <summary>
        ///     省区划编码
        /// </summary>
        public string ProvinceCode { get; set; }

        /// <summary>
        ///     市区划编码
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        ///     县区划编码
        /// </summary>
        public string CountryCode { get; set; }
    }
}