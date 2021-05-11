using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CMAPlatform.Chart.DVM;
using CMAPlatform.Chart.Models;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using Point = System.Drawing.Point;

namespace CMAPlatform.Chart.Controller
{
    public class BubbleController : ChartControllerBase
    {
        private readonly DispatcherTimer dt = new DispatcherTimer();

        private readonly ObservableCollection<BubbleInfo> tempBubbleInfos = new ObservableCollection<BubbleInfo>();

        private readonly ObservableCollection<BubbleInfo> tempDbInfos = new ObservableCollection<BubbleInfo>();

        private ObservableCollection<BubbleInfo> m_BubbleInfos = new ObservableCollection<BubbleInfo>();

        private double m_CanvasHeight;

        private Thickness m_CanvasMargin;

        private double m_CanvasScale;

        private double m_CanvasWidth;


        private BubbleViewModel m_DVM = new BubbleViewModel();


        public BubbleController(BubbleViewModel itemsViewModel, IDataProxy dataProsy, IPlayable player)
            : base(itemsViewModel, dataProsy, player)
        {
            DVM = itemsViewModel;
            m_DataProxy = dataProsy;
            m_Player = player;

            DVM.PropertyChanged += DVM_PropertyChanged;

            dt.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dt.Tick += dt_Tick;
            dt.Stop();
        }

        private IPlayable m_Player { get; set; }

        public ObservableCollection<BubbleInfo> BubbleInfos
        {
            get { return m_BubbleInfos; }
            set
            {
                m_BubbleInfos = value;
                OnPropertyChanged("BubbleInfos");
            }
        }

        public double CanvasWidth
        {
            get { return m_CanvasWidth; }
            set
            {
                m_CanvasWidth = value;
                OnPropertyChanged("CanvasWidth");
            }
        }

        public double CanvasHeight
        {
            get { return m_CanvasHeight; }
            set
            {
                m_CanvasHeight = value;
                OnPropertyChanged("CanvasHeight");
            }
        }

        public Thickness CanvasMargin
        {
            get { return m_CanvasMargin; }
            set
            {
                m_CanvasMargin = value;
                OnPropertyChanged("CanvasMargin");
            }
        }

        public double CanvasScale
        {
            get { return m_CanvasScale; }
            set
            {
                m_CanvasScale = value;
                OnPropertyChanged("CanvasScale");
            }
        }

        public BubbleViewModel DVM
        {
            get { return m_DVM; }
            set
            {
                m_DVM = value;
                OnPropertyChanged("DVM");
            }
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            if (BubbleInfos.Count < tempBubbleInfos.Count)
            {
                BubbleInfos =
                    new ObservableCollection<BubbleInfo>(
                        tempBubbleInfos.Where(item => tempBubbleInfos.IndexOf(item) <= BubbleInfos.Count()).ToList());
            }
            else
            {
                dt.Stop();
            }
        }

