using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using Digihail.CCP.Utilities;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using ContentControl = System.Windows.Controls.ContentControl;

// ReSharper disable once CheckNamespace
namespace CMAPlatform.TimeLine
{
    /// <summary>
    ///     DocumentsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DocumentsWindow
    {
        #region 成员变量

        // 文件下载URL
        private readonly string m_DownLoadUrl = "";

        #endregion

        #region 构造函数

        /// <summary>
        ///     无参构造
        /// </summary>
        public DocumentsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     有参构造
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="name">窗体名称</param>
        public DocumentsWindow(string path, string name)
        {
            InitializeComponent();
            m_DownLoadUrl = path;
            Title = name.Substring(0, 2) + "详情";
            Loaded += DocumentsWindow_Loaded;
        }

        #endregion

        #region 私有方法

        // 窗体加载完成事件
        private void DocumentsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var analysisControls = docViewer.GetChildsByType<ContentControl>();
            analysisControls[0].Visibility = Visibility.Collapsed;
            analysisControls[2].Visibility = Visibility.Collapsed;
            DeleteDownLoad(); //删除之前下载的文件

            string savePath;

            if (m_DownLoadUrl.ToLower().StartsWith("http"))
            {
                savePath = DownLoadFile(m_DownLoadUrl); //下载新文件到指定位置，返回路径
            }
            else
            {
                savePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\文档\\" + m_DownLoadUrl;
            }
            //打开文件
            var name = m_DownLoadUrl.Split('/').LastOrDefault();
            var type = name.Split('.').LastOrDefault();
            if (type.ToLower() == "docx" || type.ToLower() == "doc" || type.ToLower() == "pdf")
            {
                docViewer.Document = ConvertWordToXPS(savePath).GetFixedDocumentSequence();
                docViewer.FitToWidth();
            }
            else
            {
                docViewer.Visibility = Visibility.Collapsed;
                image.Source = new BitmapImage(new Uri(savePath));
                image.Visibility = Visibility.Visible;
            }
        }

        // 删除下载文件
        private void DeleteDownLoad()
        {
            try
            {
                var savePath = Environment.CurrentDirectory + "DownLoad";
                var dir = new DirectoryInfo(savePath);
                var fileInfos = dir.GetFileSystemInfos();

                foreach (var fileSystemInfo in fileInfos)
                {
                    if (fileSystemInfo is DirectoryInfo)
                    {
                        var subDir = new DirectoryInfo(fileSystemInfo.FullName);
                        subDir.Delete(true);
                    }
                    else
                    {
                        File.Delete(fileSystemInfo.FullName);
                    }
                }
            }
            catch (Exception ee)
            {
                CcpLoggerManager.WriteViewerErrow(ee);
            }
        }

        // 下载文件到指定位置
        private string DownLoadFile(string path)
        {
            var names = path.Split('/');
            var name = names.LastOrDefault();
            var request = WebRequest.Create(path) as HttpWebRequest;
            var response = request.GetResponse() as HttpWebResponse;
            var responseStream = response.GetResponseStream();
            var savePath = Environment.CurrentDirectory + "\\DownLoad\\" + name;
            Stream stream = new FileStream(savePath, FileMode.Create);
            var bArr = new byte[2048];
            var size = responseStream.Read(bArr, 0, bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            return savePath;
        }

        //读取文件内容显示在窗体中
        private XpsDocument ConvertWordToXPS(string wordDocName)
        {
            var fi = new FileInfo(wordDocName);
            XpsDocument result = null;

            docViewer.Visibility = Visibility.Visible;
            image.Visibility = Visibility.Collapsed;
            var xpsDocName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), fi.Name);
            xpsDocName = xpsDocName.Replace(".docx", ".xps").Replace(".doc", ".xps");
            var wordApplication = new Application();
            try
            {
                if (!File.Exists(xpsDocName))
                {
                    wordApplication.Documents.Add(wordDocName);
                    var doc = wordApplication.ActiveDocument;
                    doc.ExportAsFixedFormat(xpsDocName, WdExportFormat.wdExportFormatXPS, false,
                        WdExportOptimizeFor.wdExportOptimizeForPrint, WdExportRange.wdExportAllDocument, 0, 0,
                        WdExportItem.wdExportDocumentContent, true, true,
                        WdExportCreateBookmarks.wdExportCreateHeadingBookmarks, true, true, false, Type.Missing);
                    result = new XpsDocument(xpsDocName, FileAccess.Read);
                }
                if (File.Exists(xpsDocName))
                {
                    result = new XpsDocument(xpsDocName, FileAccess.Read);
                }
            }
            catch (Exception)
            {
                wordApplication.Quit(WdSaveOptions.wdDoNotSaveChanges);
            }
            wordApplication.Quit(WdSaveOptions.wdDoNotSaveChanges);


            return result;
        }

        #endregion
    }
}