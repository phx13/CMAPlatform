using System.Collections.Generic;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     舆情热点
    /// </summary>
    public class RootobjectModel
    {
        public List<Property> Property { get; set; }
    }

    public class Property
    {
        public int eventid { get; set; }
        public int quantity { get; set; }
        public string persent { get; set; }
        public string simplename { get; set; }
        public string eventname { get; set; }
    }
}