        private void DVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ChartWidth") || e.PropertyName.Equals("ChartHeight") ||
                e.PropertyName.Equals("BiggerIndex"))
            {
                Task.Run(() => DrawChart(tempDbInfos));
            }
        }


        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        public override void ReceiveData(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows.Count <= 0) return;
            if (adt != null && adt.Rows.Count > 0)
            {
                tempDbInfos.Clear();

                foreach (var row in adt.Rows)
                {
                    tempDbInfos.Add(new BubbleInfo
                    {
                        Name = row[DVM.ItemName.AsName].ToString(),
                        Value = double.Parse(row[DVM.ItemNumber.AsName].ToString())
                    });
                }

                Task.Run(() => DrawChart(tempDbInfos));
            }
        }

        /// <summary>
        ///     设定气泡图表所需信息及绘制气泡
        /// </summary>
        /// <param name="BubbleListInfo"></param>
        private void DrawChart(ObservableCollection<BubbleInfo> BubbleListInfo)
        {
            if (BubbleListInfo == null || BubbleListInfo.Count == 0)
            {
                return;
            }

            dt.Stop();

            BubbleInfos.Clear();

            // 画布中心位置
            var centorPoint = new Point(Convert.ToInt32(DVM.ChartWidth/2), Convert.ToInt32(DVM.ChartHeight/2));

            // 绘画区域半径
            double radius = centorPoint.X > centorPoint.Y ? centorPoint.Y : centorPoint.X;

            // 绘画区域圆面积
            var area = Math.PI*radius*radius;

            // 单个气泡所占面积
            var singleArea = area/BubbleListInfo.Count();

            // 单个气泡半径
            var singleRadius = Math.Sqrt(singleArea/Math.PI);

            // 绘画时采用的最小气泡半径
            var minRadius = Common.MinRadius;

            // 绘画时采用的最大气泡半径
            var maxRadius = singleRadius;

            //maxRadius = maxRadius > minRadius ? maxRadius : minRadius;
            //if (maxRadius / minRadius < Common.BiggerIndex)
            //{
            //    maxRadius = minRadius * Common.BiggerIndex;
            //}

            maxRadius = minRadius*DVM.BiggerIndex;

            // 气泡间间隔
            var interval = Common.BubbleInterval;

            // 数据中的最大最小值
            var sortList = BubbleListInfo.OrderByDescending(p => p.Value).ToList();
            var maxValue = sortList.FirstOrDefault().Value;
            var minValue = sortList.LastOrDefault().Value;

            tempBubbleInfos.Clear();

            foreach (var bubble in BubbleListInfo)
            {
                // 设定单个气泡信息
                SetBubbleInfo(centorPoint,
                    maxRadius,
                    minRadius,
                    interval,
                    maxValue,
                    minValue,
                    bubble,
                    tempBubbleInfos);
            }

            // 重新设定坐标
            ResetCoordinate(centorPoint, tempBubbleInfos);

            dt.Start();
        }

        /// <summary>
        ///     设定单个气泡信息
        /// </summary>
        /// <param name="centorPoint"></param>
        /// <param name="maxRadius"></param>
        /// <param name="minRadius"></param>
        /// <param name="interval"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <param name="currentBubble"></param>
        /// <param name="settedBubbleList"></param>
        private void SetBubbleInfo(Point centorPoint,
            double maxRadius,
            double minRadius,
            double interval,
            double maxValue,
            double minValue,
            BubbleInfo currentBubble,
            ObservableCollection<BubbleInfo> settedBubbleList)
        {
            var oneFlag = false;
            var twoFlag = false;
            var added = false;

            // 设定气泡半径
            if (maxValue == minValue)
            {
                currentBubble.Radius = maxRadius;
            }
            else
            {
                currentBubble.Radius = (currentBubble.Value - minValue)/(maxValue - minValue)*(maxRadius - minRadius) +
                                       minRadius;
            }

            // 设定第一个气泡信息（第一个气泡在画布中心点位置）
            if (settedBubbleList.Count == 0)
            {
                // 设定气泡中心点坐标
                currentBubble.X = centorPoint.X;
                currentBubble.Y = centorPoint.Y;
                // 设定气泡相对画布中心点角度
                currentBubble.Angle = 0;

                settedBubbleList.Add(currentBubble);
            }
            else if (settedBubbleList.Count == 1)
            {
                // 设定第三个气泡信息（位置随机设定）
                //int r = random.Next(0, 359);
                var r = 45;
                // 设定气泡中心点坐标
                currentBubble.X = centorPoint.X +
                                  Math.Sin(Math.PI/180*r)*(settedBubbleList[0].Radius + currentBubble.Radius + interval);
                currentBubble.Y = centorPoint.Y +
                                  Math.Cos(Math.PI/180*r)*(settedBubbleList[0].Radius + currentBubble.Radius + interval);
                // 设定气泡相对画布中心点角度
                currentBubble.Angle =
                    Common.CalcAngle(new Point(Convert.ToInt32(currentBubble.X), Convert.ToInt32(currentBubble.Y)),
                        centorPoint);
                // 设定相切气泡
                currentBubble.Touchs.Add(settedBubbleList[0]);

                settedBubbleList.Add(currentBubble);
            }
            else
            {
                //int r = random.Next(1, 100);
                var r = 70;

                var lastId = settedBubbleList.LastOrDefault().Id;

                // 其它气泡
                for (var round = 0; round < settedBubbleList.Count; round++)
                {
                    var bubbleList = settedBubbleList.Where(p => p.Id <= lastId - round).ToList();

                    // 在气泡右侧指定范围角度内查找最外层气泡
                    var findBubbleList = FindBubble(bubbleList);

                    added = false;

                    for (var i = 0; i < findBubbleList.Count; i++)
                    {
                        // 求两个圆的交点处理逻辑
                        // 以第一个圆的中心为坐标中心（坐标转换利于计算，圆心为（0， 0）圆方程：x^2 + y^2 = r1^2）
                        // 第一个圆的半径（第一个相切气泡的半径 + 新气泡的半径 + 气泡间隔）
                        var r1 = bubbleList.LastOrDefault().Radius + currentBubble.Radius + interval;

                        // 第二个圆的中心相对第一个圆的坐标(圆方程：(x - x2)^2 + (y - y2)^2 = r2^2)
                        var x2 = findBubbleList[i].X - bubbleList.LastOrDefault().X;
                        var y2 = findBubbleList[i].Y - bubbleList.LastOrDefault().Y;
                        // 第二个圆的半径（第二个相切气泡的半径 + 新气泡的半径 + 气泡间隔）
                        var r2 = findBubbleList[i].Radius + currentBubble.Radius + interval;

                        // 两式合并
                        // 4*(x2^2+y2^2)*y^2 - 4*y2*(x2^2 + y2^2 + r1^2 - r2^2)*y + (x2^2 + y2^2 + r1^2 - r2^2)*(x2^2 + y2^2 +=  r1^2 - r2^2) - 4*x2^2*r1^2 = 0
                        // a*y^2 + b*y + c = 0
                        var a = 4*(x2*x2 + y2*y2);
                        var b = -4*y2*(x2*x2 + y2*y2 + r1*r1 - r2*r2);
                        var c = (x2*x2 + y2*y2 + r1*r1 - r2*r2)*(x2*x2 + y2*y2 + r1*r1 - r2*r2) - 4*x2*x2*r1*r1;

                        // 有解（b2-4ac≥0）
                        if (b*b - 4*a*c >= 0)
                        {
                            if (x2 == 0)
                            {
                                x2 = 0.00000000000001;
                            }

                            // y = [-b±(b^2-4ac)^(1/2)]/(2a) 
                            // x = (x2^2 + y2^2 + r1^2 - r2^2 - 2*y2*y)/(2*x2)
                            var yOne = (-b + Math.Sqrt(b*b - 4*a*c))/(2*a);
                            var xOne = (x2*x2 + y2*y2 + r1*r1 - r2*r2 - 2*y2*yOne)/(2*x2);
                            var yTwo = (-b - Math.Sqrt(b*b - 4*a*c))/(2*a);
                            var xTwo = (x2*x2 + y2*y2 + r1*r1 - r2*r2 - 2*y2*yTwo)/(2*x2);

                            //相对坐标转绝对坐标
                            var pointOne = new Point(Convert.ToInt32(xOne + bubbleList.LastOrDefault().X),
                                Convert.ToInt32(yOne + bubbleList.LastOrDefault().Y));
                            var pointTwo = new Point(Convert.ToInt32(xTwo + bubbleList.LastOrDefault().X),
                                Convert.ToInt32(yTwo + bubbleList.LastOrDefault().Y));

                            oneFlag = false;
                            twoFlag = false;

                            // 判断是否有重叠(与以存在的所有气泡进行判断)
                            for (var j = settedBubbleList.Count - 1; j >= 0; j--)
                            {
                                if (!oneFlag)
                                {
                                    // 两圆心的距离
                                    var distanceOne =
                                        Math.Sqrt((pointOne.X - settedBubbleList[j].X)*
                                                  (pointOne.X - settedBubbleList[j].X) +
                                                  (pointOne.Y - settedBubbleList[j].Y)*
                                                  (pointOne.Y - settedBubbleList[j].Y));
                                    // 重叠
                                    if (settedBubbleList[j].Radius + currentBubble.Radius + interval - distanceOne > 0.1)
                                    {
                                        oneFlag = true;
                                    }
                                }

                                if (!twoFlag)
                                {
                                    // 两圆心的距离
                                    var distanceTwo =
                                        Math.Sqrt((pointTwo.X - settedBubbleList[j].X)*
                                                  (pointTwo.X - settedBubbleList[j].X) +
                                                  (pointTwo.Y - settedBubbleList[j].Y)*
                                                  (pointTwo.Y - settedBubbleList[j].Y));
                                    // 重叠
                                    if (settedBubbleList[j].Radius + currentBubble.Radius + interval - distanceTwo > 0.1)
                                    {
                                        twoFlag = true;
                                    }
                                }
                            }

                            // 全没有重叠
                            if (!oneFlag && !twoFlag)
                            {
                                // 新气泡相对相切气泡的角度（位置1）
                                var angleOne = Common.CalcAngle(pointOne,
                                    new Point(Convert.ToInt32(findBubbleList[i].X), Convert.ToInt32(findBubbleList[i].Y)));

                                // 新气泡相对相切气泡的角度（位置2）
                                var angleTwo = Common.CalcAngle(pointTwo,
                                    new Point(Convert.ToInt32(findBubbleList[i].X), Convert.ToInt32(findBubbleList[i].Y)));

                                // 两相切气泡指针方向角度
                                var angle =
                                    Common.CalcAngle(
                                        new Point(Convert.ToInt32(bubbleList.LastOrDefault().X),
                                            Convert.ToInt32(bubbleList.LastOrDefault().Y)),
                                        new Point(Convert.ToInt32(findBubbleList[i].X),
                                            Convert.ToInt32(findBubbleList[i].Y)));

                                // 取顺时针/逆时针方向180度的圆点坐标
                                if (Common.RightAngleCheck(angle, Math.PI, angleOne, r > 50))
                                {
                                    // 设定坐标
                                    currentBubble.X = pointOne.X;
                                    currentBubble.Y = pointOne.Y;
                                }
                                else
                                {
                                    // 设定坐标
                                    currentBubble.X = pointTwo.X;
                                    currentBubble.Y = pointTwo.Y;
                                }

                                // 设定气泡相对画布中心点角度
                                currentBubble.Angle =
                                    Common.CalcAngle(
                                        new Point(Convert.ToInt32(currentBubble.X), Convert.ToInt32(currentBubble.Y)),
                                        centorPoint);
                                // 设定相切气泡
                                currentBubble.Touchs.Add(bubbleList.LastOrDefault());
                                currentBubble.Touchs.Add(findBubbleList[i]);

                                settedBubbleList.Add(currentBubble);
                                added = true;
                                break;
                            }
                            if (!oneFlag)
                            {
                                // 设定坐标
                                currentBubble.X = pointOne.X;
                                currentBubble.Y = pointOne.Y;
                                // 设定气泡相对画布中心点角度
                                currentBubble.Angle =
                                    Common.CalcAngle(
                                        new Point(Convert.ToInt32(currentBubble.X), Convert.ToInt32(currentBubble.Y)),
                                        centorPoint);
                                // 设定相切气泡
                                currentBubble.Touchs.Add(bubbleList.LastOrDefault());
                                currentBubble.Touchs.Add(findBubbleList[i]);

                                settedBubbleList.Add(currentBubble);
                                added = true;
                                break;
                            }
                            if (!twoFlag)
                            {
                                // 设定坐标
                                currentBubble.X = pointTwo.X;
                                currentBubble.Y = pointTwo.Y;
                                // 设定气泡相对画布中心点角度
                                currentBubble.Angle =
                                    Common.CalcAngle(
                                        new Point(Convert.ToInt32(currentBubble.X), Convert.ToInt32(currentBubble.Y)),
                                        centorPoint);
                                // 设定相切气泡
                                currentBubble.Touchs.Add(bubbleList.LastOrDefault());
                                currentBubble.Touchs.Add(findBubbleList[i]);

                                settedBubbleList.Add(currentBubble);
                                added = true;
                                break;
                            }
                        }
                    }

                    // 添加成功
                    if (added)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     查找第二个相切气泡的范围
        /// </summary>
        /// <param name="bubbleList"></param>
        /// <returns></returns>
        private List<BubbleInfo> FindBubble(List<BubbleInfo> bubbleList)
        {
            var findBubbleList = new List<BubbleInfo>();

            if (bubbleList.Count >= 2)
            {
                for (var i = bubbleList.Count - 2; i >= bubbleList.LastOrDefault().Touchs.Last().Id; i--)
                {
                    findBubbleList.Insert(0, bubbleList[i]);
                }
            }
            return findBubbleList;
        }

        /// <summary>
        ///     重新设定坐标
        /// </summary>
        private void ResetCoordinate(Point centorPoint, ObservableCollection<BubbleInfo> settedBubbleList)
        {
            var top = DVM.ChartHeight;
            var left = DVM.ChartWidth;
            var bottom = DVM.ChartHeight;
            var right = DVM.ChartWidth;

            // 计算气泡区域边界
            foreach (var item in settedBubbleList)
            {
                if (item.X - item.Radius < left)
                {
                    left = item.X - item.Radius;
                }

                if (centorPoint.X*2 - (item.X + item.Radius) < right)
                {
                    right = centorPoint.X*2 - (item.X + item.Radius);
                }

                if (item.Y - item.Radius < top)
                {
                    top = item.Y - item.Radius;
                }

                if (centorPoint.Y*2 - (item.Y + item.Radius) < bottom)
                {
                    bottom = centorPoint.Y*2 - (item.Y + item.Radius);
                }
            }

            // 绘图中心点坐标
            var drawCentorPoint = new Point(Convert.ToInt32((centorPoint.X*2 + left - right)/2),
                Convert.ToInt32((centorPoint.Y*2 + top - bottom)/2));

            // 中心偏移量
            var offsetPoint = new Point(drawCentorPoint.X - centorPoint.X, drawCentorPoint.Y - centorPoint.Y);

            // 变形画布设定
            CanvasWidth = centorPoint.X*2 - left - right;
            CanvasHeight = centorPoint.Y*2 - top - bottom;

            // 绘图中心点坐标对齐画布中心坐标
            foreach (var item in settedBubbleList)
            {
                item.X = item.X - offsetPoint.X - (DVM.ChartWidth - CanvasWidth)/2;
                item.Y = item.Y - offsetPoint.Y - (DVM.ChartHeight - CanvasHeight)/2;
            }

            // 设定缩放比例
            double scalcX = 1;

            // 左右压缩,压缩比例为
            scalcX = DVM.ChartWidth/CanvasWidth;

            double scalcY = 1;

            // 上下压缩，压缩比例为
            scalcY = DVM.ChartHeight/CanvasHeight;

            if (scalcX != 1 || scalcY != 1)
            {
                CanvasScale = Math.Min(scalcX, scalcY);
            }

            CanvasMargin = new Thickness(centorPoint.X - CanvasWidth*CanvasScale/2,
                centorPoint.Y - CanvasHeight*CanvasScale/2,
                centorPoint.X - CanvasWidth*CanvasScale/2,
                centorPoint.Y - CanvasHeight*CanvasScale/2);
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }
    }
}