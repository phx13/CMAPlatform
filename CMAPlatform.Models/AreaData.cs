namespace CMAPlatform.Models
{
    public class AreaData
    {
        public ProvinceData[] provinces { get; set; }
    }

    public class ProvinceData
    {
        public string[] code { get; set; }
        public string provincename { get; set; }
        public CityData[] citys { get; set; }
    }

    public class CityData
    {
        public string[] code { get; set; }
        public string cityname { get; set; }
        public CountyData[] countys { get; set; }
    }

    public class CountyData
    {
        public string[] code { get; set; }
        public string countyname { get; set; }
    }
}