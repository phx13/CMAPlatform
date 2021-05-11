using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using CMAPlatform.Models;
using CMAPlatform.Models.AlarmDetailModel;
using CMAPlatform.Models.TimeBarModel;
using CMAPlatform.Models.TimeLineModel;
using Digihail.AVE.Controls.Utils;
using Microsoft.Practices.Prism.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMAPlatform.DataClient
{
    public class DataManager : NotificationObject
    {
        #region 单件

        public static DataManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (m_Obj)
                    {
                        if (m_Instance == null)
                        {
                            m_Instance = new DataManager();

                            var codebase = Assembly.GetExecutingAssembly().CodeBase;
                            var uri = new UriBuilder(codebase);
                            var path = Uri.UnescapeDataString(uri.Path);
                            var exePath = Path.GetDirectoryName(path);

                            // 加载配置文件
                            // 先从dll对应的config文件找
                            var currentExecutingFileName = Assembly.GetExecutingAssembly().GetModules()[0].Name;
                            // DLL名称
                            var fullyQualifiedName = Path.Combine(exePath,
                                currentExecutingFileName + ".config"); // .config文件完全路径

                            // 打开Config文件
                            var configFileMap = new ExeConfigurationFileMap();
                            configFileMap.ExeConfigFilename = fullyQualifiedName;
                            m_Instance.Config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap,
                                ConfigurationUserLevel.None);


                            rootPath = GetApiPath("ApiPath");
                            m_ConnectionString = GetApiPath("connectionString");

                            var dataType = m_Instance.Config.AppSettings.Settings["DataType"].Value;
                            //测试服务器
                            if (dataType == "0")
                            {
                                m_Instance.IsTestMode = true;
                            }
                            else
                            {
                                m_Instance.IsTestMode = false;
                            }

                            var isTestData = m_Instance.Config.AppSettings.Settings["IsTestData"].Value;
                            m_Instance.IsTestData = bool.Parse(isTestData);


                            var delaySeconds = m_Instance.Config.AppSettings.Settings["delaySeconds"].Value;
                            m_Instance.DelaySeconds = double.Parse(delaySeconds);

                            m_Instance.EventDictionary = m_Instance.EventDictiinaryType();
                        }
                    }
                }
                return m_Instance;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        ///     获取接口路径
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        private static string GetApiPath(string portName)
        {
            var pathPath = "";
            if (m_Instance == null)
                return "";
            //测试数据
            if (m_Instance.IsTestMode)
            {
                pathPath = m_Instance.Config.AppSettings.Settings["ApiPath"].Value;
                if (portName == "connectionString")
                {
                    pathPath = m_Instance.Config.AppSettings.Settings["connectionString"].Value;
                }
            }
            else
            {
                pathPath = m_Instance.Config.AppSettings.Settings[portName].Value;
            }
            return pathPath;
        }

        #endregion

        #region 成员变量

        #region 静态

        // 实例
        private static DataManager m_Instance;

        // 锁
        private static readonly object m_Obj = new object();

        // 字符串连接
        public static string m_ConnectionString =
            "server=10.0.0.116;port=3306;database=cmaplatform;user id=root;password=frontfree;";

        // 地址
        public static string rootPath = "http://localhost:1234/WebServiceTest.asmx/";

        #endregion

        /// <summary>
        ///     配置
        /// </summary>
        private Configuration Config;

        #endregion

        #region 公共属性

        /// <summary>
        ///     是否是测试模式
        /// </summary>
        public bool IsTestMode { get; private set; }

        /// <summary>
        ///     是否使用测试数据
        /// </summary>
        public bool IsTestData { get; private set; }

        /// <summary>
        ///     是否使用测试数据
        /// </summary>
        public double DelaySeconds { get; private set; }

        /// <summary>
        ///     类型表
        /// </summary>
        public ObservableCollection<EventDictionary> EventDictionary { get; private set; }

        #endregion

        #region 全局使用的突发事件与风险态势 - 实时通知属性

        private ObservableCollection<Events> m_EventModels = new ObservableCollection<Events>();

        /// <summary>
        ///     突发事件列表Model
        /// </summary>
        public ObservableCollection<Events> EventModels
        {
            get { return m_EventModels; }
            set
            {
                m_EventModels = value;
                RaisePropertyChanged("EventModels");
            }
        }

        private ObservableCollection<RiskSituationModel> m_RiskSituationModels =
            new ObservableCollection<RiskSituationModel>();

        /// <summary>
        ///     风险态势
        /// </summary>
        public ObservableCollection<RiskSituationModel> RiskSituationModels
        {
            get { return m_RiskSituationModels; }
            set
            {
                m_RiskSituationModels = value;
                RaisePropertyChanged("RiskSituationModels");
            }
        }

        private ObservableCollection<ExemtremeWeatherModel> m_ExemtremeWeatherModel =
            new ObservableCollection<ExemtremeWeatherModel>();

        /// <summary>
        ///     极端天气
        /// </summary>
        public ObservableCollection<ExemtremeWeatherModel> ExemtremeWeatherModel
        {
            get { return m_ExemtremeWeatherModel; }
            set
            {
                m_ExemtremeWeatherModel = value;
                RaisePropertyChanged("ExemtremeWeatherModel");
            }
        }

        #endregion

        #region 公共方法 - 调取API获取数据

        #region 首页

        /// <summary>
        ///     全国大喇叭总数及在线率
        ///     全国气象显示屏总数及在线率
        /// </summary>
        /// <returns></returns>
        public static YJSSGeneralModel getYJSSGeneralData()
        {
            rootPath = GetApiPath("getYJSSGeneralData");
            var path = rootPath + "getYJSSGeneralData";
            var json = APIHelper.Get(path);
            var yjssGeneralModel = new YJSSGeneralModel();
            yjssGeneralModel = JsonHelper.Deserialize<YJSSGeneralModel>(json);
            return yjssGeneralModel;
        }

        /// <summary>
        ///     信息员、灾情上报情况
        /// </summary>
        /// <returns></returns>
        public static XXYGeneralModel getXXYGeneralData()
        {
            rootPath = GetApiPath("getXXYGeneralData");
            var path = rootPath + "getXXYGeneralData?hyd=month";
            var json = APIHelper.Get(path);
            var xxyGeneralModel = new XXYGeneralModel();
            xxyGeneralModel = JsonHelper.Deserialize<XXYGeneralModel>(json);
            return xxyGeneralModel;
        }

        /// <summary>
        ///     预警总数信息员数量统计
        /// </summary>
        /// <returns></returns>
        public static CountOfEffectWarningModel getCountOfEffectWarning()
        {
            rootPath = GetApiPath("getCountOfEffectWarning");
            var path = rootPath + "getCountOfEffectWarning";
            var json = APIHelper.Get(path);
            var countOfEffectWarningModel = new CountOfEffectWarningModel();
            countOfEffectWarningModel = JsonHelper.Deserialize<CountOfEffectWarningModel>(json);
            return countOfEffectWarningModel;
        }

        /// <summary>
        ///     获取预警准确率
        /// </summary>
        /// <returns></returns>
        public static AccuracyOfWarningModel getAccuracyOfWarning()
        {
            rootPath = GetApiPath("getAccuracyOfWarning");
            var path = rootPath + "getAccuracyOfWarning";
            var json = APIHelper.Get(path);
            var accuracyOfWarningModel = new AccuracyOfWarningModel();
            accuracyOfWarningModel = JsonHelper.Deserialize<AccuracyOfWarningModel>(json);
            return accuracyOfWarningModel;
        }

        /// <summary>
        ///     34个省份数据
        /// </summary>
        /// <returns></returns>
        public static List<PreventionModel> infos()
        {
            rootPath = GetApiPath("infos");
            var path = rootPath + "infos";
            var json = APIHelper.Get(path);
            var ModelSource = new List<PreventionModel>();
            ModelSource = JsonHelper.Deserialize<List<PreventionModel>>(json);
            return ModelSource;
        }

        /// <summary>
        ///     单个省份详细数据
        /// </summary>
        /// <returns></returns>
        public static List<ProventionMessageModel> percent(string percent)
        {
            rootPath = GetApiPath("percent");
            var path = rootPath + "percent";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("province", percent);
            var json = APIHelper.Get(path, dictionary);
            var ModelSoures = new List<ProventionMessageModel>();
            var ModelSource = new ProventionMessageModel();
            ModelSource = JsonHelper.Deserialize<ProventionMessageModel>(json);
            ModelSoures.Add(ModelSource);
            return ModelSoures;
        }

        /// <summary>
        ///     首页-获取突发事件数据
        /// </summary>
        /// <returns></returns>
        public static RecEmergencise getRecEmergencise(int limit)
        {
            rootPath = GetApiPath("getRecEmergencise");
            var path = rootPath + "getRecEmergencise";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("limit", limit.ToString());
            var json = APIHelper.Get(path, dictionary);
            var data = JsonHelper.Deserialize<RecEmergencise>(json);
            return data;
        }

        /// <summary>
        ///     获取风险态势数据
        /// </summary>
        /// <returns></returns>
        public static SituationObject GetRiskSituationList()
        {
            rootPath = GetApiPath("GetRiskSituationList");
            var path = rootPath + "getStatisticsWarningRegin";
            var json = APIHelper.Get(path);
            var data = JsonHelper.Deserialize<SituationObject>(json);
            return data;
        }

        /// <summary>
        ///     极端天气---------------------待确认---------------------------待确认-------------------待确认-----------------------------------待确认
        /// </summary>
        /// <returns></returns>
        public static ExemtremeWeatherModel GetExemtremeWeatherList()
        {
            rootPath = GetApiPath("");
            var path = rootPath + "";
            var json = APIHelper.Get(path);
            var data = JsonHelper.Deserialize<ExemtremeWeatherModel>(json);
            return data;
        }

        #endregion

        #region 首页-POI点相关

        /// <summary>
        ///     获取POI数量
        /// </summary>
        /// <returns></returns>
        public static POIStatistics getPoiCount()
        {
            rootPath = GetApiPath("getPoiCount");
            var path = rootPath + "getPoiCount";
            var json = APIHelper.Get(path);
            var data = new POIStatistics();
            data = JsonHelper.Deserialize<POIStatistics>(json);
            return data;
        }

        /// <summary>
        ///     获取POI坐标
        /// </summary>
        /// <param name="poiType">POI类型</param>
        /// <returns>坐标集合</returns>
        public static List<Coord> GetPoiCoord(string poiType)
        {
            rootPath = GetApiPath("getCoord");
            var path = rootPath + "poiCoord";

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("poiType", poiType);
            var json = APIHelper.Get(path, dictionary);

            var Pois = new List<Coord>();

            if (!string.IsNullOrWhiteSpace(json))
            {
                var jsoArray = (JArray)JsonConvert.DeserializeObject(json);

                foreach (var jso in jsoArray)
                {
                    var cord = JsonHelper.Deserialize<Coord>(jso.ToString());
                    if (cord != null)
                    {
                        Pois.Add(cord);
                    }
                }
            }
            return Pois;
        }

        #endregion

        #region 综合查询

        /// <summary>
        ///     综合查询-突发事件
        /// </summary>
        /// <param name="eventList"></param>
        /// <returns></returns>
        public static RecEmergencise findEmergencise(EventListEnter eventList)
        {
            rootPath = GetApiPath("getRecEmergencise"); //与首页突发事件地址用一套
            var path = rootPath + "findEmergencise";
            var begintime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            ;
            var endtime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            var dictionary = new Dictionary<string, string>();
            if (eventList != null)
            {
                dictionary.Add("province", eventList.province);
                dictionary.Add("city", eventList.city);
                dictionary.Add("country", eventList.country);
                if (eventList.Begintime != null || eventList.Begintime != "")
                {
                    begintime = eventList.Begintime;
                }
                if (eventList.endtime != null || eventList.endtime != "")
                {
                    endtime = eventList.endtime;
                }
                dictionary.Add("begintime", begintime);
                dictionary.Add("endtime", endtime);
                dictionary.Add("eventType", eventList.eventType);
            }
            else
            {
                dictionary.Add("province", "null");
                dictionary.Add("city", "null");
                dictionary.Add("country", "null");
                dictionary.Add("begintime", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                dictionary.Add("endtime", DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                dictionary.Add("eventType", "null");
            }
            var json = APIHelper.Get(path, dictionary);
            var data = JsonHelper.Deserialize<RecEmergencise>(json);

            return data;
        }

        /// <summary>
        ///     综合查询_突发事件_灾害类型
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<EventDictionary> EventDictiinaryType()
        {
            var eventDictionaryLisCollection = new ObservableCollection<EventDictionary>();
            var strSql = "select * from eventdictionary";
            var ds = DBHelper.ExecuteDataSet(strSql);
            var dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var eventDictionary = new EventDictionary
                    {
                        Name = dr["事件类型"] + "",
                        Code = dr["编码"] + ""
                    };
                    eventDictionaryLisCollection.Add(eventDictionary);
                }
            }
            return eventDictionaryLisCollection;
        }

        /// <summary>
        ///     综合查询_风险态势_台风事件
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static TyphoonPortModel searchTyphoon(string beginTime = null, string endTime = null)
        {
            TyphoonPortModel data = new TyphoonPortModel();

            if (!DataManager.Instance.IsTestData)
            {
                rootPath = GetApiPath("getTyphoonAllList"); //与首页突发事件地址用一套
                var path = rootPath + "getTyphoonAllList";
                var json = APIHelper.Get(path);
                data = JsonHelper.Deserialize<TyphoonPortModel>(json);
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/台风事件.json");

                data = JsonHelper.DeserializeFromFile<TyphoonPortModel>(filePath);
            }


            return data;
        }

        /// <summary>
        ///     综合查询_风险态势_预警信息
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="eventType"></param>
        /// <param name="severity"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static SearchWarnings getWarningByEventType(string senderCode, string msgType, string eventType,
            string severity,
            string beginTime, string endTime)
        {
            var path = "";
            var dictionary = new Dictionary<string, string>();
            rootPath = GetApiPath("getWarningByEventType");
            if (senderCode != "")
            {
                path = rootPath + "getWarningByArea";
                dictionary.Add("senderCode", senderCode);
            }
            else
            {
                path = rootPath + "getWarningByEventType";
            }
            dictionary.Add("msgType", msgType);
            dictionary.Add("eventType", eventType);
            dictionary.Add("severity", severity);
            dictionary.Add("beginTime", beginTime);
            dictionary.Add("endTime", endTime);
            var json = APIHelper.Get(path, dictionary);
            var data = JsonHelper.Deserialize<SearchWarnings>(json);
            return data;
        }

        /// <summary>
        ///     综合查询_风险态势_台风事件
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static List<ClosestStation> GetClosestStation(string lon, string lat, string num)
        {
            var res = new List<ClosestStation>();

            if (DataManager.Instance.IsTestData)
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页/closestStation.json");
                res = JsonHelper.DeserializeFromFile<List<ClosestStation>>(filePath);
            }
            else
            {
                rootPath = GetApiPath("station");
                var path = rootPath + "station";
                var dictionary = new Dictionary<string, string>
                {
                    {"lon", lon},
                    {"lat", lat},
                    {"num", num}
                };
                var json = APIHelper.Get(path, dictionary);
                res = JsonHelper.Deserialize<List<ClosestStation>>(json);
            }
            return res;
        }

        /// <summary>
        ///     舆情热点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static List<Property> action(string start, string end, string limit)
        {
            rootPath = GetApiPath("action"); //与首页突发事件地址用一套
            var path = rootPath + "action";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("start", start);
            dictionary.Add("end", end);
            dictionary.Add("limit", limit);
            var json = APIHelper.Get(path, dictionary);
            var data = JsonHelper.Deserialize<List<Property>>(json);
            return data;
        }

        #region 事件页

        /// <summary>
        ///     获取责任区/警戒区/监视区范围
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="county"></param>
        /// <returns></returns>
        public static string code(string areaCode, string areaType)
        {
            var nZone3 = new Zone3();

            var JsonData = "";

            rootPath = GetApiPath("code");
            var path = rootPath + "code";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("areaCode", areaCode);
            dictionary.Add("areaType", areaType);
            var json = APIHelper.Get(path, dictionary);
            JsonData = json;

            return JsonData;
        }

        /// <summary>
        ///     获取预警概要描述
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static WarningDetail GetWarningByIdentifier(string identifier)
        {
            var data = new WarningDetail();
            rootPath = GetApiPath("getWarningByIdentifier");
            var path = rootPath + "getWarningByIdentifier";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("identifier", identifier);
            var json = APIHelper.Get(path, dictionary);
            data = JsonHelper.Deserialize<WarningDetail>(json);
            return data;
        }

        /// <summary>
        ///     单条预警的信息员发布情况
        /// </summary>
        /// <param name="id">告警id</param>
        /// <param name="hyd">month</param>
        /// <returns></returns>
        public static xxyAboutEarlyWarning GetXXYGeneralDataAboutEarlyWarning(string id, string hyd)
        {
            var data = new xxyAboutEarlyWarning();

            rootPath = GetApiPath("getXXYGeneralDataAboutEarlyWarning");
            var path = rootPath + "getXXYGeneralDataAboutEarlyWarning";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("id", id);
            dictionary.Add("hyd", hyd);
            var json = APIHelper.Get(path, dictionary);
            data = JsonHelper.Deserialize<xxyAboutEarlyWarning>(json);

            return data;
        }

        #endregion

        #endregion

        #region 省市县数据

        /// <summary>
        ///     获取省市县所有数据
        /// </summary>
        public List<Nationwide_Area> NationwideArea()
        {
            var nationwideAreas = new List<Nationwide_Area>();

            var strsql = "select * from a_region_szbb";
            var ds = DBHelper.ExecuteDataSet(strsql);
            var dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var nationwideArea = new Nationwide_Area();
                    nationwideArea.id = dr["id"] + "";
                    nationwideArea.parent_code = dr["parent_code"] + "";
                    nationwideArea.parent_name = dr["parent_name"] + "";
                    nationwideArea.region_name = dr["region_name"] + "";
                    nationwideArea.region_type = dr["region_type"] + "";
                    nationwideArea.code = dr["code"] + "";
                    nationwideAreas.Add(nationwideArea);
                }
            }
            return nationwideAreas;
        }

        #endregion

        #region 泳道图数据

        /// <summary>
        ///     获取泳道图告警数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<TimeLineWarningData> getTimeLineWarningData(string identifier, string eventType,
            string beginTime, string endTime)
        {
            var list = new List<TimeLineWarningData>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("getWarningByTime");
                var path = rootPath + "getWarningByTime";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("identifier", identifier);
                dictionary.Add("eventType", eventType);
                dictionary.Add("beginTime", beginTime);
                dictionary.Add("endTime", endTime);
                var json = APIHelper.Get(path, dictionary);

                var jsoArray = (JArray)JsonConvert.DeserializeObject(json);
                foreach (var jso in jsoArray)
                {
                    var data = JsonHelper.Deserialize<TimeLineWarningSource>(jso.ToString());
                    if (data != null)
                    {
                        list.AddRange(data.souce.ToList());
                    }
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/泳道图-预警.json");
                if (eventType == "eventTest")
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/突发事件泳道图预警.json");
                }
                var Content = File.ReadAllText(filePath);
                var jsoArray = (JArray)JsonConvert.DeserializeObject(Content);
                foreach (var jso in jsoArray)
                {
                    var data = JsonHelper.Deserialize<TimeLineWarningSource>(jso.ToString());
                    if (data != null)
                    {
                        list.AddRange(data.souce.ToList());
                    }
                }
            }
            return list;
        }

        /// <summary>
        ///     获取泳道图服务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<TimeLineService> getTimelineServices(string id, string start, string end)
        {
            var list = new List<TimeLineService>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("findDecisionFiles");
                var path = rootPath + "findDecisionFiles";

                var dictionary = new Dictionary<string, string>();
                //dictionary.Add("id", id);
                dictionary.Add("begintime", start);
                dictionary.Add("endtime", end);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var jsoArray = (JArray)JsonConvert.DeserializeObject(json);

                    foreach (var jso in jsoArray)
                    {
                        var cord = JsonHelper.Deserialize<TimeLineService>(jso.ToString());
                        if (cord != null)
                        {
                            list.Add(cord);
                        }
                    }
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/决策服务产品-全部.json");
                if (start == "eventTest")
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/突发事件泳道图服务.json");
                }
                var Content = File.ReadAllText(filePath);
                var jsoArray = (JArray)JsonConvert.DeserializeObject(Content);
                foreach (var jso in jsoArray)
                {
                    var cord = JsonHelper.Deserialize<TimeLineService>(jso.ToString());
                    if (cord != null)
                    {
                        list.Add(cord);
                    }
                }
            }


            return list;
        }

        /// <summary>
        ///     获取某条预警的站数统计数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<TimeLineWarningCount> getTimelineWarningCount(string AdminCode, string EventType,
            string StartTime, string EndTime)
        {
            var list = new List<TimeLineWarningCount>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("GetWarningNumber");
                var path = rootPath + "GetWarningNumber";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("AdminCode", AdminCode);
                dictionary.Add("EventType", EventType);
                dictionary.Add("StartTime", StartTime);
                dictionary.Add("EndTime", EndTime);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<TimeLineWarningCountRoot>(json);
                    if (root != null)
                    {
                        list = root.WarningsCount.ToList();
                    }
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/泳道图-站数统计.json");

                if (EventType == "11B05")
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/泳道图-寒潮站数统计.json");
                }

                var root = JsonHelper.DeserializeFromFile<TimeLineWarningCountRoot>(filePath);
                if (root != null)
                {
                    list = root.WarningsCount.ToList();
                }
            }
            return list;
        }

        /// <summary>
        ///     获取某条预警的站数据弹窗
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warningtype"></param>
        /// <param name="timestring"></param>
        /// <returns></returns>
        public static KeyValuePair<string, List<TimeLineStationData>> getTimelineWarningStationPop(string EventType,
            string Stations, string Time)
        {
            var keyValuePair = new KeyValuePair<string, List<TimeLineStationData>>();
            var list = new List<TimeLineStationData>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("GetWarningStationData");
                var path = rootPath + "GetWarningStationData";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("EventType", EventType);
                dictionary.Add("Stations", Stations);
                dictionary.Add("Time", Time);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<TimeLineStationDataRoot>(json);
                    if (root != null)
                    {
                        if (root.blue != null)
                        {
                            list = root.blue.ToList();
                            keyValuePair = new KeyValuePair<string, List<TimeLineStationData>>("blue", list);
                        }
                        else if (root.red != null)
                        {
                            list = root.red.ToList();
                            keyValuePair = new KeyValuePair<string, List<TimeLineStationData>>("red", list);
                        }
                        else if (root.orange != null)
                        {
                            list = root.orange.ToList();
                            keyValuePair = new KeyValuePair<string, List<TimeLineStationData>>("orange", list);
                        }
                        else if (root.yellow != null)
                        {
                            list = root.yellow.ToList();
                            keyValuePair = new KeyValuePair<string, List<TimeLineStationData>>("yellow", list);
                        }
                    }
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/泳道图-站数据弹窗.json");

                if (EventType == "11B05")
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/泳道图-寒潮站数据弹窗.json");
                }

                var root = JsonHelper.DeserializeFromFile<TimeLineStationDataRoot>(filePath);
                if (root != null)
                {
                    if (root.blue != null)
                    {
                        list = root.blue.ToList();
                        keyValuePair = new KeyValuePair<string, List<TimeLineStationData>>("blue", list);
                    }
                    else if (root.red != null)
                    {
                        list = root.red.ToList();
                        keyValuePair = new KeyValuePair<string, List<TimeLineStationData>>("red", list);
                    }
                    else if (root.orange != null)
                    {
                        list = root.orange.ToList();
                        keyValuePair = new KeyValuePair<string, List<TimeLineStationData>>("orange", list);
                    }
                    else if (root.yellow != null)
                    {
                        list = root.yellow.ToList();
                        keyValuePair = new KeyValuePair<string, List<TimeLineStationData>>("yellow", list);
                    }
                }
            }
            return keyValuePair;
        }

        /// <summary>
        ///     获取极值数据
        /// </summary>
        /// <param name="id">预警ID</param>
        /// <param name="hyd">活跃度</param>
        /// <returns></returns>
        public static List<PeakDataInfo> getPeakData(PeakDataParamsModel PeakParams)
        {
            var list = new List<PeakDataInfo>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("getPeakDataByTimeRegionAndRegion");
                var path = rootPath + "getPeakDataByTimeRegionAndRegion";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("adminCodeChn", PeakParams.adminCodeChn);
                dictionary.Add("endTime", PeakParams.endTime);
                dictionary.Add("peakName", PeakParams.peakName);
                dictionary.Add("startTime", PeakParams.startTime);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<PeakDataModel>(json);
                    if (root != null)
                    {
                        list = root.data.ToList();
                    }
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/泳道图-最大值.json");
                var root = JsonHelper.DeserializeFromFile<PeakDataModel>(filePath);
                if (root != null)
                {
                    list = root.data.ToList();
                }
            }
            return list;
        }

        /// <summary>
        ///     获取突发事件数据
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static List<EmergencyBarInfoJson> GetEmergencyForecastData(string stationId, string startTime,
            string endTime)
        {
            var res = new List<EmergencyBarInfoJson>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("forecast1");
                var path = rootPath + "forecast";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("stationId", stationId);
                dictionary.Add("startTime", startTime);
                dictionary.Add("endTime", endTime);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<EmergencyBarInfoRoot>(json);
                    res = root.liveDataList.ToList();
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/突发事件泳道图右下图表.json");
                var root = JsonHelper.DeserializeFromFile<EmergencyBarInfoRoot>(filePath);
                if (root != null)
                {
                    res = root.liveDataList.ToList();
                }
            }
            return res;
        }

        /// <summary>
        ///     获取寒潮数据
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static List<string[]> GetColdWaveData(string stationId, string startTime, string endTime)
        {
            var res = new List<string[]>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("wave");
                var path = rootPath + "wave";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("stationId", stationId);
                dictionary.Add("startTime", startTime);
                dictionary.Add("endTime", endTime);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<SingleStationColdWaveJson>(json);
                    res.Add(root.data_48);
                    res.Add(root.data_24);
                    res.Add(root.windSpeed);
                    res.Add(root.windSpeedDirection);
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/寒潮数据.json");
                var root = JsonHelper.DeserializeFromFile<SingleStationColdWaveJson>(filePath);
                if (root != null)
                {
                    res.Add(root.data_48);
                    res.Add(root.data_24);
                    res.Add(root.windSpeed);
                    res.Add(root.windSpeedDirection);
                }
            }
            return res;
        }

        /// <summary>
        ///     获取大风数据
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static List<string[]> GetWindData(string stationId, string startTime, string endTime)
        {
            var res = new List<string[]>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("live");
                var path = rootPath + "live";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("stationId", stationId);
                dictionary.Add("startTime", startTime);
                dictionary.Add("endTime", endTime);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<SingleStationWindJson>(json);
                    res.Add(root.maxWindSpeed);
                    res.Add(root.maxWindSpeedDirection);
                    res.Add(root.avgWindSpeed);
                    res.Add(root.avgWindSpeedDirection);
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/寒潮数据.json");
                var root = JsonHelper.DeserializeFromFile<SingleStationWindJson>(filePath);
                if (root != null)
                {
                    res.Add(root.maxWindSpeed);
                    res.Add(root.maxWindSpeedDirection);
                    res.Add(root.avgWindSpeed);
                    res.Add(root.avgWindSpeedDirection);
                }
            }
            return res;
        }

        /// <summary>
        ///     获取单站点暴雨数据
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static List<string[]> GetRainLiveAndForecastData(string stationId, string startTime, string endTime)
        {
            var res = new List<string[]>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("forecast");
                var path = rootPath + "forecast";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("stationId", stationId);
                dictionary.Add("startTime", startTime);
                dictionary.Add("endTime", endTime);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<TimeLineStationDetailDataRoot>(json);
                    res.Add(root.pre_1h);
                    res.Add(root.pre_3h);
                    res.Add(root.pre_6h);
                    res.Add(root.pre_12h);
                    res.Add(root.pre_24h);
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/预警态势泳道图降雨单柱图.json");
                var root = JsonHelper.DeserializeFromFile<TimeLineStationDetailDataRoot>(filePath);
                res.Add(root.pre_1h);
                res.Add(root.pre_3h);
                res.Add(root.pre_6h);
                res.Add(root.pre_12h);
                res.Add(root.pre_24h);
            }
            return res;
        }

        /// <summary>
        ///     获取单站点需要标注的时间点
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="stationId"></param>
        /// <param name="time"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="eventLevel"></param>
        /// <returns></returns>
        public static TimeLineStationDetailSignTime GetWarningSignTime(string eventType, string stationId, string time,
            string startTime, string endTime, string eventLevel)
        {
            var res = new TimeLineStationDetailSignTime();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("GetWarningSignTime");
                var path = rootPath + "GetWarningSignTime";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("EventType", eventType);
                dictionary.Add("StationId", stationId);
                dictionary.Add("Time", time);
                dictionary.Add("StartTime", startTime);
                dictionary.Add("EndTime", endTime);
                dictionary.Add("EventLevel", eventLevel);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    res = JsonHelper.Deserialize<TimeLineStationDetailSignTime>(json);
                }
            }
            return res;
        }

        /// <summary>
        ///     获取某条预警的预警责任人数量
        /// </summary>
        /// <param name="id">预警ID</param>
        /// <returns></returns>
        public static AlarmDutyPersonModel GetAlarmDutyPersonNumber(string id)
        {
            var data = new AlarmDutyPersonModel();

            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("getDutyPersonNumAboutEarlyWarning");
                var path = rootPath + "getDutyPersonNumAboutEarlyWarning";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("id", id);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<AlarmDutyPersonModel>(json);
                    if (root != null)
                    {
                        data = root;
                    }
                }
            }

            return data;
        }

        /// <summary>
        ///     获取某条预警详细的信息员数据
        /// </summary>
        /// <param name="id">预警ID</param>
        /// <param name="hyd">活跃度</param>
        /// <returns></returns>
        public static List<PeopleInfo> getTimelineWarningDetailPeopleInfo(string id, string hyd)
        {
            var list = new List<PeopleInfo>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("getXXYGeneralDataAboutEarlyWarning");
                var path = rootPath + "getXXYGeneralDataAboutEarlyWarning";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("id", id);
                dictionary.Add("hyd", hyd);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<TimeLineWarningDetailPeopleInfoModel>(json);
                    if (root != null)
                    {
                        list = root.data.ToList();
                    }
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/预警详情-信息员.json");
                var root = JsonHelper.DeserializeFromFile<TimeLineWarningDetailPeopleInfoModel>(filePath);
                if (root != null)
                {
                    list = root.data.ToList();
                }
            }
            return list;
        }

        /// <summary>
        ///     获取某条预警的预警流程
        /// </summary>
        /// <param name="id">预警ID</param>
        /// <returns></returns>
        public static List<AlarmProcess> GetSendProcessInfoAboutEarlyWarning(string id)
        {
            var data = new List<AlarmProcess>();

            rootPath = GetApiPath("getSendProcessInfoAboutEarlyWarning");
            var path = rootPath + "getSendProcessInfoAboutEarlyWarning";

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("id", id);
            var json = APIHelper.Get(path, dictionary);

            if (!string.IsNullOrWhiteSpace(json))
            {
                var root = JsonHelper.Deserialize<AlarmProcessModel>(json);
                if (root != null)
                {
                    data = root.result.ToList();
                }
            }

            return data;
        }

        /// <summary>
        ///     获取某条预警的预警手段数量
        /// </summary>
        /// <param name="id">预警ID</param>
        /// <returns></returns>
        public static List<AlarmMethodInfo> getSendResultInfoAboutEarlyWarning(string id)
        {
            var data = new List<AlarmMethodInfo>();
            rootPath = GetApiPath("getSendResultInfoAboutEarlyWarning");
            var path = rootPath + "getSendResultInfoAboutEarlyWarning";

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("id", id);
            var json = APIHelper.Get(path, dictionary);

            if (!string.IsNullOrWhiteSpace(json))
            {
                var root = JsonHelper.Deserialize<AlarmMethodModel>(json);
                if (root != null)
                {
                    data = root.result.ToList();
                }
            }
            return data;
        }

        #endregion

        #region 台风数据

        /// <summary>
        ///     获取台风数据
        /// </summary>
        public static List<TyphoonDataInfo> TyphoonData(string id)
        {
            var res = new List<TyphoonDataInfo>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("getTyphoonMessageById2");
                var path = rootPath + "getTyphoonMessageById2/" + id;

                var json = APIHelper.Get(path);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<TyphoonPathModel>(json);
                    if (root != null)
                    {
                        res = root.data.typhoon_live.ToList();
                    }
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页/台风.json");
                var root = JsonHelper.DeserializeFromFile<TyphoonPathModel>(filePath);
                if (root != null)
                {
                    res = root.data.typhoon_live.ToList();
                }
            }
            return res;
        }

        /// <summary>
        ///     获取台风落区数据
        /// </summary>
        public static List<TyphoonRegionModel> GetTyphoonWarningsByTime(string beginTime,string endTime)
        {
            var res = new List<TyphoonRegionModel>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("getTyphoonWarningsByTime");
                var path = rootPath + "getTyphoonWarningsByTime";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("beginTime", beginTime);
                dictionary.Add("endTime", endTime);
                var json = APIHelper.Get(path, dictionary);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<List<TyphoonRegionSouseModel>>(json);
                    if (root != null)
                    {
                        res = root.FirstOrDefault().souce;
                    }
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页/台风预警色斑图.json");
                var root = JsonHelper.DeserializeFromFile<List<TyphoonRegionSouseModel>>(filePath);
                if (root != null)
                {
                    res = root.FirstOrDefault().souce;
                }
            }
            return res;
        }

        /// <summary>
        ///     获取台风泳道图告警数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<TimeLineWarningData> GetWarningsOfTyphoon(string beginTime, string endTime)
        {
            var list = new List<TimeLineWarningData>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("getWarningsOfTyphoon");
                var path = rootPath + "getWarningsOfTyphoon";

                var dictionary = new Dictionary<string, string>();
                dictionary.Add("beginTime", beginTime);
                dictionary.Add("endTime", endTime);
                var json = APIHelper.Get(path, dictionary);

                var jsoArray = (JArray)JsonConvert.DeserializeObject(json);
                foreach (var jso in jsoArray)
                {
                    var data = JsonHelper.Deserialize<TimeLineWarningSource>(jso.ToString());
                    if (data != null)
                    {
                        list.AddRange(data.souce.ToList());
                    }
                }
            }
            else
            {
                //var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/泳道图-预警.json");
                //if (eventType == "eventTest")
                //{
                //    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页-泳道图/突发事件泳道图预警.json");
                //}
                //var Content = File.ReadAllText(filePath);
                //var jsoArray = (JArray)JsonConvert.DeserializeObject(Content);
                //foreach (var jso in jsoArray)
                //{
                //    var data = JsonHelper.Deserialize<TimeLineWarningSource>(jso.ToString());
                //    if (data != null)
                //    {
                //        list.AddRange(data.souce.ToList());
                //    }
                //}
            }
            return list;
        }

        /// <summary>
        ///     获取预警台风路径
        /// </summary>
        public static List<List<WarningTyphoonDataInfo>> GetTyphoonByTime(string time)
        {
            var res = new List<List<WarningTyphoonDataInfo>>();
            if (m_Instance.IsTestData == false)
            {
                rootPath = GetApiPath("getTyphoonByTime");
                var path = rootPath + "getTyphoonByTime/" + time;

                var json = APIHelper.Get(path);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var root = JsonHelper.Deserialize<WarningTyphoonPathModel>(json);
                    if (root != null)
                    {
                        for (int i = 0; i < root.data.ToList().Count; i++)
                        {
                            res.Add(root.data.ToList()[i].path.ToList());
                        }
                    }
                }
            }
            else
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/事件页/预警台风路径.json");
                var root = JsonHelper.DeserializeFromFile<WarningTyphoonPathModel>(filePath);
                if (root != null)
                {
                    for (int i = 0; i < root.data.ToList().Count; i++)
                    {
                        res.Add(root.data.ToList()[i].path.ToList());
                    }
                }
            }
            return res;
        }
        #endregion

        #endregion
    }
}