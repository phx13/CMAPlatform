using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CMAPlatform.DataClient;
using CMAPlatform.Models;
using Digihail.AVE.Launcher.Controls;

namespace CMAPlatform.Chart.Window
{
    /// <summary>
    ///     PreventionMessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PreventionMessageWindow : PopWindow, INotifyPropertyChanged
    {
        /// <summary>
        ///     浏览器控件
        /// </summary>
        private readonly string m_Province;

        private List<ProventionMessageModel> m_ProventionMessage;

        /// <summary>
        ///     当前url
        /// </summary>
        private string m_Url = "";

        public PreventionMessageWindow(string province)
        {
            InitializeComponent();
            m_Province = province;
            Loaded += PreventionMessageWindow_Loaded;
        }

        public List<ProventionMessageModel> ProventionMessage
        {
            get { return m_ProventionMessage; }
            set
            {
                m_ProventionMessage = value;
                OnPropertyChanged("ProventionMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void PreventionMessageWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SelectData();
        }

        public void SelectData()
        {
            var createData = new CreateData();
            var DataSource = new Dictionary<bool, List<ProventionMessageModel>>();
            DataSource = createData.percent(m_Province);

            ProventionMessage = DataSource.Values.FirstOrDefault();
            DataContext = ProventionMessage;
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void GoToUrl_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var url = Properties.Resources.ResourceManager.GetString(m_Province);
            if (string.IsNullOrWhiteSpace(url))
            {
                url = "http://www.baidu.com";
            }

            Process.Start("iexplore.exe", url);
        }
    }
}