namespace CMAPlatform.Models.TimeLineModel
{
    /// <summary>
    ///     预警详细弹窗 - 信息员
    /// </summary>
    public class TimeLineWarningDetailPeopleInfoModel
    {
        /// <summary>
        ///     信息员数据
        /// </summary>
        public PeopleInfo[] data { get; set; }

        /// <summary>
        ///     结果信息
        /// </summary>
        public string errmgs { get; set; }

        /// <summary>
        ///     结果代码
        /// </summary>
        public string errcode { get; set; }
    }

    /// <summary>
    ///     信息员数据类
    /// </summary>
    public class PeopleInfo
    {
        /// <summary>
        ///     浏览人数
        /// </summary>
        public string read { get; set; }

        /// <summary>
        ///     转发人数
        /// </summary>
        public string send { get; set; }

        /// <summary>
        ///     发送组织人数
        /// </summary>
        public string org { get; set; }

        /// <summary>
        ///     几乎率（未使用）
        /// </summary>
        public string jhl { get; set; }

        /// <summary>
        ///     活跃度（未使用）
        /// </summary>
        public string hyd { get; set; }
    }
}