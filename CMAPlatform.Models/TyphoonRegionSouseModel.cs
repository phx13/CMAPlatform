using System;
using System.Collections.Generic;

namespace CMAPlatform.Models
{

    /// <summary>
    /// 台风路径
    /// </summary>
    public class TyphoonModel
    {
        public TyphoonData data { get; set; }
    }

    /// <summary>
    /// 台风落区SourceModel
    /// </summary>
    public class TyphoonRegionSouseModel
    {
        public List<TyphoonRegionModel> souce { get; set; }
    }

    /// <summary>
    /// 台风落区Model
    /// </summary>
    [Serializable]
    public class TyphoonRegionModel
    {
        /// <summary>
        /// 等级（红橙黄蓝）
        /// </summary>
        public string severity { get; set; }

        /// <summary>
        /// 落区Code码
        /// </summary>
        public string code { get; set; }
    }
}