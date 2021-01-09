using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CMAPlatform.Chart.Controller
{
    /// <summary>
    ///     带单位的数字输入控件
    /// </summary>
    [TemplatePart(Name = "UniteTextBox", Type = typeof (TextBox))]
    [TemplatePart(Name = "NumberValueTextBox", Type = typeof (TextBox))]
    public class UnitedNumberTextBox : TextBox
    {
        /// <summary>
        ///     最大值Property
        /// </summary>
        public static readonly DependencyProperty MaxValueContentProperty = DependencyProperty.Register(
            "MaxValueContent", typeof (double), typeof (UnitedNumberTextBox),
            new FrameworkPropertyMetadata(OnValueChanged));

        /// <summary>
        ///     最小值Property
        /// </summary>
        public static readonly DependencyProperty MinValueContentProperty = DependencyProperty.Register(
            "MinValueContent", typeof (double), typeof (UnitedNumberTextBox),
            new FrameworkPropertyMetadata(OnValueChanged));

        /// <summary>
        ///     小数点位数Property
        /// </summary>
        public static readonly DependencyProperty PerValueContentProperty = DependencyProperty.Register(
            "PerValueContent", typeof (double), typeof (UnitedNumberTextBox),
            new FrameworkPropertyMetadata(5.0, OnValueChanged));

        /// <summary>
        ///     单位的属性触发器
        /// </summary>
        public static readonly DependencyProperty UnitedTextProperty =
            DependencyProperty.Register("UnitedText", typeof (string), typeof (UnitedNumberTextBox),
                new PropertyMetadata("公里/小时"));

        /// <summary>
        ///     静态构造函数
        /// </summary>
        static UnitedNumberTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (UnitedNumberTextBox),
                new FrameworkPropertyMetadata(typeof (UnitedNumberTextBox)));
        }

        /// <summary>
        ///     g构造函数
        /// </summary>
        public UnitedNumberTextBox()
        {
            // this.TextChanged += NumberTxt_OnTextChanged;
            TextChanged += NumberTextBox_TextChanged;
            KeyDown += NumberTextBox_KeyDown;
        }

        /// <summary>
        ///     最大值
        /// </summary>
        public double MaxValueContent
        {
            get { return (double) GetValue(MaxValueContentProperty); }
            set { SetValue(MaxValueContentProperty, value); }
        }

        /// <summary>
        ///     最小值
        /// </summary>
        public double MinValueContent
        {
            get { return (double) GetValue(MinValueContentProperty); }
            set { SetValue(MinValueContentProperty, value); }
        }

        /// <summary>
        ///     小数点位数
        /// </summary>
        public double PerValueContent
        {
            get { return (double) GetValue(PerValueContentProperty); }
            set { SetValue(PerValueContentProperty, value); }
        }

        /// <summary>
        ///     单位
        /// </summary>
        public string UnitedText
        {
            get { return (string) GetValue(UnitedTextProperty); }
            set { SetValue(UnitedTextProperty, value); }
        }

        /// <summary>
        ///     最大最小值变化触发事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as NumberTextBox;

            obj.Text = string.Empty;
        }

        /// <summary>
        ///     键盘输入事件，验证输入字符合法性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;

            //屏蔽非法按键  
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal || e.Key.ToString() == "Tab" ||
                e.Key == Key.Subtract)
            {
                if ((txt.Text.Contains(".") && e.Key == Key.Decimal) ||
                    (txt.Text.Contains("-") && e.Key == Key.Subtract))
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod || e.Key == Key.OemMinus) &&
                     e.KeyboardDevice.Modifiers != ModifierKeys.Shift) //|| e.Key == Key.ImeProcessed
            {
                if ((txt.Text.Contains(".") && e.Key == Key.OemPeriod) ||
                    (txt.Text.Contains("-") && e.Key == Key.OemMinus))
                    //|| (txt.Text.Contains("-") && e.Key == Key.ImeProcessed))
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        ///     检查限制条件（最大值和小数点位数）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool CheckDouble(string text)
        {
            double value;
            var isDouble = double.TryParse(text, out value);
            if (text == "-" ||
                (isDouble &&
                 ((value >= MinValueContent && value <= MaxValueContent) ||
                  (MinValueContent == 0 && MaxValueContent == 0))))
            {
                var strs = text.Split('.');
                if (strs.Length > 1)
                {
                    if (strs[1].Length <= PerValueContent)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Text变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //屏蔽中文输入和非法字符粘贴输入  
            var textBox = sender as TextBox;
            var change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);

            var offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (!(double.TryParse(textBox.Text, out num) && CheckDouble(textBox.Text)))
                {
                    if (!textBox.Text.Equals("-"))
                    {
                        textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                        textBox.Select(offset, 0);
                    }
                }
            }
        }

        #region 属性

        #endregion
    }
}