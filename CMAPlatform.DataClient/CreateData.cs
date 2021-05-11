using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMAPlatform.Models;
using CMAPlatform.Models.MessageModel;
using Digihail.AVE.Controls.Utils;

namespace CMAPlatform.DataClient
{
    public class CreateData
    {
        // 实例
        private static CreateData m_Instance;
        public static CreateData Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (new object())
                    {
                        if (m_Instance == null)
                        {
                            m_Instance = new CreateData();
                        }
                    }
                }
                return m_Instance;
            }
        }
        private readonly ResultModel resultModel = new ResultModel();

        public ResultModel CreateDataResult()
        {
            bool Isbool;
            //全国大喇叭总数及在线率和全国气象显示屏总数及在线率
            Isbool = YJSSGeneral();
            UpdateMessage(Isbool, "全国大喇叭总数及在线率和全国气象显示屏总数及在线率getYJSSGeneralData");
            //信息员、灾情上报情况
            Isbool = XXYGeneral();
            UpdateMessage(Isbool, "信息员、灾情上报情况getXXYGeneralData");
            //预警总数信息员数量统计
            Isbool = CountOfEffectWarning();
            UpdateMessage(Isbool, "预警总数信息员数量统计getCountOfEffectWarning");
            //获取所有省份一本账数据
            Isbool = infos();
            UpdateMessage(Isbool, "获取所有省份一本账数据GetPreventionProvinceList");

            Isbool = GetEventList(resultModel);
            UpdateMessage(Isbool, "获取首页的突发事件数据getRecEmergencise");


            Isbool = GetRiskSituationModels(resultModel);
            UpdateMessage(Isbool, "获取首页的风险态势数据GetRiskSituationList");
            Isbool = action();
            UpdateMessage(Isbool, "获取首页的舆情热点GetRiskSituationList");
            LoadJsons(resultModel);


            if (string.IsNullOrEmpty(resultModel.ErrorMessage))
            {
                resultModel.Result = true;
            }
            else
            {
                resultModel.Result = false;
            }
            return resultModel;
        }

        #region 事件页

        #region 事件页-左侧

        /// <summary>
        ///     插入事件页中预警的信息员活跃度与激活率
        /// </summary>
        /// <param name="eventListModel"></param>
        /// <returns></returns>
        public bool InsertXXYGeneralDataAboutEarlyWarning(xxyAboutEarlyWarningDatum data)
        {
            var Isbool = false;
            if (data != null)
            {
                var tableName = "eventrate";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);

                    var columnNames = new Dictionary<string, Dictionary<string, string>>();
                    columnNames.Add("ID", ColumnValueli("string", Guid.NewGuid().ToString()));
                    columnNames.Add("ActivationRate", ColumnValueli("int", data.jhl));
                    columnNames.Add("Activeness", ColumnValueli("int", data.hyd));
                    columnNames.Add("Time", ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                    sqls.Add(sqlstr);
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        #endregion

        #region 事件页-泳道图

        #endregion

        #endregion

        /// <summary>
        ///     把例值和列类型添加到集合
        /// </summary>
        /// <param name="type">列类型</param>
        /// <param name="value">列值</param>
        /// <returns></returns>
        public Dictionary<string, string> ColumnValueli(string type, string value)
        {
            var columnDictionary = new Dictionary<string, string>();
            columnDictionary.Add(type, value);
            return columnDictionary;
        }

        /// <summary>
        ///     拼接sql删除或插入语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="type">做的操作：Delete、Insert</param>
        /// <param name="columnNames">
        ///     <列名<列类型, 列值>>
        /// </param>
        /// <returns></returns>
        public string ShareExecuteSqlstr(string tableName, string type,
            Dictionary<string, Dictionary<string, string>> columnNames = null)
        {
            var sqlStr = "";
            var CloumnNamesql = "";
            var CloumnTypesql = "";
            var CloumnValuesql = "";
            if (type == "Delete")
            {
                sqlStr = "Delete From " + tableName;
            }
            else if (type == "Insert")
            {
                var columnNameli = new List<string>();
                var columnTypeli = new List<string>();
                var columnValueli = new List<string>();
                foreach (var columnName in columnNames.Keys)
                {
                    if (CloumnNamesql == "")
                    {
                        CloumnNamesql = columnName;
                    }
                    else
                    {
                        CloumnNamesql += "," + columnName;
                    }
                    foreach (var VARIABLE in columnNames[columnName])
                    {
                        if (VARIABLE.Key == "string")
                        {
                            if (CloumnValuesql == "")
                            {
                                CloumnValuesql = "'" + VARIABLE.Value + "'";
                            }
                            else
                            {
                                CloumnValuesql += ",'" + VARIABLE.Value + "'";
                            }
                        }
                        if (VARIABLE.Key == "int")
                        {
                            if (CloumnValuesql == "")
                            {
                                CloumnValuesql = VARIABLE.Value;
                            }
                            else
                            {
                                CloumnValuesql += "," + VARIABLE.Value;
                            }
                        }
                    }
                }
                sqlStr = string.Format("INSERT INTO {0}({1}) values({2});", tableName, CloumnNamesql, CloumnValuesql);
            }
            return sqlStr;
        }

        /// <summary>
        ///     错误集合
        /// </summary>
        /// <param name="isbool"></param>
        /// <param name="errormessage"></param>
        /// <returns></returns>
        public ResultModel UpdateMessage(bool isbool, string errormessage)
        {
            var error = "【报错接口】：";
            if (isbool)
            {
                resultModel.Result = isbool;
            }
            else
            {
                resultModel.Result = isbool;
                resultModel.ErrorMessage = error + errormessage;
            }
            return resultModel;
        }

        #region 首页接口

        #region 首页-左上角部分

        /// <summary>
        ///     预警总数信息员数量统计
        /// </summary>
        /// <returns></returns>
        public bool CountOfEffectWarning()
        {
            var countOfEffectWarningModel = new CountOfEffectWarningModel();
            countOfEffectWarningModel = DataManager.getCountOfEffectWarning();
            var accuracyOfWarningModel = new AccuracyOfWarningModel();
            accuracyOfWarningModel = DataManager.getAccuracyOfWarning();
            var columnNames = new Dictionary<string, Dictionary<string, string>>();
            var Isbool = false;
            if (countOfEffectWarningModel != null && accuracyOfWarningModel != null)
            {
                var tableName = "warnmessenger";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);
                    var redCount = 0;
                    var orangeCount = 0;
                    redCount = Convert.ToInt32(countOfEffectWarningModel.nation.red) +
                               Convert.ToInt32(countOfEffectWarningModel.province.red) +
                               Convert.ToInt32(countOfEffectWarningModel.city.red) +
                               Convert.ToInt32(countOfEffectWarningModel.county.red);
                    orangeCount = Convert.ToInt32(countOfEffectWarningModel.nation.orange) +
                                  Convert.ToInt32(countOfEffectWarningModel.province.orange) +
                                  Convert.ToInt32(countOfEffectWarningModel.city.orange) +
                                  Convert.ToInt32(countOfEffectWarningModel.county.orange);
                    columnNames.Add("TotalWarnCount", ColumnValueli("int", countOfEffectWarningModel.nationwide.total));
                    columnNames.Add("RedWarnCount", ColumnValueli("int", redCount.ToString()));
                    columnNames.Add("OrangeWarnCount", ColumnValueli("int", orangeCount.ToString()));
                    columnNames.Add("WarnQualityIndex",
                        ColumnValueli("string",
                            (Convert.ToDouble(accuracyOfWarningModel.accuracy) * 100).ToString()));
                    //columnNames.Add("WarnQualityIndex", ColumnValueli("string", "99.96"));
                    columnNames.Add("Time", ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                    sqls.Add(sqlstr);
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        /// <summary>
        ///     信息员、灾情上报情况
        /// </summary>
        /// <returns></returns>
        public bool XXYGeneral()
        {
            var xxyGeneralModel = new XXYGeneralModel();
            xxyGeneralModel = DataManager.getXXYGeneralData();
            var columnNames = new Dictionary<string, Dictionary<string, string>>();
            var Isbool = false;
            if (xxyGeneralModel != null && xxyGeneralModel.data.Count > 0)
            {
                var tableName = "messengerinfo";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);
                    foreach (var item in xxyGeneralModel.data)
                    {
                        columnNames.Add("xxyCount", ColumnValueli("int", item.xxy.ToString()));
                        columnNames.Add("Count", ColumnValueli("int", item.zq.ToString()));
                        columnNames.Add("ActivationRate", ColumnValueli("int", item.jhl.ToString()));
                        columnNames.Add("Activeness", ColumnValueli("int", item.hyd.ToString()));
                        columnNames.Add("Time", ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        sqls.Add(sqlstr);
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return Isbool;
        }

        /// <summary>
        ///     全国大喇叭总数及在线率和全国气象显示屏总数及在线率
        /// </summary>
        /// <returns></returns>
        public bool YJSSGeneral()
        {
            var yjssGeneralModel = new YJSSGeneralModel();
            yjssGeneralModel = DataManager.getYJSSGeneralData();
            var Isbool = false;
            if (yjssGeneralModel != null && yjssGeneralModel.Result.Count > 0)
            {
                var sqls = new List<string>();
                try
                {
                    var deletesql = "Delete From warnhornpercent;Delete From weatherscreenpercent";
                    DBHelper.ExecuteSql(deletesql);
                    for (var i = 0; i < yjssGeneralModel.Result.Count; i++)
                    {
                        var item = yjssGeneralModel.Result;
                        var dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        var Sqlstr = "";
                        if (item[i].type.ToString() == "0")
                        {
                            Sqlstr =
                                string.Format(
                                    "INSERT INTO warnhornpercent(Percent,Count,Time) values('{0}','{1}','{2}');",
                                    item[i].zxl, item[i].count, dateTime);
                            sqls.Add(Sqlstr);
                        }
                        else
                        {
                            Sqlstr =
                                string.Format(
                                    "INSERT INTO weatherscreenpercent(Percent,Count,Time) values('{0}','{1}','{2}');",
                                    item[i].zxl, item[i].count, dateTime);
                            sqls.Add(Sqlstr);
                        }
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return Isbool;
        }

        #endregion

        #region 首页-左下角部分

        /// <summary>
        ///     所有省份一本账数据
        /// </summary>
        /// <returns></returns>
        public bool infos()
        {
            var sourcesModel = new List<PreventionModel>();
            sourcesModel = DataManager.infos();
            var Isbool = false;
            if (sourcesModel != null && sourcesModel.Count > 0)
            {
                var tableName = "preventionprovince";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);
                    foreach (var item in sourcesModel)
                    {
                        var columnNames = new Dictionary<string, Dictionary<string, string>>();
                        columnNames.Add("Name", ColumnValueli("string", item.province));
                        columnNames.Add("Value", ColumnValueli("int", item.Value.ToString()));
                        columnNames.Add("Time", ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        sqls.Add(sqlstr);
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        /// <summary>
        ///     单个省份详细数据
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public Dictionary<bool, List<ProventionMessageModel>> percent(string percent)
        {
            var ResualSource = new Dictionary<bool, List<ProventionMessageModel>>();
            var proventionMessageModel = new List<ProventionMessageModel>();
            var Isbool = false;
            proventionMessageModel = DataManager.percent(percent);
            if (proventionMessageModel != null)
            {
                Isbool = true;
            }
            ResualSource.Add(Isbool, proventionMessageModel);
            return ResualSource;
        }

        #endregion

        #region 舆情论点

        /// <summary>
        ///     舆情热点
        /// </summary>
        /// <returns></returns>
        public bool action()
        {
            var rootobjectModel = new List<Property>();

            rootobjectModel = DataManager.action(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"),
                DateTime.Now.ToString("yyyy-MM-dd 23:59:59"), "10");
            var Isbool = false;
            if (rootobjectModel != null && rootobjectModel.Count > 0)
            {
                var tableName = "opinionhotspots";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);
                    foreach (var item in rootobjectModel)
                    {
                        var columnNames = new Dictionary<string, Dictionary<string, string>>();
                        columnNames.Add("`Index`", ColumnValueli("int", item.quantity.ToString()));
                        columnNames.Add("Opinion", ColumnValueli("string", item.eventname));
                        columnNames.Add("HotValue", ColumnValueli("int", item.persent));
                        columnNames.Add("Time", ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        sqls.Add(sqlstr);
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        #endregion

        #region 首页-突发事件/风险态势/极端天气

        /// <summary>
        ///     首页-突发事件
        /// </summary>
        /// <returns></returns>
        public bool GetEventList(ResultModel result)
        {
            var eventListModel = new EventListModel();
            eventListModel.Events = new List<Events>();
            var Isbool = true;
            try
            {
                //1、拉取数据
                var data = DataManager.getRecEmergencise(5);
                if (data == null)
                {
                    return false;
                }
                //2、转换数据
                eventListModel = ConvertEventListModel(data);
                if (eventListModel == null)
                {
                    return false;
                }
                //3、插入数据
                Isbool = InsertConvertEventListModel(eventListModel);

                //4、更新全局突发事件
                result.EventListModel = eventListModel;
            }
            catch (Exception ee)
            {
                Isbool = false;
            }

            return Isbool;
        }

        /// <summary>
        ///     首页-风险态势
        /// </summary>
        /// <returns></returns>
        public bool GetRiskSituationModels(ResultModel result)
        {
            var Isbool = true;
            try
            {
                //1、拉取数据
                var data = DataManager.GetRiskSituationList();
                if (data == null)
                {
                    return false;
                }
                //2、转换数据
                var risks = ConvertRiskSituationModels(data);
                if (risks == null)
                {
                    return false;
                }
                //3、插入数据
                Isbool = InsertRisksituation(risks);
                if (!Isbool)
                {
                    return false;
                }
                Isbool = InsertRisksituationLabel(risks);
                //4、更新全局突发事件
                result.RiskSituationModels = risks;
            }
            catch (Exception ee)
            {
                Isbool = false;
            }

            return Isbool;
        }

        /// <summary>
        ///     转换突发事件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static EventListModel ConvertEventListModel(RecEmergencise data)
        {
            var eventListModel = new EventListModel();
            eventListModel.Events = new List<Events>();
            if (data != null && data.code == "0")
            {
                //转换模型
                if (data.value != null && data.value.Count() > 0)
                {
                    foreach (var recEmergenciseValue in data.value)
                    {
                        var newEvents = new Events();
                        newEvents.ID = Guid.NewGuid().ToString(); //突发事件目前没有ID,New一个
                        newEvents.Name = recEmergenciseValue.title.Trim();
                        newEvents.ShowTitle = recEmergenciseValue.title.Trim();
                        double longitude = 0;
                        double.TryParse(recEmergenciseValue.lon, out longitude);
                        newEvents.Longitude = longitude;
                        double latitude = 0;
                        double.TryParse(recEmergenciseValue.lat, out latitude);
                        newEvents.Latitude = latitude;
                        newEvents.EventType = "";
                        newEvents.Location = recEmergenciseValue.place.Trim();
                        newEvents.Detail = recEmergenciseValue.eventDescription.Trim();
                        newEvents.Time = recEmergenciseValue.time.Trim();
                        newEvents.Source = recEmergenciseValue.from.Trim();
                        newEvents.EndTime = recEmergenciseValue.ctime;
                        newEvents.Province = recEmergenciseValue.province;
                        newEvents.City = recEmergenciseValue.city;
                        newEvents.Country = recEmergenciseValue.country;

                        if (recEmergenciseValue.eventStatus == "Begin" ||
                            recEmergenciseValue.eventStatus == "Update")
                        {
                            newEvents.IsHistory = false;
                        }
                        else
                        {
                            newEvents.IsHistory = true;
                        }
                        eventListModel.Events.Add(newEvents);
                    }
                }
            }

            return eventListModel;
        }

        /// <summary>
        ///     转换台风事件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<TyphoonEventsModel> ConvertTyphoonModel(TyphoonPortModel data)
        {
            var typhoonEventsModels = new List<TyphoonEventsModel>();
            if (data != null)
            {
                //转换模型
                if (data.data != null && data.data.Count() > 0)
                {
                    foreach (var item in data.data)
                    {
                        var typhoonEventsModel = new TyphoonEventsModel();
                        typhoonEventsModel.ID = item.typhoon_id;
                        typhoonEventsModel.Name = item.typhoon_name_cn;
                        typhoonEventsModel.EnglishName = item.typhoon_name;
                        typhoonEventsModel.StartTime = item.bj_datetime;
                        typhoonEventsModels.Add(typhoonEventsModel);
                    }
                }
            }
            return typhoonEventsModels;
        }

        /// <summary>
        ///     首页转换风险态势
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<RiskSituationModel> ConvertRiskSituationModels(SituationObject data)
        {
            var riskSituationModels = new List<RiskSituationModel>();

            if (data != null)
            {
                //转换模型
                if (data.Situation != null && data.Situation.Count() > 0)
                {
                    var GroupWarningtype = data.Situation.GroupBy(p => p.warningtype);

                    foreach (var item in GroupWarningtype)
                    {
                        var name = item.Key;
                        var WarningOrderBy = 5;
                        var WarningLevelName = "";
                        var WarningLevelName_red = "";
                        var WarningLevelName_yellow = "";
                        var WarningLevelName_orange = "";
                        var WarningLevelName_blue = "";
                        var GroupTypeData = data.Situation.Where(p => p.warningtype == name).ToList();

                        #region

                        if (GroupTypeData.Count > 0)
                        {
                            for (var i = 0; i < GroupTypeData.Count; i++)
                            {
                                if (GroupTypeData[i].warninglevel == "red")
                                {
                                    WarningLevelName_red = "red";
                                }
                                else if (GroupTypeData[i].warninglevel == "orange")
                                {
                                    WarningLevelName_orange = "orange";
                                }
                                else if (GroupTypeData[i].warninglevel == "yellow")
                                {
                                    WarningLevelName_yellow = "yellow";
                                }
                                else
                                {
                                    WarningLevelName_blue = "blue";
                                }
                            }
                            if (WarningLevelName_blue != "")
                            {
                                WarningLevelName = WarningLevelName_blue;
                                WarningOrderBy = 4;
                            }
                            if (WarningLevelName_yellow != "")
                            {
                                WarningLevelName = WarningLevelName_yellow;
                                WarningOrderBy = 3;
                            }
                            if (WarningLevelName_orange != "")
                            {
                                WarningLevelName = WarningLevelName_orange;
                                WarningOrderBy = 2;
                            }
                            if (WarningLevelName_red != "")
                            {
                                WarningLevelName = WarningLevelName_red;
                                WarningOrderBy = 1;
                            }

                            #endregion

                            var situationGroupSituation = GroupTypeData[0];
                            var riskSituationModel = new RiskSituationModel();
                            var guid = Guid.NewGuid().ToString();
                            riskSituationModel.WarningType = name;
                            riskSituationModel.WarningLevel = WarningLevelName;
                            riskSituationModel.OrderId = WarningOrderBy;
                            riskSituationModel.ZoneId = guid; //落区目前没有ID,New一个
                            riskSituationModel.GroupWarning = new List<GroupWarning>();
                            string id;
                            string colourLeve;
                            var MessageData = data.Situation.Where(p => p.warningtype == name);

                            foreach (var situation in MessageData)
                            {
                                riskSituationModel.WarningCount = riskSituationModel.WarningCount +
                                                                  int.Parse(situation.warningcount);
                                var groupWarning = new GroupWarning();
                                id = Guid.NewGuid().ToString();
                                groupWarning.id.Add(id);
                                groupWarning.aid = Guid.NewGuid().ToString();
                                colourLeve = situation.warninglevel;
                                groupWarning.WarningLevel.Add(colourLeve);
                                var location = new Locations();
                                double longitude = 0;
                                double.TryParse(situation.lon, out longitude);
                                double latitude = 0;
                                double.TryParse(situation.lat, out latitude);
                                location.Longitude = longitude;
                                location.Latitude = latitude;
                                groupWarning.Location.Add(location);
                                riskSituationModel.ZoneData = situation.zonedata;
                                var zonedata = riskSituationModel.ZoneData;
                                var colorCount = new colorCountObj();
                                if (situation.colourcount != null)
                                {
                                    riskSituationModel.ColorCount = situation.colourcount;
                                    if (riskSituationModel.ColorCount != null)
                                    {
                                        riskSituationModel.RedCount = riskSituationModel.RedCount +
                                                                      riskSituationModel.ColorCount.red;
                                        riskSituationModel.OrangeCount = riskSituationModel.OrangeCount +
                                                                         riskSituationModel.ColorCount.orange;
                                        riskSituationModel.Blue = riskSituationModel.Blue +
                                                                  riskSituationModel.ColorCount.blue;
                                        riskSituationModel.Yellow = riskSituationModel.Yellow +
                                                                    riskSituationModel.ColorCount.yellow;
                                        riskSituationModel.AreacodesCount = riskSituationModel.RedCount +
                                                                            riskSituationModel.OrangeCount +
                                                                            riskSituationModel.Blue +
                                                                            riskSituationModel.Yellow;
                                    }
                                }
                                colorCount.red = riskSituationModel.ColorCount.red;
                                colorCount.orange = riskSituationModel.ColorCount.orange;
                                colorCount.yellow = riskSituationModel.ColorCount.yellow;
                                colorCount.blue = riskSituationModel.ColorCount.blue;
                                groupWarning.Colourcount.Add(colorCount);
                                groupWarning.ZoneData.Add(zonedata);
                                //riskSituationModel.ColorCount = situation.colourCount;

                                foreach (var warning in situation.warnings)
                                {
                                    var newWarnings = new Warnings();
                                    newWarnings.ID = warning.identifier;
                                    newWarnings.AreaCode = warning.areadesc;
                                    newWarnings.Lon = Convert.ToDouble(warning.lon);
                                    newWarnings.Lat = Convert.ToDouble(warning.lat);
                                    newWarnings.Station = warning.sender;
                                    newWarnings.WarningLevel = warning.warninglevel;
                                    newWarnings.WarningType = warning.warningtype;
                                    newWarnings.WarningTypeName = warning.warningtypename;
                                    newWarnings.WarningState = ""; //告警状态暂无
                                    //newWarnings.StationLevel = warning.s;
                                    newWarnings.Time = warning.time;
                                    groupWarning.Warnings.Add(newWarnings);
                                }
                                riskSituationModel.GroupWarning.Add(groupWarning);
                            }
                            riskSituationModels.Add(riskSituationModel);
                        }
                    }
                }
            }

            return riskSituationModels;
        }

        /// <summary>
        ///     综合查询转换风险态势
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<RiskSituationModel> ConvertRiskSituationModel(SearchWarnings data)
        {
            var riskSituationModels = new List<RiskSituationModel>();

            if (data != null)
            {
                //转换模型
                if (data.warnings != null && data.warnings.Count() > 0)
                {
                    foreach (var situation in data.warnings)
                    {
                        if (situation.souce.Count() > 0)
                        {
                            var riskSituationModel = new RiskSituationModel();
                            riskSituationModel.ZoneId = Guid.NewGuid().ToString(); //落区目前没有ID,New一个
                            riskSituationModel.WarningType = situation.name;
                            riskSituationModel.WarningLevel = "red";
                            riskSituationModel.WarningCount = situation.souce.Count();
                            var existColour = situation.souce.ToList().Any(t => t.severity == "red");
                            if (existColour)
                            {
                                riskSituationModel.WarningLevel = "red";
                            }
                            existColour = situation.souce.ToList().Any(t => t.severity == "orange");
                            if (existColour)
                            {
                                riskSituationModel.WarningLevel = "orange";
                            }
                            existColour = situation.souce.ToList().Any(t => t.severity == "yellow");
                            if (existColour)
                            {
                                riskSituationModel.WarningLevel = "yellow";
                            }
                            existColour = situation.souce.ToList().Any(t => t.severity == "blue");
                            if (existColour)
                            {
                                riskSituationModel.WarningLevel = "blue";
                            }
                            if (situation.souce != null && situation.souce.Length > 0)
                            {
                                riskSituationModel.Warnings = new List<Warnings>();
                                foreach (var item in situation.souce)
                                {
                                    var newWarnings = new Warnings();
                                    newWarnings.ID = item.identifier;
                                    newWarnings.Station = item.sender;
                                    newWarnings.WarningLevel = item.severity;
                                    newWarnings.WarningType = item.eventType;
                                    newWarnings.AreaCode = item.identifier.Substring(0, 6);
                                    newWarnings.WarningState = item.eventTypeName; //告警状态暂无
                                    //newWarnings.StationLevel = warning.s;
                                    newWarnings.Time = Convert.ToDateTime(item.sendTime).ToString("yyyy-MM-dd HH:mm");
                                    riskSituationModel.Warnings.Add(newWarnings);
                                }
                            }
                            riskSituationModels.Add(riskSituationModel);
                        }
                    }
                }
            }
            return riskSituationModels;
        }

        /// <summary>
        ///     插入突发事件
        /// </summary>
        /// <param name="eventListModel"></param>
        /// <returns></returns>
        public bool InsertConvertEventListModel(EventListModel eventListModel)
        {
            var Isbool = false;
            if (eventListModel != null && eventListModel.Events != null && eventListModel.Events.Count > 0)
            {
                var tableName = "emergencyevent";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);
                    foreach (var item in eventListModel.Events)
                    {
                        if (item.IsHistory)
                        {
                            //目前不在库里插入历史数据
                            continue;
                        }

                        var columnNames = new Dictionary<string, Dictionary<string, string>>();
                        columnNames.Add("ID", ColumnValueli("string", item.ID));
                        columnNames.Add("Name", ColumnValueli("string", item.Name));
                        columnNames.Add("EventType", ColumnValueli("string", item.EventType));
                        columnNames.Add("Location", ColumnValueli("string", item.Location));
                        columnNames.Add("Longitude", ColumnValueli("int", item.Longitude.ToString()));
                        columnNames.Add("Latitude", ColumnValueli("int", item.Latitude.ToString()));
                        columnNames.Add("ShowTitle", ColumnValueli("string", item.ShowTitle));
                        columnNames.Add("Time", ColumnValueli("string", item.Time));
                        columnNames.Add("Detail", ColumnValueli("string", item.Detail));
                        columnNames.Add("Source", ColumnValueli("string", item.Source));

                        var historyint = item.IsHistory ? 1 : 0;

                        var dataTime = DateTime.MinValue;

                        //判断是否是今天数据
                        var istoday = "0";

                        if (!DateTime.TryParse(item.Time, out dataTime))
                        {
                            istoday = "0";
                        }
                        else
                        {
                            if (dataTime > DateTime.Now.AddDays(-1))
                            {
                                istoday = "1";
                            }
                            else
                            {
                                istoday = "0";
                            }
                        }
                        columnNames.Add("IsToday", ColumnValueli("string", istoday));

                        columnNames.Add("IsHistory", ColumnValueli("int", historyint.ToString()));
                        columnNames.Add("InsertTime",
                            ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                        var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        sqls.Add(sqlstr);
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        private int SetIndex(int count, int totalcount, string zoneid)
        {
            var result = totalcount + count;
            if (zoneid.EndsWith("0000"))
            {
                result += 10000;
            }
            else if (zoneid.EndsWith("00"))
            {
                result += 20000;
            }
            else
            {
                result += 30000;
            }
            return result;
        }

        /// <summary>
        ///     插入风险态势-落区
        /// </summary>
        /// <param name="eventListModel"></param>
        /// <returns></returns>
        public bool InsertRisksituation(List<RiskSituationModel> riskSituationModels)
        {
            var Isbool = false;
            if (riskSituationModels != null && riskSituationModels.Count > 0)
            {
                var tableName = "risksituation";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);

                    var totalcount = 0;
                    foreach (var items in riskSituationModels)
                    {
                        var sqlstr = "";
                        var dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        foreach (var data in items.GroupWarning)
                        {
                            var itemZonedatas = data.ZoneData;
                            foreach (var item in itemZonedatas)
                            {
                                if (item.red != null)
                                {
                                    var count = item.red.Count();

                                    for (var i = 0; i < count; i++)
                                    {
                                        var redCode = item.red[i];
                                        var columnNames = new Dictionary<string, Dictionary<string, string>>();
                                        columnNames.Add("name", ColumnValueli("string", items.ZoneId));
                                        columnNames.Add("id", ColumnValueli("string", redCode));
                                        columnNames.Add("color", ColumnValueli("string", "red"));
                                        columnNames.Add("time", ColumnValueli("string", dateTime));
                                        columnNames.Add("type",
                                            ColumnValueli("int", SetIndex(i, totalcount, redCode).ToString()));
                                        sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                                        sqls.Add(sqlstr);
                                    }
                                    totalcount += count;
                                }
                                if (item.orange != null)
                                {
                                    var count = item.orange.Count();

                                    for (var i = 0; i < count; i++)
                                    {
                                        var orangeCode = item.orange[i];
                                        var columnNames = new Dictionary<string, Dictionary<string, string>>();
                                        columnNames.Add("name", ColumnValueli("string", items.ZoneId));
                                        columnNames.Add("id", ColumnValueli("string", orangeCode));
                                        columnNames.Add("color", ColumnValueli("string", "orange"));
                                        columnNames.Add("time", ColumnValueli("string", dateTime));
                                        columnNames.Add("type",
                                            ColumnValueli("int", SetIndex(i, totalcount, orangeCode).ToString()));
                                        sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                                        sqls.Add(sqlstr);
                                    }
                                    totalcount += count;
                                }
                            }
                        }
                        //if (item.blue != null)
                        //{
                        //    int count = item.blue.Count();

                        //    for (int i = 0; i < count; i++)
                        //    {
                        //        var blueCode = item.blue[i];
                        //        Dictionary<string, Dictionary<string, string>> columnNames = new Dictionary<string, Dictionary<string, string>>();
                        //        columnNames.Add("name", ColumnValueli("string", items.ZoneId));
                        //        columnNames.Add("id", ColumnValueli("string", blueCode));
                        //        columnNames.Add("color", ColumnValueli("string", "blue"));
                        //        columnNames.Add("time", ColumnValueli("string", dateTime));
                        //        columnNames.Add("type", ColumnValueli("int", SetIndex(i, totalcount,blueCode).ToString()));
                        //        sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        //        sqls.Add(sqlstr);

                        //    }

                        //    totalcount += count;
                        //}

                        //if (item.yellow != null)
                        //{
                        //    int count = item.yellow.Count();

                        //    for (int i = 0; i < count; i++)
                        //    {
                        //        var yellowCode = item.yellow[i];
                        //        Dictionary<string, Dictionary<string, string>> columnNames = new Dictionary<string, Dictionary<string, string>>();
                        //        columnNames.Add("name", ColumnValueli("string", items.ZoneId));
                        //        columnNames.Add("id", ColumnValueli("string", yellowCode));
                        //        columnNames.Add("color", ColumnValueli("string", "yellow"));
                        //        columnNames.Add("time", ColumnValueli("string", dateTime));
                        //        columnNames.Add("type", ColumnValueli("int", SetIndex(i,totalcount, yellowCode).ToString()));
                        //        sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        //        sqls.Add(sqlstr);
                        //    }

                        //    totalcount += count;
                        //}
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        /// <summary>
        ///     插入风险态势-标牌
        /// </summary>
        /// <param name="eventListModel"></param>
        /// <returns></returns>
        public bool InsertRisksituationLabel(List<RiskSituationModel> riskSituationModels)
        {
            var Isbool = false;
            if (riskSituationModels != null && riskSituationModels.Count > 0)
            {
                var tableName = "risksituationlabel";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);
                    foreach (var item in riskSituationModels)
                    {
                        foreach (var data in item.GroupWarning)
                        {
                            var itemLocations = data.Location;
                            double d = -1;
                            foreach (var dataLocations in itemLocations)
                            {
                                d++;
                                if (data.WarningLevel[Convert.ToInt32(d)] == "blue" ||
                                    data.WarningLevel[Convert.ToInt32(d)] == "yellow")
                                {
                                    continue;
                                }
                                var columnNames = new Dictionary<string, Dictionary<string, string>>();
                                columnNames.Add("GroupKey", ColumnValueli("string", data.id[Convert.ToInt32(d)]));
                                columnNames.Add("Lon", ColumnValueli("int", dataLocations.Longitude.ToString()));
                                columnNames.Add("Lat", ColumnValueli("int", dataLocations.Latitude.ToString()));
                                columnNames.Add("Alt", ColumnValueli("int", "0"));

                                columnNames.Add("Level", ColumnValueli("string", data.WarningLevel[Convert.ToInt32(d)]));
                                columnNames.Add("TypeName", ColumnValueli("string", item.WarningType));
                                columnNames.Add("time",
                                    ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                                columnNames.Add("RedCount",
                                    ColumnValueli("int", data.Colourcount[Convert.ToInt32(d)].red.ToString()));
                                columnNames.Add("OrangeCount",
                                    ColumnValueli("int", data.Colourcount[Convert.ToInt32(d)].orange.ToString()));
                                columnNames.Add("YellowCount", ColumnValueli("int", "0"));
                                columnNames.Add("BlueCount", ColumnValueli("int", "0"));

                                var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                                sqls.Add(sqlstr);
                            }

                            //List<string> idsList = data.id;
                            //double d = -1;
                            //foreach (string id in idsList)
                            //{
                            //    d++;
                            //    if (data.WarningLevel[Convert.ToInt32(d)] == "blue" || data.WarningLevel[Convert.ToInt32(d)] == "yellow")
                            //    {
                            //        continue;
                            //    }
                            //    Dictionary<string, Dictionary<string, string>> columnNames = new Dictionary<string, Dictionary<string, string>>();
                            //    columnNames.Add("GroupKey", ColumnValueli("string", id));
                            //    columnNames.Add("Lon", ColumnValueli("int", data.Location[Convert.ToInt32(d)].Longitude.ToString()));
                            //    columnNames.Add("Lat", ColumnValueli("int", data.Location[Convert.ToInt32(d)].Latitude.ToString()));
                            //    columnNames.Add("Alt", ColumnValueli("int", "0"));

                            //    columnNames.Add("Level", ColumnValueli("string", data.WarningLevel[Convert.ToInt32(d)]));
                            //    columnNames.Add("TypeName", ColumnValueli("string", item.WarningType));
                            //    columnNames.Add("time", ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                            //    columnNames.Add("RedCount", ColumnValueli("int", data.Colourcount[Convert.ToInt32(d)].red.ToString()));
                            //    columnNames.Add("OrangeCount", ColumnValueli("int", data.Colourcount[Convert.ToInt32(d)].orange.ToString()));
                            //    columnNames.Add("YellowCount", ColumnValueli("int", data.Colourcount[Convert.ToInt32(d)].yellow.ToString()));
                            //    columnNames.Add("BlueCount", ColumnValueli("int", data.Colourcount[Convert.ToInt32(d)].blue.ToString()));

                            //    string sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                            //    sqls.Add(sqlstr);
                            //}
                        }
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        /// <summary>
        ///     从Json文件获取数据
        /// </summary>
        /// <param name="result"></param>
        public void LoadJsons(ResultModel result)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = "";

            //filePath = System.IO.Path.Combine(directory, "Data/RiskData.json");
            //result.RiskSituationModels = JsonHelper.DeserializeFromFile<List<RiskSituationModel>>(filePath, null, false);
            filePath = Path.Combine(directory, "Data/ExemtremeData.json");
            result.ExemtremeWeatherModels = JsonHelper.DeserializeFromFile<List<ExemtremeWeatherModel>>(filePath, null,
                false);
        }

        #endregion

        #region 首页-POI点相关

        /// <summary>
        ///     仅调用一次 - POI点相关
        /// </summary>
        public void CreateDataOnlyOnce()
        {
            //获取Poi热力图数据
            GetPoiStatistic();

            //获取Poi散点
            GetPoiData();

            //获取Poi分类统计数据
            GetPoiTypeCount();
        }

        /// <summary>
        ///     获取POI热力数据
        /// </summary>
        /// <returns></returns>
        public bool GetPoiStatistic()
        {
            var heats = new List<PoiHeat>();
            var Isbool = true;
            try
            {
                //1、拉取数据
                var data = DataManager.getPoiCount();
                if (data == null)
                {
                    return false;
                }
                //2、转换数据
                heats = ConvertPoiHeat(data);
                if (heats == null)
                {
                    return false;
                }
                //3、插入数据
                Isbool = InsertConvertPoiHeat(heats);
            }
            catch (Exception ee)
            {
                Isbool = false;
            }

            return Isbool;
        }

        /// <summary>
        ///     转换POI热力图数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<PoiHeat> ConvertPoiHeat(POIStatistics data)
        {
            var heats = new List<PoiHeat>();
            if (data != null)
            {
                //转换模型
                foreach (var province in data.province)
                {
                    var porvinceHeat = new PoiHeat();

                    if (province.coord == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(province.coord.lon) || string.IsNullOrWhiteSpace(province.coord.lat))
                    {
                        continue;
                    }
                    double lon = 0;
                    double lat = 0;
                    if (!double.TryParse(province.coord.lon, out lon) || !double.TryParse(province.coord.lat, out lat))
                    {
                        continue;
                    }
                    porvinceHeat.Lon = lon;
                    porvinceHeat.Lat = lat;
                    porvinceHeat.Code = province.code;
                    porvinceHeat.Level = "province";
                    porvinceHeat.Count = province.poiCount.Airport + province.poiCount.CarService +
                                         province.poiCount.JYZ + province.poiCount.RailwayStation
                                         + province.poiCount.automaticStation + province.poiCount.caipiao +
                                         province.poiCount.canyin + province.poiCount.daolujiebing +
                                         province.poiCount.dasha
                                         + province.poiCount.dizhizaihai + province.poiCount.fog +
                                         province.poiCount.gaosuService + province.poiCount.gonganjiaojing +
                                         province.poiCount.gonglujixue
                                         + province.poiCount.heliu + province.poiCount.huapo +
                                         province.poiCount.jiaotongchuxing + province.poiCount.jinrongfuwu +
                                         province.poiCount.keyanjiaoyu
                                         + province.poiCount.laboratory + province.poiCount.lvyou +
                                         province.poiCount.messager + province.poiCount.nishiliu +
                                         province.poiCount.parkinglot
                                         + province.poiCount.port + province.poiCount.qiangjiangshui +
                                         province.poiCount.qiaoliang + province.poiCount.qichezhan +
                                         province.poiCount.qixiangStation
                                         + province.poiCount.qixiangStation_new + province.poiCount.river +
                                         province.poiCount.shanfeng + province.poiCount.shanhong +
                                         province.poiCount.shelter
                                         + province.poiCount.shoufeizhan + province.poiCount.shuiku +
                                         province.poiCount.tailings + province.poiCount.torrent + province.poiCount.town
                                         + province.poiCount.tuanwu + province.poiCount.village +
                                         province.poiCount.xinxiyuan + province.poiCount.xiuxianyule +
                                         province.poiCount.yiliaofuwu
                                         + province.poiCount.yinhuandian + province.poiCount.yujingsheshi +
                                         province.poiCount.zhengfujiguan + province.poiCount.zhongxiaoheliu +
                                         province.poiCount.zhusu;
                    heats.Add(porvinceHeat);
                }

                foreach (var province in data.city)
                {
                    var porvinceHeat = new PoiHeat();

                    if (province.coord == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(province.coord.lon) || string.IsNullOrWhiteSpace(province.coord.lat))
                    {
                        continue;
                    }
                    double lon = 0;
                    double lat = 0;
                    if (!double.TryParse(province.coord.lon, out lon) || !double.TryParse(province.coord.lat, out lat))
                    {
                        continue;
                    }
                    porvinceHeat.Lon = lon;
                    porvinceHeat.Lat = lat;
                    porvinceHeat.Code = province.code;
                    porvinceHeat.Level = "city";
                    porvinceHeat.Count = province.poiCount.Airport + province.poiCount.CarService +
                                         province.poiCount.JYZ + province.poiCount.RailwayStation
                                         + province.poiCount.automaticStation + province.poiCount.caipiao +
                                         province.poiCount.canyin + province.poiCount.daolujiebing +
                                         province.poiCount.dasha
                                         + province.poiCount.dizhizaihai + province.poiCount.fog +
                                         province.poiCount.gaosuService + province.poiCount.gonganjiaojing +
                                         province.poiCount.gonglujixue
                                         + province.poiCount.heliu + province.poiCount.huapo +
                                         province.poiCount.jiaotongchuxing + province.poiCount.jinrongfuwu +
                                         province.poiCount.keyanjiaoyu
                                         + province.poiCount.laboratory + province.poiCount.lvyou +
                                         province.poiCount.messager + province.poiCount.nishiliu +
                                         province.poiCount.parkinglot
                                         + province.poiCount.port + province.poiCount.qiangjiangshui +
                                         province.poiCount.qiaoliang + province.poiCount.qichezhan +
                                         province.poiCount.qixiangStation
                                         + province.poiCount.qixiangStation_new + province.poiCount.river +
                                         province.poiCount.shanfeng + province.poiCount.shanhong +
                                         province.poiCount.shelter
                                         + province.poiCount.shoufeizhan + province.poiCount.shuiku +
                                         province.poiCount.tailings + province.poiCount.torrent + province.poiCount.town
                                         + province.poiCount.tuanwu + province.poiCount.village +
                                         province.poiCount.xinxiyuan + province.poiCount.xiuxianyule +
                                         province.poiCount.yiliaofuwu
                                         + province.poiCount.yinhuandian + province.poiCount.yujingsheshi +
                                         province.poiCount.zhengfujiguan + province.poiCount.zhongxiaoheliu +
                                         province.poiCount.zhusu;

                    heats.Add(porvinceHeat);
                }

                foreach (var province in data.country)
                {
                    var porvinceHeat = new PoiHeat();

                    if (province.coord == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(province.coord.lon) || string.IsNullOrWhiteSpace(province.coord.lat))
                    {
                        continue;
                    }
                    double lon = 0;
                    double lat = 0;
                    if (!double.TryParse(province.coord.lon, out lon) || !double.TryParse(province.coord.lat, out lat))
                    {
                        continue;
                    }
                    porvinceHeat.Lon = lon;
                    porvinceHeat.Lat = lat;
                    porvinceHeat.Code = province.code;
                    porvinceHeat.Level = "country";
                    porvinceHeat.Count = province.poiCount.Airport + province.poiCount.CarService +
                                         province.poiCount.JYZ + province.poiCount.RailwayStation
                                         + province.poiCount.automaticStation + province.poiCount.caipiao +
                                         province.poiCount.canyin + province.poiCount.daolujiebing +
                                         province.poiCount.dasha
                                         + province.poiCount.dizhizaihai + province.poiCount.fog +
                                         province.poiCount.gaosuService + province.poiCount.gonganjiaojing +
                                         province.poiCount.gonglujixue
                                         + province.poiCount.heliu + province.poiCount.huapo +
                                         province.poiCount.jiaotongchuxing + province.poiCount.jinrongfuwu +
                                         province.poiCount.keyanjiaoyu
                                         + province.poiCount.laboratory + province.poiCount.lvyou +
                                         province.poiCount.messager + province.poiCount.nishiliu +
                                         province.poiCount.parkinglot
                                         + province.poiCount.port + province.poiCount.qiangjiangshui +
                                         province.poiCount.qiaoliang + province.poiCount.qichezhan +
                                         province.poiCount.qixiangStation
                                         + province.poiCount.qixiangStation_new + province.poiCount.river +
                                         province.poiCount.shanfeng + province.poiCount.shanhong +
                                         province.poiCount.shelter
                                         + province.poiCount.shoufeizhan + province.poiCount.shuiku +
                                         province.poiCount.tailings + province.poiCount.torrent + province.poiCount.town
                                         + province.poiCount.tuanwu + province.poiCount.village +
                                         province.poiCount.xinxiyuan + province.poiCount.xiuxianyule +
                                         province.poiCount.yiliaofuwu
                                         + province.poiCount.yinhuandian + province.poiCount.yujingsheshi +
                                         province.poiCount.zhengfujiguan + province.poiCount.zhongxiaoheliu +
                                         province.poiCount.zhusu;

                    heats.Add(porvinceHeat);
                }
            }

            return heats;
        }

        /// <summary>
        ///     插入POI热力图数据
        /// </summary>
        /// <param name="eventListModel"></param>
        /// <returns></returns>
        public bool InsertConvertPoiHeat(List<PoiHeat> poiHeats)
        {
            var Isbool = false;
            if (poiHeats != null && poiHeats.Count > 0)
            {
                var tableName = "poiheat";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);
                    foreach (var item in poiHeats)
                    {
                        var columnNames = new Dictionary<string, Dictionary<string, string>>();
                        columnNames.Add("Lon", ColumnValueli("int", item.Lon.ToString()));
                        columnNames.Add("Lat", ColumnValueli("int", item.Lat.ToString()));
                        columnNames.Add("Code", ColumnValueli("string", item.Code));
                        columnNames.Add("Level", ColumnValueli("string", item.Level));
                        columnNames.Add("Count", ColumnValueli("int", item.Count.ToString()));
                        columnNames.Add("InsertTime",
                            ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        sqls.Add(sqlstr);
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        /// <summary>
        ///     获取POI数据点
        /// </summary>
        /// <returns></returns>
        public bool GetPoiData()
        {
            var Isbool = true;
            try
            {
                //先清空POI数据
                DeletePoiData();
                //基础信息POI
                //责任人
                //1、拉取数据
                var coordszrr = DataManager.GetPoiCoord("responsibleperson");
                //2、转换数据
                var poiDataszzr = ConvertPoiDatas(coordszrr, "基础信息", "责任人");
                //3、插入数据
                Isbool = InsertConvertPoiData(poiDataszzr);

                //学校
                //1、拉取数据
                var xuexiaoCoords = DataManager.GetPoiCoord("keyanjiaoyu");
                //2、转换数据
                var xuexiaoPOI = ConvertPoiDatas(xuexiaoCoords, "基础信息", "学校");
                //3、插入数据
                Isbool = InsertConvertPoiData(xuexiaoPOI);

                //医院
                //1、拉取数据
                var yiyuancoords = DataManager.GetPoiCoord("yiliaofuwu");
                //2、转换数据
                var yiyuanPOI = ConvertPoiDatas(yiyuancoords, "基础信息", "医院");
                //3、插入数据
                Isbool = InsertConvertPoiData(yiyuanPOI);

                //景点
                //1、拉取数据
                var lvyoucoords = DataManager.GetPoiCoord("lvyou");
                //2、转换数据
                var lvyouPOI = ConvertPoiDatas(lvyoucoords, "基础信息", "景点");
                //3、插入数据
                Isbool = InsertConvertPoiData(lvyouPOI);

                //易燃易爆场所
                //1、拉取数据
                var yrybcoords = DataManager.GetPoiCoord("flaexpplace");
                //2、转换数据
                var yrybPOI = ConvertPoiDatas(yrybcoords, "基础信息", "易燃易爆场所");
                //3、插入数据
                Isbool = InsertConvertPoiData(yrybPOI);

                //水库
                //1、拉取数据
                var skcoords = DataManager.GetPoiCoord("shuiku");
                //2、转换数据
                var skPOI = ConvertPoiDatas(skcoords, "基础信息", "水库");
                //3、插入数据
                Isbool = InsertConvertPoiData(skPOI);

                //信息员
                //1、拉取数据
                var xxycoords = DataManager.GetPoiCoord("messager");
                //2、转换数据
                var xxyPOI = ConvertPoiDatas(xxycoords, "基础信息", "信息员");
                //3、插入数据
                Isbool = InsertConvertPoiData(xxyPOI);

                //预警设备
                //1、拉取数据
                var yjsscoords = DataManager.GetPoiCoord("yujingsheshi");
                //2、转换数据
                var yjssPOI = ConvertPoiDatas(yjsscoords, "基础信息", "预警设备");
                //3、插入数据
                Isbool = InsertConvertPoiData(yjssPOI);

                //隐患点POI
                //中小河流域
                //1、拉取数据
                var zxhlcoords = DataManager.GetPoiCoord("zhongxiaoheliu");
                //2、转换数据
                var zxhlPOI = ConvertPoiDatas(zxhlcoords, "隐患点", "中小河流域");
                //3、插入数据
                Isbool = InsertConvertPoiData(zxhlPOI);

                //山洪沟
                //1、拉取数据
                var torrentcoords = DataManager.GetPoiCoord("torrent");
                //2、转换数据
                var torrentPOI = ConvertPoiDatas(torrentcoords, "隐患点", "山洪沟");
                //3、插入数据
                Isbool = InsertConvertPoiData(torrentPOI);

                //地灾隐患点
                //1、拉取数据
                var yhdcoords = DataManager.GetPoiCoord("yinhuandian");
                //2、转换数据
                var yhdPOI = ConvertPoiDatas(yhdcoords, "隐患点", "地灾隐患点");
                //3、插入数据
                Isbool = InsertConvertPoiData(yhdPOI);

                //城镇易涝点
                //1、拉取数据
                var floodcoords = DataManager.GetPoiCoord("vulnerflood");
                //2、转换数据
                var flood = ConvertPoiDatas(floodcoords, "隐患点", "城镇易涝点");
                //3、插入数据
                Isbool = InsertConvertPoiData(flood);
            }
            catch (Exception ee)
            {
                Isbool = false;
            }

            return Isbool;
        }

        /// <summary>
        ///     转换POI数据点
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="typeclass"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<PoiData> ConvertPoiDatas(List<Coord> coords, string typeclass, string type)
        {
            var poiDatas = new List<PoiData>();

            if (coords == null)
            {
                return poiDatas;
            }

            for (var i = 0; i < coords.Count; i++)
            {
                var coord = coords[i];
                if (string.IsNullOrWhiteSpace(coord.lon) || string.IsNullOrWhiteSpace(coord.lat))
                {
                    continue;
                }
                double lon = 0;
                double lat = 0;
                if (!double.TryParse(coord.lon, out lon) || !double.TryParse(coord.lat, out lat))
                {
                    continue;
                }
                var newData = new PoiData();
                newData.Id = string.Format("{0}-{1}", type, i);
                newData.Class = typeclass;
                newData.Type = type;
                newData.Lon = lon;
                newData.Lat = lat;
                poiDatas.Add(newData);
            }
            return poiDatas;
        }

        /// <summary>
        ///     删除POI点数据
        /// </summary>
        /// <returns></returns>
        public bool DeletePoiData()
        {
            var Isbool = false;
            try
            {
                var tableName = "poidata";
                var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                DBHelper.ExecuteSql(deletesql);
            }
            catch (Exception ex)
            {
                return false;
            }
            return Isbool;
        }

        /// <summary>
        ///     插入POI点数据
        /// </summary>
        /// <param name="eventListModel"></param>
        /// <returns></returns>
        public bool InsertConvertPoiData(List<PoiData> poiDatas)
        {
            var Isbool = false;
            if (poiDatas != null && poiDatas.Count > 0)
            {
                var tableName = "poidata";
                var sqls = new List<string>();
                try
                {
                    foreach (var item in poiDatas)
                    {
                        var columnNames = new Dictionary<string, Dictionary<string, string>>();
                        columnNames.Add("Id", ColumnValueli("string", item.Id));
                        columnNames.Add("Lon", ColumnValueli("int", item.Lon.ToString()));
                        columnNames.Add("Lat", ColumnValueli("int", item.Lat.ToString()));
                        columnNames.Add("Type", ColumnValueli("string", item.Type));
                        columnNames.Add("Class", ColumnValueli("string", item.Class));
                        columnNames.Add("InsertTime",
                            ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        sqls.Add(sqlstr);
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        /// <summary>
        ///     按省市县分类统计Poi点
        /// </summary>
        /// <returns></returns>
        public bool GetPoiTypeCount()
        {
            var poitypes = new List<PoiTypeCount>();
            var Isbool = true;
            try
            {
                //1、拉取数据
                var data = DataManager.getPoiCount();
                if (data == null)
                {
                    return false;
                }
                //2、转换数据
                poitypes = ConvertPoiTypeCount(data);
                if (poitypes == null)
                {
                    return false;
                }
                //3、插入数据
                Isbool = InsertPoiTypeCount(poitypes);
            }
            catch (Exception ee)
            {
                Isbool = false;
            }

            return Isbool;
        }

        /// <summary>
        ///     转换POI分类统计数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<PoiTypeCount> ConvertPoiTypeCount(POIStatistics data)
        {
            var poitypes = new List<PoiTypeCount>();
            if (data != null)
            {
                //转换模型
                foreach (var area in data.province)
                {
                    if (area.coord == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(area.coord.lon) || string.IsNullOrWhiteSpace(area.coord.lat))
                    {
                        continue;
                    }
                    double lon = 0;
                    double lat = 0;
                    if (!double.TryParse(area.coord.lon, out lon) || !double.TryParse(area.coord.lat, out lat))
                    {
                        continue;
                    }
                    var poicount = area.poiCount;

                    //责任人

                    //学校
                    if (poicount.keyanjiaoyu > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "province";
                        poiTypeCount.Type = "学校";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.keyanjiaoyu;
                        poitypes.Add(poiTypeCount);
                    }
                    //医院
                    if (poicount.yiliaofuwu > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "province";
                        poiTypeCount.Type = "医院";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.yiliaofuwu;
                        poitypes.Add(poiTypeCount);
                    }
                    //景点
                    if (poicount.lvyou > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "province";
                        poiTypeCount.Type = "景点";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.lvyou;
                        poitypes.Add(poiTypeCount);
                    }
                    //水库
                    if (poicount.shuiku > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "province";
                        poiTypeCount.Type = "水库";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.shuiku;
                        poitypes.Add(poiTypeCount);
                    }
                    //易燃易爆场所

                    //信息员
                    if (poicount.xinxiyuan > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "province";
                        poiTypeCount.Type = "信息员";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.xinxiyuan;
                        poitypes.Add(poiTypeCount);
                    }
                    //预警设备
                    if (poicount.yujingsheshi > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "province";
                        poiTypeCount.Type = "信息员";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.yujingsheshi;
                        poitypes.Add(poiTypeCount);
                    }
                    //隐患点POI
                    //中小河流域
                    if (poicount.zhongxiaoheliu > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "province";
                        poiTypeCount.Type = "中小河流域";
                        poiTypeCount.Class = "隐患点";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.zhongxiaoheliu;
                        poitypes.Add(poiTypeCount);
                    }

                    //山洪沟
                    if (poicount.shanhong > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "province";
                        poiTypeCount.Type = "山洪沟";
                        poiTypeCount.Class = "隐患点";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.shanhong;
                        poitypes.Add(poiTypeCount);
                    }

                    //地灾隐患点
                    if (poicount.dizhizaihai > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "province";
                        poiTypeCount.Type = "地灾隐患点";
                        poiTypeCount.Class = "隐患点";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.dizhizaihai;
                        poitypes.Add(poiTypeCount);
                    }

                    //城镇易涝点
                }

                foreach (var area in data.city)
                {
                    if (area.coord == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(area.coord.lon) || string.IsNullOrWhiteSpace(area.coord.lat))
                    {
                        continue;
                    }
                    double lon = 0;
                    double lat = 0;
                    if (!double.TryParse(area.coord.lon, out lon) || !double.TryParse(area.coord.lat, out lat))
                    {
                        continue;
                    }
                    var poicount = area.poiCount;

                    //责任人

                    //学校
                    if (poicount.keyanjiaoyu > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "city";
                        poiTypeCount.Type = "学校";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.keyanjiaoyu;
                        poitypes.Add(poiTypeCount);
                    }
                    //医院
                    if (poicount.yiliaofuwu > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "city";
                        poiTypeCount.Type = "医院";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.yiliaofuwu;
                        poitypes.Add(poiTypeCount);
                    }
                    //景点
                    if (poicount.lvyou > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "city";
                        poiTypeCount.Type = "景点";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.lvyou;
                        poitypes.Add(poiTypeCount);
                    }
                    //水库
                    if (poicount.shuiku > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "city";
                        poiTypeCount.Type = "水库";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.shuiku;
                        poitypes.Add(poiTypeCount);
                    }
                    //易燃易爆场所

                    //信息员
                    if (poicount.xinxiyuan > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "city";
                        poiTypeCount.Type = "信息员";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.xinxiyuan;
                        poitypes.Add(poiTypeCount);
                    }
                    //预警设备
                    if (poicount.yujingsheshi > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "city";
                        poiTypeCount.Type = "信息员";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.yujingsheshi;
                        poitypes.Add(poiTypeCount);
                    }
                    //隐患点POI
                    //中小河流域
                    if (poicount.zhongxiaoheliu > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "city";
                        poiTypeCount.Type = "中小河流域";
                        poiTypeCount.Class = "隐患点";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.zhongxiaoheliu;
                        poitypes.Add(poiTypeCount);
                    }

                    //山洪沟
                    if (poicount.shanhong > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "city";
                        poiTypeCount.Type = "山洪沟";
                        poiTypeCount.Class = "隐患点";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.shanhong;
                        poitypes.Add(poiTypeCount);
                    }

                    //地灾隐患点
                    if (poicount.dizhizaihai > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "city";
                        poiTypeCount.Type = "地灾隐患点";
                        poiTypeCount.Class = "隐患点";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.dizhizaihai;
                        poitypes.Add(poiTypeCount);
                    }

                    //城镇易涝点
                }

                foreach (var area in data.country)
                {
                    if (area.coord == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(area.coord.lon) || string.IsNullOrWhiteSpace(area.coord.lat))
                    {
                        continue;
                    }
                    double lon = 0;
                    double lat = 0;
                    if (!double.TryParse(area.coord.lon, out lon) || !double.TryParse(area.coord.lat, out lat))
                    {
                        continue;
                    }
                    var poicount = area.poiCount;

                    //责任人

                    //学校
                    if (poicount.keyanjiaoyu > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "country";
                        poiTypeCount.Type = "学校";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.keyanjiaoyu;
                        poitypes.Add(poiTypeCount);
                    }
                    //医院
                    if (poicount.yiliaofuwu > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "country";
                        poiTypeCount.Type = "医院";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.yiliaofuwu;
                        poitypes.Add(poiTypeCount);
                    }
                    //景点
                    if (poicount.lvyou > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "country";
                        poiTypeCount.Type = "景点";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.lvyou;
                        poitypes.Add(poiTypeCount);
                    }
                    //水库
                    if (poicount.shuiku > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "country";
                        poiTypeCount.Type = "水库";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.shuiku;
                        poitypes.Add(poiTypeCount);
                    }
                    //易燃易爆场所

                    //信息员
                    if (poicount.xinxiyuan > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "country";
                        poiTypeCount.Type = "信息员";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.xinxiyuan;
                        poitypes.Add(poiTypeCount);
                    }
                    //预警设备
                    if (poicount.yujingsheshi > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "country";
                        poiTypeCount.Type = "信息员";
                        poiTypeCount.Class = "基础信息";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.yujingsheshi;
                        poitypes.Add(poiTypeCount);
                    }
                    //隐患点POI
                    //中小河流域
                    if (poicount.zhongxiaoheliu > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "country";
                        poiTypeCount.Type = "中小河流域";
                        poiTypeCount.Class = "隐患点";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.zhongxiaoheliu;
                        poitypes.Add(poiTypeCount);
                    }

                    //山洪沟
                    if (poicount.shanhong > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "country";
                        poiTypeCount.Type = "山洪沟";
                        poiTypeCount.Class = "隐患点";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.shanhong;
                        poitypes.Add(poiTypeCount);
                    }

                    //地灾隐患点
                    if (poicount.dizhizaihai > 0)
                    {
                        var poiTypeCount = new PoiTypeCount();
                        poiTypeCount.Lon = lon;
                        poiTypeCount.Lat = lat;
                        poiTypeCount.Level = "country";
                        poiTypeCount.Type = "地灾隐患点";
                        poiTypeCount.Class = "隐患点";
                        poiTypeCount.Code = area.code;
                        poiTypeCount.Count = poicount.dizhizaihai;
                        poitypes.Add(poiTypeCount);
                    }

                    //城镇易涝点
                }
            }

            return poitypes;
        }

        /// <summary>
        ///     插入POI热力图数据
        /// </summary>
        /// <param name="eventListModel"></param>
        /// <returns></returns>
        public bool InsertPoiTypeCount(List<PoiTypeCount> poiHeats)
        {
            var Isbool = false;
            if (poiHeats != null && poiHeats.Count > 0)
            {
                var tableName = "poitypecount";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);
                    foreach (var item in poiHeats)
                    {
                        var columnNames = new Dictionary<string, Dictionary<string, string>>();
                        columnNames.Add("Lon", ColumnValueli("int", item.Lon.ToString()));
                        columnNames.Add("Lat", ColumnValueli("int", item.Lat.ToString()));
                        columnNames.Add("Code", ColumnValueli("string", item.Code));
                        columnNames.Add("Level", ColumnValueli("string", item.Level));
                        columnNames.Add("Count", ColumnValueli("int", item.Count.ToString()));
                        columnNames.Add("Type", ColumnValueli("string", item.Type));
                        columnNames.Add("Class", ColumnValueli("string", item.Class));
                        columnNames.Add("InsertTime",
                            ColumnValueli("string", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        var sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        sqls.Add(sqlstr);
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        #endregion

        #endregion

        #region 综合查询

        /// <summary>
        ///     综合查询——突发事件
        /// </summary>
        /// <param name="eventList"></param>
        /// <returns></returns>
        public EventListModel findEmergencise(EventListEnter eventList)
        {
            var eventListModel = new EventListModel();
            var Isbool = false;
            try
            {
                //1、拉取数据
                var data = DataManager.findEmergencise(eventList);
                //2、转换数据
                eventListModel = ConvertEventListModel(data);
                if (eventListModel == null)
                {
                    UpdateMessage(false, "综合查询突发事件_GetRiskSituationList");
                }
            }
            catch (Exception ee)
            {
                Isbool = false;
                UpdateMessage(false, "综合查询突发事件_GetRiskSituationList");
            }
            return eventListModel;
        }

        /// <summary>
        ///     综合查询_台风事件
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<TyphoonEventsModel> searchTyphoon(string beginTime, string endTime)
        {
            var typhoonEventsModels = new List<TyphoonEventsModel>();
            try
            {
                //1、拉取数据
                var data = DataManager.searchTyphoon(beginTime, endTime);

                //2、转换数据
                typhoonEventsModels = ConvertTyphoonModel(data);
                if (typhoonEventsModels == null)
                {
                    UpdateMessage(false, "综合查询台风事件_searchTyphoon");
                }
            }
            catch (Exception ee)
            {
                UpdateMessage(false, "综合查询台风事件_searchTyphoon");
            }
            return typhoonEventsModels;
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
        public List<RiskSituationModel> getWarningByEventType(string senderCode, string msgType, string eventType,
            string severity, string beginTime, string endTime)
        {
            var riskSituationModels = new List<RiskSituationModel>();
            try
            {
                //1、拉取数据
                var data = DataManager.getWarningByEventType(senderCode, msgType, eventType, severity, beginTime,
                    endTime);

                //2、转换数据
                riskSituationModels = ConvertRiskSituationModel(data);
                if (riskSituationModels == null)
                {
                    UpdateMessage(false, "综合查询预警信息_getWarningByEventType");
                }
            }
            catch (Exception ee)
            {
                UpdateMessage(false, "综合查询预警信息_GetRiskSituationList");
            }
            return riskSituationModels;
        }

        #endregion

        #region 数据入库

        /// <summary>
        /// 台风数据入库
        /// </summary>
        /// <param name="typhoonDataInfos"></param>
        /// <returns></returns>
        public bool Typhoon(List<TyphoonDataInfo> typhoonDataInfos)
        {
            var Isbool = false;
            if (typhoonDataInfos != null && typhoonDataInfos.Count > 0)
            {
                var tableName = "typhoonpath";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);

                    foreach (var items in typhoonDataInfos)
                    {
                        var sqlstr = "";
                        var dateTime = DateTime.Now.AddSeconds(5).ToString("yyyy-MM-dd HH:mm:ss");

                        var columnNames = new Dictionary<string, Dictionary<string, string>>();
                        //columnNames.Add("Id", ColumnValueli("string", items.id));
                        columnNames.Add("Id", ColumnValueli("string", items.bj_datetime));
                        columnNames.Add("Lon", ColumnValueli("string", items.lon));
                        columnNames.Add("Lat", ColumnValueli("string", items.lat));
                        columnNames.Add("Alt", ColumnValueli("string", "0"));
                        columnNames.Add("Color", ColumnValueli("string", items.trank));
                        columnNames.Add("TyphoonId", ColumnValueli("string", items.typhoon_id));
                        columnNames.Add("Name", ColumnValueli("string", items.typhoon_name_cn));
                        columnNames.Add("EnglishName", ColumnValueli("string", items.typhoon_name));
                        columnNames.Add("ArriveTime", ColumnValueli("string", items.bj_datetime));
                        columnNames.Add("Center", ColumnValueli("string", items.lon + "°E," + items.lat + "°N"));
                        columnNames.Add("Wind", ColumnValueli("string", items.windspeed));
                        columnNames.Add("Pascal", ColumnValueli("string", items.pressure));
                        columnNames.Add("FutureSpeed", ColumnValueli("string", items.gust));
                        columnNames.Add("FutureDirection", ColumnValueli("string", items.direction));
                        columnNames.Add("Conclusion", ColumnValueli("string", items.conclusion));
                        columnNames.Add("Time", ColumnValueli("string", dateTime));
                        sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                        sqls.Add(sqlstr);

                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        /// <summary>
        /// 预警台风数据入库
        /// </summary>
        /// <param name="typhoonDataInfos"></param>
        /// <returns></returns>
        public bool WarningTyphoon(List<List<WarningTyphoonDataInfo>> typhoonDataInfos)
        {
            var Isbool = false;
            if (typhoonDataInfos != null && typhoonDataInfos.Count > 0)
            {
                var tableName = "typhoonpath";
                var sqls = new List<string>();
                try
                {
                    var deletesql = ShareExecuteSqlstr(tableName, "Delete");
                    DBHelper.ExecuteSql(deletesql);

                    foreach (var typhoonData in typhoonDataInfos)
                    {
                        foreach (WarningTyphoonDataInfo item in typhoonData)
                        {
                            var sqlstr = "";
                            var dateTime = DateTime.Now.AddSeconds(10).ToString("yyyy-MM-dd HH:mm:ss");

                            var columnNames = new Dictionary<string, Dictionary<string, string>>();
                            columnNames.Add("Id", ColumnValueli("string", item.id + item.bj_datetime));
                            columnNames.Add("Lon", ColumnValueli("string", item.lon));
                            columnNames.Add("Lat", ColumnValueli("string", item.lat));
                            columnNames.Add("Alt", ColumnValueli("string", "0"));
                            columnNames.Add("Color", ColumnValueli("string", "TS"));
                            columnNames.Add("TyphoonId", ColumnValueli("string", item.id));
                            columnNames.Add("Name", ColumnValueli("string", ""));
                            columnNames.Add("EnglishName", ColumnValueli("string", ""));
                            columnNames.Add("ArriveTime", ColumnValueli("string", item.bj_datetime));
                            columnNames.Add("Center", ColumnValueli("string", item.lon + "°E," + item.lat + "°N"));
                            columnNames.Add("Wind", ColumnValueli("string", ""));
                            columnNames.Add("Pascal", ColumnValueli("string", ""));
                            columnNames.Add("FutureSpeed", ColumnValueli("string", ""));
                            columnNames.Add("FutureDirection", ColumnValueli("string", ""));
                            columnNames.Add("Conclusion", ColumnValueli("string", ""));
                            columnNames.Add("Time", ColumnValueli("string", dateTime));
                            sqlstr = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                            sqls.Add(sqlstr);
                        }
                    }
                    Isbool = DBHelper.ExecuteSqlBatch(sqls);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return Isbool;
        }

        /// <summary>
        /// 最邻近站点数据入库
        /// </summary>
        /// <param name="closestStations">邻近站集合</param>
        /// <returns></returns>
        public async void ClosestStationToDatabaseAsync(List<ClosestStation> closestStations)
        {
            await Task.Run(() =>
            {
                var isBool = false;
                if (closestStations != null && closestStations.Count > 0)
                {
                    var tableName = "ClosestStation";
                    var sqls = new List<string>();
                    try
                    {
                        var deleteSql = ShareExecuteSqlstr(tableName, "Delete");
                        DBHelper.ExecuteSql(deleteSql);

                        foreach (var items in closestStations)
                        {
                            var dateTime = DateTime.Now.AddSeconds(5).ToString("yyyy-MM-dd HH:mm:ss");

                            var columnNames = new Dictionary<string, Dictionary<string, string>>
                            {
                                {"Id", ColumnValueli("string", items.stationId)},
                                {"Lon", ColumnValueli("string", items.lon)},
                                {"Lat", ColumnValueli("string", items.lat)},
                                {"Alt", ColumnValueli("string", items.alti)},
                                {"Name", ColumnValueli("string", items.stationNa)},
                                {"Time", ColumnValueli("string", dateTime)}
                            };
                            string sqlString = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                            sqls.Add(sqlString);

                        }
                        isBool = DBHelper.ExecuteSqlBatch(sqls);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return isBool;
            });

        }

        /// <summary>
        /// 突发事件数据入库
        /// </summary>
        /// <param name="eventInfo">邻近站集合</param>
        /// <returns></returns>
        public async void EventInsertToDatabaseAsync(EventPageInto eventInfo,List<ClosestStation> closestStations)
        {
            await Task.Run(() =>
            {
                // 清除原数据
                var tableName = "eventpage_poi";
                var sqls = new List<string>();
                var deleteSql = ShareExecuteSqlstr(tableName, "Delete");
                DBHelper.ExecuteSql(deleteSql);

                var sqlString = "";
                var dateTime = DateTime.Now.AddSeconds(5).ToString("yyyy-MM-dd HH:mm:ss");

                var columnNames = new Dictionary<string, Dictionary<string, string>>
                {
                    {"ID", ColumnValueli("string", Guid.NewGuid().ToString())},
                    {"Name", ColumnValueli("string", eventInfo.EventTitle)},
                    {"EventType", ColumnValueli("string", "")},
                    {"Location", ColumnValueli("string", eventInfo.EventPlace)},
                    {"Longitude", ColumnValueli("string", eventInfo.EventLon.ToString())},
                    {"Latitude", ColumnValueli("string", eventInfo.EventLat.ToString())},
                    {"ShowTitle", ColumnValueli("string", eventInfo.EventTitle)},
                    {"Time", ColumnValueli("string", eventInfo.EventBeginTime)},
                    {"Detail", ColumnValueli("string", eventInfo.EventDescription)},
                    {"Source", ColumnValueli("string", "")},
                    {"IsHistory", ColumnValueli("string", "")},
                    {"InsertTime", ColumnValueli("string", dateTime)},
                    {"IsToday", ColumnValueli("string", "")},
                    {"Alt", ColumnValueli("string", closestStations.FirstOrDefault().alti)}
                };
                sqlString = ShareExecuteSqlstr(tableName, "Insert", columnNames);
                sqls.Add(sqlString);

                DBHelper.ExecuteSqlBatch(sqls);
            });
        }

        #endregion

    }
}