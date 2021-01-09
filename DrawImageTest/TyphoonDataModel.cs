namespace DrawImageTest
{

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
        public TyphoonDataModel[] typhoon_live { get; set; }
    }

    public class TyphoonDataModel
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

    public class Typhoon
    {
        public double CenterLon { get; set; }
        public double CenterLat { get; set; }
        public double YellowRadius1 { get; set; }
        public double YellowRadius2 { get; set; }
        public double YellowRadius3 { get; set; }
        public double YellowRadius4 { get; set; }
        public double RedRadius1 { get; set; }
        public double RedRadius2 { get; set; }
        public double RedRadius3 { get; set; }
        public double RedRadius4 { get; set; }
        //public Dictionary<double[], string> Playbacks { get; set; }

        /// <summary>
        /// 台风强度
        /// </summary>
        public string TyphoonIntensity { get; set; }
    }
}