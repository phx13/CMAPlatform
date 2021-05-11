using System;
using System.ComponentModel;
using System.IO;
using System.Web.Script.Services;
using System.Web.Services;

namespace CMAPlatform.TestService
{
    /// <summary>
    ///     WebServiceTest 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [ScriptService]
    public class WebServiceTest : WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string TestString(string a, string b)
        {
            return a + b;
        }

        #region

        /// <summary>
        ///     全国大喇叭及合格率
        ///     全国显示屏及合格率
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getYJSSGeneralData()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/全国大喇叭显示屏及合格率.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     信息员、灾情上报情况
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getXXYGeneralData()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/信息员情况.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     预警总数信息员数量统计
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getCountOfEffectWarning()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/全国预警生效总数红橙总数.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     获取预警准确率
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getAccuracyOfWarning()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/预警质量检测正确率.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     获取34个省份数据
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string infos()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/省份数据.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     省份详细数据
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string percent(string province)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/省份详细数据.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }


        /// <summary>
        ///     首页获取突发事件列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getRecEmergencise(int limit)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/突发事件.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     获取风险态势列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getStatisticsWarningRegin()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/风险态势.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     得到省市县分级POI统计信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getPoiCount()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/散点数据.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }


        /// <summary>
        ///     根据隐患点类型，得到隐患点经纬度。
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getCoord(string poiType)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/POI坐标数组.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }


        /// <summary>
        ///     综合查询-突发事件
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [WebMethod]
        public string findEmergencise(string province, string city, string country, string Begintime, string endtime,
            string eventType)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/突发事件.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     综合查询_风险态势_台风事件
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [WebMethod]
        public string searchTyphoon(string beginTime, string endTime)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/台风事件.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     综合查询_风险态势_预警信息_类型查询
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [WebMethod]
        public string getWarningByEventType(string msgType, string eventType, string severity,
            string beginTime, string endTime)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/综合查询/综合查询_风险态势_预警信息按类型.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     综合查询_风险态势_预警信息_地域查询
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [WebMethod]
        public string getWarningByArea(string senderCode, string msgType, string eventType, string severity,
            string beginTime, string endTime)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/综合查询/综合查询_风险态势_预警信息按地域.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     舆情热点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [WebMethod]
        public string action(string start, string end, string limit)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/舆情热点.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        #endregion

        #region 事件页测试接口

        /// <summary>
        ///     获取责任区/警戒区/监视区范围
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <param name="county">县</param>
        /// <returns></returns>
        [WebMethod]
        public string code(string areaCode, string areaType)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = "";
            if (areaType == "C")
            {
                filePath = Path.Combine(directory, "Data/事件页/责任区警戒区监视区范围C.json");
            }
            else if (areaType == "E")
            {
                filePath = Path.Combine(directory, "Data/事件页/责任区警戒区监视区范围E.json");
            }
            else
            {
                filePath = Path.Combine(directory, "Data/事件页/责任区警戒区监视区范围R.json");
            }
            var Content = File.ReadAllText(filePath);
            return Content;
        }


        /// <summary>
        ///     获取预警概要描述
        /// </summary>
        /// <param name="identifier">identifier</param>
        /// <returns></returns>
        [WebMethod]
        public string getWarningByIdentifier(string identifier)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/事件页/预警概要描述.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }


        /// <summary>
        ///     单条预警的信息员发布情况
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns></returns>
        [WebMethod]
        public string getXXYGeneralDataAboutEarlyWarning(string id, string hyd)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/事件页/单条预警的信息员发布情况.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        #region 泳道图部分接口

        /// <summary>
        ///     获取相关预警事件列表
        /// </summary>
        /// <param name="identifier">预警id</param>
        /// <param name="eventType">预警事件类型</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [WebMethod]
        public string getWarningByTime(string identifier, string eventType, string beginTime, string endTime)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/事件页-泳道图/泳道图-预警.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }


        /// <summary>
        ///     获取某条预警的相关服务（接口未定）
        /// </summary>
        /// <param name="id">identifier</param>
        /// <param name="start">identifier</param>
        /// <param name="end">identifier</param>
        /// <returns></returns>
        [WebMethod]
        public string getTimelineServices(string id, string start, string end)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/事件页-泳道图/决策服务产品-全部.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }


        /// <summary>
        ///     获取达到红橙黄蓝预警指标的气象站个数
        /// </summary>
        /// <param name="AdminCode">行政区划编码</param>
        /// <param name="EventType">预警类型</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        [WebMethod]
        public string GetWarningNumber(string AdminCode, string EventType, string StartTime, string EndTime)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/事件页-泳道图/泳道图-站数统计.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     获取符合预警指标气象站的时刻数据
        /// </summary>
        /// <param name="EventType">预警类型</param>
        /// <param name="Stations">站点</param>
        /// <param name="Time">时间</param>
        /// <returns></returns>
        [WebMethod]
        public string GetWarningStationData(string EventType, string Stations, string Time)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/事件页-泳道图/泳道图-站数据弹窗.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }

        /// <summary>
        ///     最大值
        /// </summary>
        /// <param name="paramsJson">Json参数</param>
        /// <returns></returns>
        [WebMethod]
        public string getPeakDataByTimeRegionAndRegion(string paramsJson)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(directory, "Data/事件页-泳道图/泳道图-最大值.json");
            var Content = File.ReadAllText(filePath);
            return Content;
        }


        //[WebMethod]
        //public string getPeakDataByTimeRegionAndRegion()
        //{
        //    string directory = AppDomain.CurrentDomain.BaseDirectory;
        //    string filePath = System.IO.Path.Combine(directory, "Data/事件页-泳道图/泳道图-最大值.json");
        //    string Content = File.ReadAllText(filePath);
        //    return Content;
        //}

        #endregion

        #endregion
    }
}