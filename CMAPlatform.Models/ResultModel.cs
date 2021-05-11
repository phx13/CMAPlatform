using System.Collections.Generic;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     接口错误消息体
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        ///     返回结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        ///     错误详情
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     首页_突发事件
        /// </summary>
        public EventListModel EventListModel { get; set; }

        /// <summary>
        ///     首页_风险态势
        /// </summary>
        public List<RiskSituationModel> RiskSituationModels { get; set; }

        /// <summary>
        ///     首页_极端天气
        /// </summary>
        public List<ExemtremeWeatherModel> ExemtremeWeatherModels { get; set; }
    }
}