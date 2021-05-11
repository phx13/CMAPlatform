using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CMAPlatform.Chart.Models;

namespace CMAPlatform.Chart.View
{
    public class DrawingBubbleCanvas : Canvas
    {
        public static FrameworkPropertyMetadata RingWidthdatasMetadata = new FrameworkPropertyMetadata(0.0)
        {
            AffectsRender = true
        };

        // Using a DependencyProperty as the backing store for RingWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RingWidthProperty =
            DependencyProperty.Register("RingWidth", typeof (double), typeof (DrawingBubbleCanvas),
                RingWidthdatasMetadata);

        public static FrameworkPropertyMetadata textcolordatasMetadata = new FrameworkPropertyMetadata(Brushes.Black)
        {
            AffectsRender = true
        };

        // Using a DependencyProperty as the backing store for TextColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof (Brush), typeof (DrawingBubbleCanvas),
                textcolordatasMetadata);

        public static FrameworkPropertyMetadata textsizedatasMetadata = new FrameworkPropertyMetadata(20.0)
        {
            AffectsRender = true
        };

        // Using a DependencyProperty as the backing store for TextSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextSizeProperty =
            DependencyProperty.Register("TextSize", typeof (double), typeof (DrawingBubbleCanvas), textsizedatasMetadata);

        public static FrameworkPropertyMetadata textdatasMetadata = new FrameworkPropertyMetadata(1.0)
        {
            AffectsRender = true
        };

        // Using a DependencyProperty as the backing store for TextScale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextScaleProperty =
            DependencyProperty.Register("TextScale", typeof (double), typeof (DrawingBubbleCanvas), textdatasMetadata);

        public static FrameworkPropertyMetadata datasMetadata =
            new FrameworkPropertyMetadata(new ObservableCollection<BubbleInfo>()) {AffectsRender = true};

        // Using a DependencyProperty as the backing store for Datas.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatasProperty =
            DependencyProperty.Register("Datas", typeof (ObservableCollection<BubbleInfo>), typeof (DrawingBubbleCanvas),
                datasMetadata);

        public double RingWidth
        {
            get { return (double) GetValue(RingWidthProperty); }
            set { SetValue(RingWidthProperty, value); }
        }


        public Brush TextColor
        {
            get { return (Brush) GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }


        public double TextSize
        {
            get { return (double) GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }


        public double TextScale
        {
            get { return (double) GetValue(TextScaleProperty); }
            set { SetValue(TextScaleProperty, value); }
        }

        public ObservableCollection<BubbleInfo> Datas
        {
            get { return (ObservableCollection<BubbleInfo>) GetValue(DatasProperty); }
            set { SetValue(DatasProperty, value); }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var color =
                "#2FCBFF,#A073FF,#FFBF00,#01BA8D,#9B9FEC,#228BFF,#FFDFBE31,#FFFF8209,#FFC649A5,#FF5F52CD".Split(',');
            var drawingPen = new Pen(Brushes.Transparent, 0);
            drawingPen.Freeze();

            var textSize = TextSize/TextScale;

            var tf = new Typeface("微软雅黑");

            double ringWidth = 0;

            for (var i = 0; i < Datas.Count; i++)
            {
                if (RingWidth > 0)
                {
                    ringWidth = RingWidth/TextScale;

                    ringWidth = Math.Min(ringWidth, Datas.Min(p => p.Radius));
                }
                else
                {
                    ringWidth = Datas[i].Radius;
                }


                dc.DrawGeometry(new SolidColorBrush((Color) ColorConverter.ConvertFromString(color[i%10])), drawingPen,
                    GetGeometry(0, 359.9999, Datas[i], ringWidth));

                var ft = new FormattedText(Datas[i].Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface("微软雅黑"), textSize, TextColor);
                ft.MaxTextWidth = Datas[i].Radius*2 - 10;
                ft.MaxTextHeight = Datas[i].Radius*2 - 10;
                ft.Trimming = TextTrimming.WordEllipsis;
                var point = new Point(Datas[i].X - ft.WidthIncludingTrailingWhitespace/2, Datas[i].Y - ft.Height/2);
                dc.DrawText(ft, point);
            }
        }

        private Geometry GetGeometry(double startAngle, double endAngle, BubbleInfo bubbleInfo, double distance)
        {
            var pg = new PathGeometry();
            pg.Figures.Add(DrawSingle(startAngle, endAngle, bubbleInfo, distance));
            return pg;
        }

        private PathFigure DrawSingle(double startAngle, double endAngle, BubbleInfo bubbleInfo, double distance)
        {
            var pf = new PathFigure
            {
                IsClosed = false,
                StartPoint =
                    new Point(bubbleInfo.X + bubbleInfo.Radius*Math.Sin(startAngle/180*Math.PI),
                        bubbleInfo.Y - bubbleInfo.Radius*Math.Cos(startAngle/180*Math.PI))
            };

            var src1 = new ArcSegment
            {
                Point =
                    new Point(bubbleInfo.X + bubbleInfo.Radius*Math.Sin(endAngle/180*Math.PI),
                        bubbleInfo.Y - bubbleInfo.Radius*Math.Cos(endAngle/180*Math.PI)),
                Size = new Size
                {
                    Width = bubbleInfo.Radius,
                    Height = bubbleInfo.Radius
                },
                SweepDirection = SweepDirection.Clockwise
            };

            if (endAngle - startAngle > 180)
            {
                src1.IsLargeArc = true;
            }

            var interR = bubbleInfo.Radius - distance;

            var line = new LineSegment
            {
                Point =
                    new Point(bubbleInfo.X + interR*Math.Sin(endAngle/180*Math.PI),
                        bubbleInfo.Y - interR*Math.Cos(endAngle/180*Math.PI))
            };

            var src2 = new ArcSegment
            {
                Point =
                    new Point(bubbleInfo.X + interR*Math.Sin(startAngle/180*Math.PI),
                        bubbleInfo.Y - interR*Math.Cos(startAngle/180*Math.PI)),
                Size = new Size
                {
                    Width = interR,
                    Height = interR
                },
                SweepDirection = SweepDirection.Counterclockwise
            };

            if (endAngle - startAngle > 180)
            {
                src2.IsLargeArc = true;
            }

            pf.Segments.Add(src1);
            pf.Segments.Add(line);
            pf.Segments.Add(src2);

            pf.IsFilled = true;
            return pf;
        }
    }
}