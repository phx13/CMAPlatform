using System;

namespace CMAPlatform.Models.AlarmDetailModel
{
    /// <summary>
    ///     预警流程Model
    /// </summary>
    public class AlarmProcessModel
    {
        public AlarmProcess[] result { get; set; }
    }

    /// <summary>
    ///     流程单个步骤Model
    /// </summary>
    public class AlarmProcess
    {
        /// <summary>
        ///     行为
        /// </summary>
        public string action { get; set; }

        /// <summary>
        ///     操作者
        /// </summary>
        public string @operator { get; set; }

        /// <summary>
        ///     操作时间
        /// </summary>
        public DateTime time { get; set; }
    }
}