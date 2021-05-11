using System.Windows.Media;
using Digihail.AVE.Launcher.Controls;

namespace CMAPlatform.Chart.Window
{
    /// <summary>
    ///     TyphoonDetailWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TyphoonDetailWindow : PopWindow
    {
        public TyphoonDetailWindow(string title, string time, string description, string severity)
        {
            InitializeComponent();

            txtTitle.Text = title;

            switch (severity.ToLower())
            {
                case "red":
                    txtTitle.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#DA474B"));
                    break;
                case "orange":
                    txtTitle.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#EB7804"));
                    break;
                case "yellow":
                    txtTitle.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#F3CC27"));
                    break;
                case "blue":
                    txtTitle.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#00FFFF"));
                    break;
                default:
                    txtTitle.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFFFFF"));
                    break;
            }

            txtTime.Text = time;
            txtDescription.Text = description;
        }
    }
}