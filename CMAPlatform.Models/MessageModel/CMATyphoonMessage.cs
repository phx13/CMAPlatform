using System;
using System.Collections.Generic;
using CMAPlatform.Models.Enum;
using Digihail.AVE.Launcher.Infrastructure.Communiction;

namespace CMAPlatform.Models.MessageModel
{
    /// <summary>
    ///     台风消息
    /// </summary>
    public class CMATyphoonMessage : CompositeCommunictionMessage<Typhoon>
    {
    }

    [Serializable]
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
        public bool IsShort { get; set; }

        /// <summary>
        /// 省份代码列表
        /// </summary>
        public List<TyphoonRegionModel> TyphoonRegionModels { get; set; }
    }
}