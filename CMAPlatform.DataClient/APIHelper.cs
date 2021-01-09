using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CMAPlatform.DataClient
{
    public class APIHelper
    {
        /// <summary>
        ///     GET方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            var result = "";
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                //request.ContentType = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "GET";

                var resp = (HttpWebResponse) request.GetResponse();
                var stream = resp.GetResponseStream();
                //获取响应内容
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();

                    if (DataManager.Instance.IsTestMode)
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(result);
                        result = doc.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
            }


            return result;
        }

        /// <summary>
        ///     GET方法 带参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string Get(string url, Dictionary<string, string> dictionary)
        {
            var result = "";
            try
            {
                var values = new List<KeyValuePair<string, string>>();

                foreach (var key in dictionary.Keys)
                {
                    values.Add(new KeyValuePair<string, string>(key, dictionary[key]));
                }

                #region 添加Get 参数

                var builder = new StringBuilder();
                if (values.Count > 0)
                {
                    builder.Append("?");
                }
                var i = 0;
                foreach (var item in values)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }

                #endregion

                url += builder.ToString();


                var request = (HttpWebRequest) WebRequest.Create(url);
                //request.ContentType = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.Method = "POST";
                request.Method = "GET";

                var x = new XmlSerializer(typeof (string));
                var resp = (HttpWebResponse) request.GetResponse();
                var stream = resp.GetResponseStream();
                //获取响应内容
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();

                    if (DataManager.Instance.IsTestMode)
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(result);
                        result = doc.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
            }

            return result;
        }

        /// <summary>
        ///     POST获取Json数据带参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetJson(string url, Dictionary<string, string> dictionary)
        {
            var result = "";
            try
            {
                var values = dictionary;


                var request = (HttpWebRequest) WebRequest.Create(url);
                //request.ContentType = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                #region 添加Post 参数

                var builder = new StringBuilder();
                var i = 0;
                foreach (var item in values)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
                var data = Encoding.UTF8.GetBytes(builder.ToString());
                request.ContentLength = data.Length;
                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }

                #endregion

                var resp = (HttpWebResponse) request.GetResponse();
                var stream = resp.GetResponseStream();
                //获取响应内容
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = "";
            }

            return result;
        }

        /// <summary>
        ///     Post方法 参数在头位置
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dictionary">键值对</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> dictionary)
        {
            var result = "";
            try
            {
                var values = new List<KeyValuePair<string, string>>();

                foreach (var key in dictionary.Keys)
                {
                    values.Add(new KeyValuePair<string, string>(key, dictionary[key]));
                }

                #region 添加POST 参数

                var builder = new StringBuilder();
                if (values.Count > 0)
                {
                    builder.Append("?");
                }
                var i = 0;
                foreach (var item in values)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }

                #endregion

                url += builder.ToString();


                var request = (HttpWebRequest) WebRequest.Create(url);
                //request.ContentType = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                //request.Method = "GET";
                request.ContentLength = 0;
                var x = new XmlSerializer(typeof (string));
                var resp = (HttpWebResponse) request.GetResponse();
                var stream = resp.GetResponseStream();
                //获取响应内容
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();

                    if (DataManager.Instance.IsTestMode)
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(result);
                        result = doc.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
            }

            return result;
        }
    }
}