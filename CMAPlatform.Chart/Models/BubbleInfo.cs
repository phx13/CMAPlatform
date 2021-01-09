using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace CMAPlatform.Chart.Models
{
    public class BubbleInfo : INotifyPropertyChanged
    {
        private double m_Angle;


        private Brush m_Color;

        private double m_Diameter;
        private int m_Id;

        private int m_IsSelected;


        private string m_Name;

        private string m_NameTitle;

        private double m_Radius;


        private string m_TypeTitle;


        private double m_Value;

        private string m_ValueTitle;

        private double m_X;

        private double m_Y;

        /// <summary>
        ///     唯一标识符（从0开始）
        /// </summary>
        public int Id
        {
            get { return m_Id; }
            set
            {
                m_Id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        ///     类型标题
        /// </summary>
        public string TypeTitle
        {
            get { return m_TypeTitle; }
            set
            {
                m_TypeTitle = value;
                OnPropertyChanged("TypeTitle");
            }
        }

        /// <summary>
        ///     类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     名称标题
        /// </summary>
        public string NameTitle
        {
            get { return m_NameTitle; }
            set
            {
                m_NameTitle = value;
                OnPropertyChanged("NameTitle");
            }
        }

        /// <summary>
        ///     名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        ///     值标题
        /// </summary>
        public string ValueTitle
        {
            get { return m_ValueTitle; }
            set
            {
                m_ValueTitle = value;
                OnPropertyChanged("ValueTitle");
            }
        }

        /// <summary>
        ///     值
        /// </summary>
        public double Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        ///     气泡颜色
        /// </summary>
        public Brush Color
        {
            get { return m_Color; }
            set
            {
                m_Color = value;
                OnPropertyChanged("Color");
            }
        }

        /// <summary>
        ///     X坐标(相对圆心)
        /// </summary>
        public double X
        {
            get { return m_X; }
            set
            {
                m_X = value;
                Left = value - m_Radius;
                OnPropertyChanged("X");
            }
        }

        /// <summary>
        ///     y坐标(相对圆心)
        /// </summary>
        public double Y
        {
            get { return m_Y; }
            set
            {
                m_Y = value;
                Top = value - m_Radius;
                OnPropertyChanged("Y");
            }
        }

        /// <summary>
        ///     上边间距
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        ///     左边间距
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        ///     半经
        /// </summary>
        public double Radius
        {
            get { return m_Radius; }
            set
            {
                m_Radius = value;
                m_Diameter = value*2;
                OnPropertyChanged("Radius");
            }
        }

        /// <summary>
        ///     直经
        /// </summary>
        public double Diameter
        {
            get { return m_Diameter; }
            set
            {
                m_Diameter = value;
                OnPropertyChanged("Diameter");
            }
        }

        /// <summary>
        ///     相对中心点角度
        /// </summary>
        public double Angle
        {
            get { return m_Angle; }
            set
            {
                m_Angle = value;

                // 象限
                Quadrant = Common.GetQuadrant(m_Angle);
            }
        }

        /// <summary>
        ///     相对中心点象限
        /// </summary>
        public int Quadrant { get; set; }

        /// <summary>
        ///     相切气泡
        /// </summary>
        public List<BubbleInfo> Touchs {
            get
            {
                return 
                new List<BubbleInfo>();
            }}

        /// <summary>
        ///     选择状态(0: 默认状态 1：选中状态 -1：没有选中状态)
        /// </summary>
        public int IsSelected
        {
            get { return m_IsSelected; }
            set
            {
                m_IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        ///     属性变更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     属性变更通知
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}