namespace CMAPlatform.Models
{
    /// <summary>
    ///     台风事件
    /// </summary>
    public class TyphoonEventsModel
    {
        /// <summary>
        ///     台风编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        ///     台风名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     台风英文名
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        ///     开始时间
        /// </summary>
        public string StartTime { get; set; }
    }

    public class TyphoonPortModel
    {
        public Source[] data { get; set; }
    }

    public class Source
    {
        public string typhoon_name { get; set; }
        public string bj_datetime { get; set; }
        public string typhoon_name_cn { get; set; }
        public string typhoon_id { get; set; }
    }

    /// <summary>
    /// 台风路径
    /// </summary>
    public class TyphoonPathModel
    {
        public TyphoonData data { get; set; }
    }

    /// <summary>
    /// 台风路径集合
    /// </summary>
    public class TyphoonData
    {
        public TyphoonDataInfo[] typhoon_live { get; set; }
    }

    /// <summary>
    /// 台风路径信息
    /// </summary>
    public class TyphoonDataInfo
    {
        /// <summary>
        /// 西北10，四象限
        /// </summary>
        public string wn10radii { get; set; }
        /// <summary>
        /// 东北10，一象限
        /// </summary>
        public string en10radii { get; set; }
        /// <summary>
        /// 西南10，三象限
        /// </summary>
        public string ws10radii { get; set; }
        /// <summary>
        /// 东南10，二象限
        /// </summary>
        public string es10radii { get; set; }
        /// <summary>
        /// 西北7，四象限
        /// </summary>
        public string wn7radii { get; set; }
        /// <summary>
        /// 东北7，一象限
        /// </summary>
        public string en7radii { get; set; }
        /// <summary>
        /// 西南7，三象限
        /// </summary>
        public string ws7radii { get; set; }
        /// <summary>
        /// 东南7，二象限
        /// </summary>
        public string es7radii { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string lon { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 强度，对应颜色
        /// </summary>
        public string trank { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 台风id
        /// </summary>
        public string typhoon_id { get; set; }
        /// <summary>
        /// 台风名字
        /// </summary>
        public string typhoon_name_cn { get; set; }
        /// <summary>
        /// 台风英文名
        /// </summary>
        public string typhoon_name { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        public string bj_datetime { get; set; }
        /// <summary>
        /// 风力风速
        /// </summary>
        public string windspeed { get; set; }
        /// <summary>
        /// 帕斯卡
        /// </summary>
        public string pressure { get; set; }
        /// <summary>
        /// 未来风速
        /// </summary>
        public string gust { get; set; }
        /// <summary>
        /// 未来风向
        /// </summary>
        public string direction { get; set; }

        /// <summary>
        /// 结论
        /// </summary>
        public string conclusion { get; set; }
    }

    /// <summary>
    /// 预警台风路径
    /// </summary>
    public class WarningTyphoonPathModel
    {
        public WarningTyphoonData[] data { get; set; }
    }

    /// <summary>
    /// 台风路径集合
    /// </summary>
    public class WarningTyphoonData
    {
        public WarningTyphoonDataInfo[] path { get; set; }
        public string id { get; set; }
    }

    /// <summary>
    /// 台风路径信息
    /// </summary>
    public class WarningTyphoonDataInfo
    {
        /// <summary>
        /// 经度
        /// </summary>
        public string lon { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 强度，对应颜色
        /// </summary>
        public string trank { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        public string bj_datetime { get; set; }
    }
}