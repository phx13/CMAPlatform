using System;
using System.Collections.Generic;

namespace CMAPlatform.Models.AlarmDetailModel
{
    /// <summary>
    ///     预警详情类
    /// </summary>
    public class AlarmDetailModel
    {
        /// <summary>
        ///     预警标题
        /// </summary>
        public string AlarmTitle { get; set; }

        /// <summary>
        ///     预警时间
        /// </summary>
        public DateTime AlarmTime { get; set; }

        /// <summary>
        ///     预警类型
        /// </summary>
        public string AlarmType { get; set; }

        /// <summary>
        ///     预警等级
        /// </summary>
        public string AlarmLevel { get; set; }

        /// <summary>
        ///     预警描述
        /// </summary>
        public string AlarmDescription { get; set; }

        /// <summary>
        ///     预警责任人数量
        /// </summary>
        public int AlarmPeopleNumber { get; set; }

        /// <summary>
        ///     信息员
        /// </summary>
        public AlarmInformation AlarmInformation { get; set; }

        /// <summary>
        ///     预警流程
        /// </summary>
        public List<AlarmProcess> AlarmProcess { get; set; }

        /// <summary>
        ///     预警手段
        /// </summary>
        public AlarmMethod AlarmMethod { get; set; }
    }
}