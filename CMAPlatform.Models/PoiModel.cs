namespace CMAPlatform.Models
{

    #region  接口类

    public class POIStatistics
    {
        public Country[] country { get; set; }
        public Province[] province { get; set; }
        public City[] city { get; set; }
    }

    /// <summary>
    ///     省
    /// </summary>
    public class Province
    {
        public Coord coord { get; set; }
        public string code { get; set; }
        public PoiCount poiCount { get; set; }
    }

    /// <summary>
    ///     市
    /// </summary>
    public class City
    {
        public Coord coord { get; set; }
        public string code { get; set; }
        public PoiCount poiCount { get; set; }
    }

    /// <summary>
    ///     县
    /// </summary>
    public class Country
    {
        public Coord coord { get; set; }
        public string code { get; set; }
        public PoiCount poiCount { get; set; }
    }

    public class Coord
    {
        public string lon { get; set; }
        public string lat { get; set; }
    }

    public class PoiCount
    {
        public int Airport { get; set; }
        public int CarService { get; set; }
        public int JYZ { get; set; }
        public int RailwayStation { get; set; }
        public int automaticStation { get; set; }
        public int caipiao { get; set; }
        public int canyin { get; set; }
        public int daolujiebing { get; set; }
        public int dasha { get; set; }
        public int dizhizaihai { get; set; }
        public int fog { get; set; }
        public int gaosuService { get; set; }
        public int gonganjiaojing { get; set; }
        public int gonglujixue { get; set; }
        public int heliu { get; set; }
        public int huapo { get; set; }
        public int jiaotongchuxing { get; set; }
        public int jinrongfuwu { get; set; }
        public int keyanjiaoyu { get; set; }
        public int laboratory { get; set; }
        public int lvyou { get; set; }
        public int messager { get; set; }
        public int nishiliu { get; set; }
        public int parkinglot { get; set; }
        public int port { get; set; }
        public int qiangjiangshui { get; set; }
        public int qiaoliang { get; set; }
        public int qichezhan { get; set; }
        public int qixiangStation { get; set; }
        public int qixiangStation_new { get; set; }
        public int river { get; set; }
        public int shanfeng { get; set; }
        public int shanhong { get; set; }
        public int shelter { get; set; }
        public int shoufeizhan { get; set; }
        public int shuiku { get; set; }
        public int tailings { get; set; }
        public int torrent { get; set; }
        public int town { get; set; }
        public int tuanwu { get; set; }
        public int village { get; set; }
        public int xinxiyuan { get; set; }
        public int xiuxianyule { get; set; }
        public int yiliaofuwu { get; set; }
        public int yinhuandian { get; set; }
        public int yujingsheshi { get; set; }
        public int zhengfujiguan { get; set; }
        public int zhongxiaoheliu { get; set; }
        public int zhusu { get; set; }
    }

    #endregion

    #region 可视化类

    public class PoiHeat
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string Code { get; set; }
        public string Level { get; set; }
        public int Count { get; set; }
    }

    public class PoiData
    {
        public string Id { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
    }


    public class PoiTypeCount
    {
        public int Id { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Level { get; set; }
        public int Count { get; set; }
    }

    #endregion
}