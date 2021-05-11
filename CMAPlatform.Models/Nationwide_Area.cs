namespace CMAPlatform.Models
{
    /// <summary>
    ///     全国省市县信息
    /// </summary>
    public class Nationwide_Area
    {
        public string id { get; set; }

        public string code { get; set; }

        public string region_name { get; set; }
        public string region_type { get; set; }
        public string parent_code { get; set; }
        public string parent_name { get; set; }
    }

    public class ProvinceArea
    {
        public string provinceCode { get; set; }
        public string provinceName { get; set; }
    }

    public class CityArea
    {
        public string cityCode { get; set; }
        public string cityName { get; set; }
    }

    public class CountyArea
    {
        public string countyCode { get; set; }
        public string countyName { get; set; }
    }
}