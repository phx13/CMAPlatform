using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CMAPlatform.Models;
using Digihail.AVE.Controls.Utils;

namespace CMAPlatform.DataClient
{
    public class CommonHelper
    {
        #region 单件

        private static CommonHelper m_Instance;
        private static readonly object m_Obj = new object();

        public static CommonHelper Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (m_Obj)
                    {
                        if (m_Instance == null)
                        {
                            m_Instance = new CommonHelper();

                            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EventPageView.xml");

                            if (!File.Exists(path))
                            {
                                m_Instance.EventPageViews = m_Instance.CreateDefaultEventPageViews();
                                SerializeHelper.SerializeToFile(m_Instance.EventPageViews, path, null);
                            }
                            else
                            {
                                m_Instance.EventPageViews =
                                    SerializeHelper.DeserializeFromFile<List<EventPageViewModel>>(path, null);
                            }
                        }
                    }
                }
                return m_Instance;
            }
        }

        /// <summary>
        ///     视角
        /// </summary>
        public List<EventPageViewModel> EventPageViews { get; private set; }

        /// <summary>
        ///     创建默认视角文件
        /// </summary>
        private List<EventPageViewModel> CreateDefaultEventPageViews()
        {
            var results = new List<EventPageViewModel>();

            var nation = new EventPageViewModel
            {
                Code = "000000",
                Height = 8200000,
                Yaw = -13.9,
                Pitch = -62.5,
                IsDefault = true
            };
            results.Add(nation);

            var province = new EventPageViewModel
            {
                Code = "XX0000",
                Height = 750000,
                Yaw = -13.9,
                Pitch = -62.5,
                IsDefault = true
            };
            results.Add(province);

            var city = new EventPageViewModel
            {
                Code = "XXXX00",
                Height = 210000,
                Yaw = -13.9,
                Pitch = -62.5,
                IsDefault = true
            };
            results.Add(city);

            var county = new EventPageViewModel
            {
                Code = "XXXXXX",
                Height = 100000,
                Yaw = 0,
                Pitch = -1,
                IsDefault = true
            };
            results.Add(county);

            return results;
        }


        public EventPageViewModel FindEventPageViewModel(string code)
        {
            if (code == null)
            {
                code = "000000";
            }
            EventPageViewModel result = null;
            //先找匹配视角
            result = EventPageViews.FirstOrDefault(t => t.Code == code);
            //再找默认视角
            if (result == null)
            {
                if (code.EndsWith("0000"))
                {
                    result = EventPageViews.FirstOrDefault(t => t.Code == "XX0000");
                }
                else if (code.EndsWith("00"))
                {
                    result = EventPageViews.FirstOrDefault(t => t.Code == "XXXX00");
                }
                else
                {
                    result = EventPageViews.FirstOrDefault(t => t.Code == "XXXXXX");
                }
            }

            return result;
        }

        #endregion
    }
}