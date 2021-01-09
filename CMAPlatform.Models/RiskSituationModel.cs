using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models
{
    /// <summary>
    ///     风险态势
    /// </summary>
    public class RiskSituationModel : NotificationObject
    {
        private int m_AreacodesCount;

        private int m_Blue;

        private colorCountObj m_ColorCount;

        private string m_ColourCount;

        private List<GroupWarning> m_GroupWarning;

        private bool m_IsCheck;

        private bool m_IsCheck2;

        private double m_Latitude;

        private double m_Longitude;

        private int m_OrangeCount;

        private int m_OrderId;

        private int m_RedCount;

        private int m_WarningCount;

        private string m_WarningLevel;

        private List<Warnings> m_Warnings;

        private string m_WarningType;

        private int m_Yellow;

        private Zonedata m_ZoneData;

        private string m_ZoneDataString;
        private string m_ZoneId;

        /// <summary>
        ///     落区ID
        /// </summary>
        public string ZoneId
        {
            get { return m_ZoneId; }
            set
            {
                m_ZoneId = value;
                RaisePropertyChanged(() => ZoneId);
            }
        }

        /// <summary>
        ///     是否选中
        /// </summary>
        public bool IsCheck
        {
            get { return m_IsCheck; }
            set
            {
                m_IsCheck = value;
                RaisePropertyChanged(() => IsCheck);
            }
        }

        /// <summary>
        ///     是否选中
        /// </summary>
        public bool IsCheck2
        {
            get { return m_IsCheck2; }
            set
            {
                m_IsCheck2 = value;
                RaisePropertyChanged(() => IsCheck2);
            }
        }

        /// <summary>
        ///     各颜色预警数量
        /// </summary>
        public string ColourCount
        {
            get { return m_ColourCount; }
            set
            {
                m_ColourCount = value;
                RaisePropertyChanged(() => ColourCount);
            }
        }

        /// <summary>
        ///     预警类型
        /// </summary>
        public string WarningType
        {
            get { return m_WarningType; }
            set
            {
                m_WarningType = value;
                RaisePropertyChanged(() => WarningType);
            }
        }

        /// <summary>
        ///     预警级别
        /// </summary>
        public string WarningLevel
        {
            get { return m_WarningLevel; }
            set
            {
                m_WarningLevel = value;
                RaisePropertyChanged(() => WarningLevel);
            }
        }

        /// <summary>
        ///     预警数量
        /// </summary>
        public int WarningCount
        {
            get { return m_WarningCount; }
            set
            {
                m_WarningCount = value;
                RaisePropertyChanged(() => WarningCount);
            }
        }

        /// <summary>
        ///     红色预警数量
        /// </summary>
        public int RedCount
        {
            get { return m_RedCount; }
            set
            {
                m_RedCount = value;
                RaisePropertyChanged(() => RedCount);
            }
        }

        /// <summary>
        ///     总计预警数量
        /// </summary>
        public int AreacodesCount
        {
            get { return m_AreacodesCount; }
            set
            {
                m_AreacodesCount = value;
                RaisePropertyChanged(() => AreacodesCount);
            }
        }

        /// <summary>
        ///     橙色预警数量
        /// </summary>
        public int OrangeCount
        {
            get { return m_OrangeCount; }
            set
            {
                m_OrangeCount = value;
                RaisePropertyChanged(() => OrangeCount);
            }
        }

        /// <summary>
        ///     蓝色预警数量
        /// </summary>
        public int Blue
        {
            get { return m_Blue; }
            set
            {
                m_Blue = value;
                RaisePropertyChanged(() => Blue);
            }
        }

        /// <summary>
        ///     蓝色预警数量
        /// </summary>
        public int Yellow
        {
            get { return m_Yellow; }
            set
            {
                m_Yellow = value;
                RaisePropertyChanged(() => Yellow);
            }
        }

        /// <summary>
        ///     落区中心经度
        /// </summary>
        public double Longitude
        {
            get { return m_Longitude; }
            set
            {
                m_Longitude = value;
                RaisePropertyChanged(() => Longitude);
            }
        }

        /// <summary>
        ///     落区中心纬度
        /// </summary>
        public double Latitude
        {
            get { return m_Latitude; }
            set
            {
                m_Latitude = value;
                RaisePropertyChanged(() => Latitude);
            }
        }

        /// <summary>
        ///     落区数据
        /// </summary>
        public Zonedata ZoneData
        {
            get { return m_ZoneData; }
            set
            {
                m_ZoneData = value;
                RaisePropertyChanged(() => ZoneData);
            }
        }

        /// <summary>
        ///     落区数据
        /// </summary>
        public string ZoneDataString
        {
            get { return m_ZoneDataString; }
            set
            {
                m_ZoneDataString = value;
                RaisePropertyChanged(() => ZoneDataString);
            }
        }

        /// <summary>
        ///     告警颜色数量
        /// </summary>
        public colorCountObj ColorCount
        {
            get { return m_ColorCount; }
            set
            {
                m_ColorCount = value;
                RaisePropertyChanged(() => ColorCount);
            }
        }

        /// <summary>
        ///     预警
        /// </summary>
        public List<Warnings> Warnings
        {
            get { return m_Warnings; }
            set
            {
                m_Warnings = value;
                RaisePropertyChanged(() => Warnings);
            }
        }

        public int OrderId
        {
            get { return m_OrderId; }
            set
            {
                m_OrderId = value;
                RaisePropertyChanged(() => OrderId);
            }
        }

        public List<GroupWarning> GroupWarning
        {
            get { return m_GroupWarning; }
            set
            {
                m_GroupWarning = value;
                RaisePropertyChanged(() => GroupWarning);
            }
        }
    }

    public class GroupWarning : NotificationObject
    {
        private string m_aid;
        private List<colorCountObj> m_Colourcount = new List<colorCountObj>();
        private List<string> m_id = new List<string>();

        private bool m_IsCheck;
        private List<Locations> m_Location = new List<Locations>();

        private int m_OrderId;

        private List<string> m_WarningLevel = new List<string>();

        private List<Warnings> m_Warnings = new List<Warnings>();

        private List<Zonedata> m_ZoneData = new List<Zonedata>();

        /// <summary>
        ///     key值
        /// </summary>
        public List<string> id
        {
            get { return m_id; }
            set
            {
                m_id = value;
                RaisePropertyChanged(() => id);
            }
        }

        public int OrderId
        {
            get { return m_OrderId; }
            set
            {
                m_OrderId = value;
                RaisePropertyChanged(() => OrderId);
            }
        }

        /// <summary>
        ///     key值
        /// </summary>
        public string aid
        {
            get { return m_aid; }
            set
            {
                m_aid = value;
                RaisePropertyChanged(() => aid);
            }
        }

        /// <summary>
        ///     是否选中
        /// </summary>
        public bool IsCheck
        {
            get { return m_IsCheck; }
            set
            {
                m_IsCheck = value;
                RaisePropertyChanged(() => IsCheck);
            }
        }

        /// <summary>
        ///     预警
        /// </summary>
        public List<Warnings> Warnings
        {
            get { return m_Warnings; }
            set
            {
                m_Warnings = value;
                RaisePropertyChanged(() => Warnings);
            }
        }

        /// <summary>
        ///     落区数据
        /// </summary>
        public List<Zonedata> ZoneData
        {
            get { return m_ZoneData; }
            set
            {
                m_ZoneData = value;
                RaisePropertyChanged(() => ZoneData);
            }
        }

        /// <summary>
        ///     预警级别
        /// </summary>
        public List<string> WarningLevel
        {
            get { return m_WarningLevel; }
            set
            {
                m_WarningLevel = value;
                RaisePropertyChanged(() => WarningLevel);
            }
        }

        public List<Locations> Location
        {
            get { return m_Location; }
            set
            {
                m_Location = value;
                RaisePropertyChanged(() => Location);
            }
        }

        public List<colorCountObj> Colourcount
        {
            get { return m_Colourcount; }
            set
            {
                m_Colourcount = value;
                RaisePropertyChanged(() => Colourcount);
            }
        }
    }

    public class Locations : NotificationObject
    {
        private double m_Latitude;
        private double m_Longitude;

        /// <summary>
        ///     落区中心经度
        /// </summary>
        public double Longitude
        {
            get { return m_Longitude; }
            set
            {
                m_Longitude = value;
                RaisePropertyChanged(() => Longitude);
            }
        }

        /// <summary>
        ///     落区中心纬度
        /// </summary>
        public double Latitude
        {
            get { return m_Latitude; }
            set
            {
                m_Latitude = value;
                RaisePropertyChanged(() => Latitude);
            }
        }
    }

    /// <summary>
    ///     落区数据
    /// </summary>
    public class ZoneData : NotificationObject
    {
        private double m_X;

        private double m_Y;

        /// <summary>
        ///     落区点经度
        /// </summary>
        public double X
        {
            get { return m_X; }
            set
            {
                m_X = value;
                RaisePropertyChanged(() => X);
            }
        }

        /// <summary>
        ///     落区点经度
        /// </summary>
        public double Y
        {
            get { return m_Y; }
            set
            {
                m_Y = value;
                RaisePropertyChanged(() => Y);
            }
        }
    }

    /// <summary>
    ///     预警信息
    /// </summary>
    public class Warnings : NotificationObject
    {
        private string m_areaCode;
        private string m_ID;

        private double m_Lat;

        private double m_Lon;

        private string m_Station;

        private string m_StationLevel;

        private string m_Time;

        private string m_WarningLevel;

        private string m_WarningState;

        private string m_WarningType;

        private string m_WarningTypeName;

        /// <summary>
        ///     预警唯一标识
        /// </summary>
        public string ID
        {
            get { return m_ID; }
            set
            {
                m_ID = value;
                RaisePropertyChanged(() => ID);
            }
        }

        /// <summary>
        ///     预警发布站点
        /// </summary>
        public string Station
        {
            get { return m_Station; }
            set
            {
                m_Station = value;
                RaisePropertyChanged(() => Station);
            }
        }

        /// <summary>
        ///     站点预警级别
        /// </summary>
        public string StationLevel
        {
            get { return m_StationLevel; }
            set
            {
                m_StationLevel = value;
                RaisePropertyChanged(() => StationLevel);
            }
        }

        /// <summary>
        ///     预警类型
        /// </summary>
        public string WarningType
        {
            get { return m_WarningType; }
            set
            {
                m_WarningType = value;
                RaisePropertyChanged(() => WarningType);
            }
        }

        /// <summary>
        ///     预警编码
        /// </summary>
        public string WarningTypeName
        {
            get { return m_WarningTypeName; }
            set
            {
                m_WarningTypeName = value;
                RaisePropertyChanged(() => WarningTypeName);
            }
        }

        /// <summary>
        ///     预警状态
        /// </summary>
        public string WarningState
        {
            get { return m_WarningState; }
            set
            {
                m_WarningState = value;
                RaisePropertyChanged(() => WarningState);
            }
        }

        /// <summary>
        ///     预警级别
        /// </summary>
        public string WarningLevel
        {
            get { return m_WarningLevel; }
            set
            {
                m_WarningLevel = value;
                RaisePropertyChanged(() => WarningLevel);
            }
        }

        /// <summary>
        ///     预警发布时间
        /// </summary>
        public string Time
        {
            get { return m_Time; }
            set
            {
                m_Time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        /// <summary>
        ///     经度
        /// </summary>
        public double Lon
        {
            get { return m_Lon; }
            set
            {
                m_Lon = value;
                RaisePropertyChanged(() => Lon);
            }
        }

        /// <summary>
        ///     纬度
        /// </summary>
        public double Lat
        {
            get { return m_Lat; }
            set
            {
                m_Lat = value;
                RaisePropertyChanged(() => Lat);
            }
        }

        /// <summary>
        ///     行政区域
        /// </summary>
        public string AreaCode
        {
            get { return m_areaCode; }
            set
            {
                m_areaCode = value;
                RaisePropertyChanged(() => AreaCode);
            }
        }
    }


    /// <summary>
    ///     风险态势接口类
    /// </summary>
    public class SituationObject
    {
        public Situation[] Situation { get; set; }
    }

    public class Situation
    {
        public Zonedata zonedata { get; set; }
        public colorCountObj colourcount { get; set; }
        public string warningtype { get; set; }
        public Warning[] warnings { get; set; }
        public string warningcount { get; set; }
        public string lon { get; set; }
        public string warninglevel { get; set; }
        public string lat { get; set; }
    }

    public class Zonedata
    {
        public string[] blue { get; set; }
        public string[] yellow { get; set; }
        public string[] red { get; set; }
        public string[] orange { get; set; }
        public string[] areacodes { get; set; }
    }

    /// <summary>
    ///     区域Code码
    /// </summary>
    public class ZoneColourCode
    {
        public List<string> orange { get; set; }
        public List<string> yellow { get; set; }
        public List<string> blue { get; set; }
        public List<string> red { get; set; }
        public List<string> areacodes { get; set; }
    }

    public class Rootobject
    {
        public string[] red { get; set; }
        public string[] orange { get; set; }
        public string[] yellow { get; set; }
        public string[] blue { get; set; }
        public string[] areacodes { get; set; }
    }


    public class Warning
    {
        public string identifier { get; set; }
        public string sender { get; set; }
        public string warningtype { get; set; }
        public string warningtypename { get; set; }
        public string time { get; set; }
        public string warninglevel { get; set; }

        /// <summary>
        ///     经度
        /// </summary>
        public string lon { get; set; }

        /// <summary>
        ///     纬度
        /// </summary>
        public string lat { get; set; }

        /// <summary>
        ///     行政区域
        /// </summary>
        public string areadesc { get; set; }
    }


    public class colorCountObj
    {
        public int red { get; set; }
        public int orange { get; set; }
        public int yellow { get; set; }
        public int blue { get; set; }
    }


    /// <summary>
    ///     综合查询-风险态势-预警 接口类
    /// </summary>
    public class SearchWarnings
    {
        public SearchWarning[] warnings { get; set; }
    }

    public class SearchWarning
    {
        public string name { get; set; }
        public int count { get; set; }
        public SearchWarningSouce[] souce { get; set; }
    }

    public class SearchWarningSouce
    {
        public string severity { get; set; }
        public string identifier { get; set; }
        public string sender { get; set; }
        public string eventType { get; set; }
        public string senderCode { get; set; }
        public string sendTime { get; set; }
        public string eventTypeName { get; set; }
        public string effectStatus { get; set; }
    }


    public class EventDictionary
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}