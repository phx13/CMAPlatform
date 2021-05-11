using Microsoft.Practices.Prism.ViewModel;

namespace CMAPlatform.Models
{
    public class TyphoonDataInfoModel : NotificationObject
    {
        private string m_Name;
        /// <summary>
        /// 台风名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string m_Id;
        /// <summary>
        /// 台风编号
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set
            {
                m_Id = value;
                RaisePropertyChanged("Id");
            }
        }

        private string m_Lon;
        /// <summary>
        /// 台风中心经度
        /// </summary>
        public string Lon
        {
            get { return m_Lon; }
            set
            {
                m_Lon = value;
                RaisePropertyChanged("Lon");
            }
        }

        private string m_Lat;
        /// <summary>
        /// 台风中心纬度
        /// </summary>
        public string Lat
        {
            get { return m_Lat; }
            set
            {
                m_Lat = value;
                RaisePropertyChanged("Lat");
            }
        }

        private string m_Time;
        /// <summary>
        /// 台风登录时间
        /// </summary>
        public string Time
        {
            get { return m_Time; }
            set
            {
                m_Time = value;
                RaisePropertyChanged("Time");
            }
        }

        private string m_Intension;
        /// <summary>
        /// 台风轻度
        /// </summary>
        public string Intension
        {
            get { return m_Intension; }
            set
            {
                m_Intension = value;
                RaisePropertyChanged("Intension");
            }
        }

        private string m_Speed;
        /// <summary>
        /// 台风风速
        /// </summary>
        public string Speed
        {
            get { return m_Speed; }
            set
            {
                m_Speed = value;
                RaisePropertyChanged("Speed");
            }
        }

        private string m_Pressure;
        /// <summary>
        /// 台风压强
        /// </summary>
        public string Pressure
        {
            get { return m_Pressure; }
            set
            {
                m_Pressure = value;
                RaisePropertyChanged("Pressure");
            }
        }

        private string m_Gust;
        /// <summary>
        /// 台风移速
        /// </summary>
        public string Gust
        {
            get { return m_Gust; }
            set
            {
                m_Gust = value;
                RaisePropertyChanged("Gust");
            }
        }

        private string m_Direction;
        /// <summary>
        /// 台风移向
        /// </summary>
        public string Direction
        {
            get { return m_Direction; }
            set
            {
                m_Direction = value;
                RaisePropertyChanged("Direction");
        }
        }
    }
}