using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using CMAPlatform.DataClient;
using CMAPlatform.Models;

namespace CMAPlatform.TestWindow
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //POIStatistics po = new POIStatistics();
            //po = DataManager.getPoiCount();

            //List<Coord> po = DataManager.GetPoiCoord("zhongxiaoheliu");

            #region  构建查询地区 测试

            //AreaData data = new AreaData();
            //data.provinces = new ProvinceData[1];

            //ProvinceData provinceData = new ProvinceData();
            //provinceData.provincename = "福建省";
            //provinceData.code = new[] { "350000" };
            //provinceData.citys = new CityData[1];

            //data.provinces[0] = provinceData;

            //CityData cityData1 = new CityData();
            //cityData1.cityname = "龙岩市";
            //cityData1.code = new[] { "350800" };
            ////cityData1.countys = new[] {"新罗区", "永定区", "长汀县", "上杭县", "武平县", "连城县", "漳平市"};

            //CountyData countyData1 = new CountyData();
            //countyData1.countyname = "新罗区";
            //countyData1.code = new[] { "350802" };

            //CountyData countyData2 = new CountyData();
            //countyData2.countyname = "长汀县";
            //countyData2.code = new[] { "350821" };

            //cityData1.countys = new[] { countyData1, countyData2 };


            //provinceData.citys[0] = cityData1;

            //string str = JsonHelper.Serialize(data, false);

            #endregion

            //var temp =  DataManager.Instance;
            //var zone3 = DataManager.GetZone3("111", "22", "33");
            //var xxy = DataManager.GetXXYGeneralDataAboutEarlyWarning("111","2");
            //var warning = DataManager.GetWarningByIdentifier("111");


            var createData = new CreateData();


            var vThread = new Thread(() =>
            {
                var temp = createData.GetPoiData();
                createData.CreateDataOnlyOnce();

                //var temp = po;
            });
            vThread.Start();
        }

        #region 接口测试

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var a = APIHelper.Get(@"http://localhost/CMAPlatform.TestService/WebServiceTest.asmx/HelloWorld");

            content.Content = a;
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("a", "测试接口！");
            dictionary.Add("b", "测试完毕！");

            var a = APIHelper.Get(@"http://localhost/CMAPlatform.TestService/WebServiceTest.asmx/TestString", dictionary);
            content2.Content = a;
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            //  if (!string.IsNullOrEmpty(PickerStaTime.SelectedValue.ToString()))
            //  {

            //  }
            //var a= Convert.ToDateTime(PickerStaTime.SelectedValue).ToString("yyyy-MM-dd HH:mm:ss");
            //CreateData createData = new CreateData();
            //Thread vThread = new Thread(() =>
            //{
            //    createData.CreateDataOnlyOnce();
            //});
            //vThread.Start();


            CreateDataResult();
            //DataManager.percent("上海市");
            //CreateData createData = new CreateData();
            //Dictionary<bool, List<ProventionMessageModel>> DataSource=new Dictionary<bool, List<ProventionMessageModel>>();
            //DataSource = createData.percent("黑龙江");
        }

        private readonly DispatcherTimer m_AIStimer = new DispatcherTimer();
        private ResultModel resultModel = new ResultModel();

        /// <summary>
        ///     计时入库
        /// </summary>
        private void CreateDataResult()
        {
            m_AIStimer.Interval = TimeSpan.FromSeconds(300);
            m_AIStimer.Tick += m_AIStimer_Tick;
            var createData = new CreateData();

            var vThread = new Thread(() => { resultModel = createData.CreateDataResult(); });
            vThread.Start();
            m_AIStimer.Start();
        }

        private void m_AIStimer_Tick(object sender, EventArgs e)
        {
            var vThread = new Thread(() =>
            {
                var createData = new CreateData();
                resultModel = createData.CreateDataResult();
            });
            vThread.Start();
        }

        public void JsonData()
        {
        }

        #endregion
    }
}