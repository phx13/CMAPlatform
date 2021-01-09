using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace DrawImageTest
{
    public class DrawImage
    {
        public void Draw()
        {
            FileReader fileReader = new FileReader();
            var typhoonList = fileReader.TyphoonData();

            var selectTyphoon = typhoonList.OrderByDescending(x => x.bj_datetime).FirstOrDefault();

            if (selectTyphoon == null)
            {
                Console.WriteLine("台风数据为空");
                return;
            }

            var typhoon = new Typhoon()
            {
                CenterLon = double.Parse(selectTyphoon.lon),
                CenterLat = double.Parse(selectTyphoon.lat),
                YellowRadius1 = double.Parse(selectTyphoon.en7radii.Equals("null") ? "1" : selectTyphoon.en7radii),
                YellowRadius2 = double.Parse(selectTyphoon.es7radii.Equals("null") ? "1" : selectTyphoon.es7radii),
                YellowRadius3 = double.Parse(selectTyphoon.ws7radii.Equals("null") ? "1" : selectTyphoon.ws7radii),
                YellowRadius4 = double.Parse(selectTyphoon.wn7radii.Equals("null") ? "1" : selectTyphoon.wn7radii),
                RedRadius1 = double.Parse(selectTyphoon.en10radii.Equals("null") ? "1" : selectTyphoon.en10radii),
                RedRadius2 = double.Parse(selectTyphoon.es10radii.Equals("null") ? "1" : selectTyphoon.es10radii),
                RedRadius3 = double.Parse(selectTyphoon.ws10radii.Equals("null") ? "1" : selectTyphoon.ws10radii),
                RedRadius4 = double.Parse(selectTyphoon.wn10radii.Equals("null") ? "1" : selectTyphoon.wn10radii),
                TyphoonIntensity = selectTyphoon.trank
            };

            var yellowArr = new int[4]
            {
                int.Parse(typhoon.YellowRadius1.ToString()), int.Parse(typhoon.YellowRadius2.ToString()),
                int.Parse(typhoon.YellowRadius3.ToString()), int.Parse(typhoon.YellowRadius4.ToString())
            };
            var redArr = new int[4]
            {
                int.Parse(typhoon.RedRadius1.ToString()), int.Parse(typhoon.RedRadius2.ToString()),
                int.Parse(typhoon.RedRadius3.ToString()), int.Parse(typhoon.RedRadius4.ToString())
            };
            var index = 0;
            for (var i = 0; i < yellowArr.Length; i++)
            {
                if (yellowArr[i] > yellowArr[index])
                {
                    index = i;
                }
            }

            ConvertArr(yellowArr, redArr, out int[] m_NewYellowArr, out int[] m_NewRedArr);

            Bitmap bitmap = new Bitmap(1000, 1000);
            var g = Graphics.FromImage(bitmap);
            g.PageUnit = GraphicsUnit.Pixel;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            var borderWidth = 1;

            //画黄色外圈
            var penYellow = new Pen(Color.DodgerBlue, borderWidth);
            var maxYellowRadiusIndex = GetMaxRadiusIndex(m_NewYellowArr);
            switch (maxYellowRadiusIndex)
            {
                case 0:
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[1],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[1], m_NewYellowArr[1] * 2,
                        m_NewYellowArr[1] * 2, 0, 90); //二象限
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[2],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[2], m_NewYellowArr[2] * 2,
                        m_NewYellowArr[2] * 2, 90, 90); //三象限
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[3],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[3], m_NewYellowArr[3] * 2,
                        m_NewYellowArr[3] * 2, 180, 90); //四象限
                    g.DrawArc(penYellow, 0, 0, m_NewYellowArr[maxYellowRadiusIndex] * 2,
                        m_NewYellowArr[maxYellowRadiusIndex] * 2, 270, 90); //一象限
                    break;
                case 1:
                    g.DrawArc(penYellow, 0, 0, m_NewYellowArr[maxYellowRadiusIndex] * 2,
                        m_NewYellowArr[maxYellowRadiusIndex] * 2, 0, 90); //二象限
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[2],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[2], m_NewYellowArr[2] * 2,
                        m_NewYellowArr[2] * 2, 90, 90); //三象限
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[3],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[3], m_NewYellowArr[3] * 2,
                        m_NewYellowArr[3] * 2, 180, 90); //四象限
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[0],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[0], m_NewYellowArr[0] * 2,
                        m_NewYellowArr[0] * 2, 270, 90); //一象限
                    break;
                case 2:
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[1],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[1], m_NewYellowArr[1] * 2,
                        m_NewYellowArr[1] * 2, 0, 90); //二象限
                    g.DrawArc(penYellow, 0, 0, m_NewYellowArr[maxYellowRadiusIndex] * 2,
                        m_NewYellowArr[maxYellowRadiusIndex] * 2, 90, 90); //三象限
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[3],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[3], m_NewYellowArr[3] * 2,
                        m_NewYellowArr[3] * 2, 180, 90); //四象限
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[0],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[0], m_NewYellowArr[0] * 2,
                        m_NewYellowArr[0] * 2, 270, 90); //一象限
                    break;
                case 3:
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[1],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[1], m_NewYellowArr[1] * 2,
                        m_NewYellowArr[1] * 2, 0, 90); //二象限
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[2],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[2], m_NewYellowArr[2] * 2,
                        m_NewYellowArr[2] * 2, 90, 90); //三象限
                    g.DrawArc(penYellow, 0, 0, m_NewYellowArr[maxYellowRadiusIndex] * 2,
                        m_NewYellowArr[maxYellowRadiusIndex] * 2, 180, 90); //四象限
                    g.DrawArc(penYellow, m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[0],
                        m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[0], m_NewYellowArr[0] * 2,
                        m_NewYellowArr[0] * 2, 270, 90); //一象限
                    break;
            }
            g.DrawLine(penYellow,
                new Point(m_NewYellowArr[maxYellowRadiusIndex] + m_NewYellowArr[0], m_NewYellowArr[maxYellowRadiusIndex]),
                new Point(m_NewYellowArr[maxYellowRadiusIndex] + m_NewYellowArr[1], m_NewYellowArr[maxYellowRadiusIndex]));
            //X正轴
            g.DrawLine(penYellow,
                new Point(m_NewYellowArr[maxYellowRadiusIndex], m_NewYellowArr[maxYellowRadiusIndex] + m_NewYellowArr[1]),
                new Point(m_NewYellowArr[maxYellowRadiusIndex], m_NewYellowArr[maxYellowRadiusIndex] + m_NewYellowArr[2]));
            //Y负轴
            g.DrawLine(penYellow,
                new Point(m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[2], m_NewYellowArr[maxYellowRadiusIndex]),
                new Point(m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[3], m_NewYellowArr[maxYellowRadiusIndex]));
            //X负轴
            g.DrawLine(penYellow,
                new Point(m_NewYellowArr[maxYellowRadiusIndex], m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[3]),
                new Point(m_NewYellowArr[maxYellowRadiusIndex], m_NewYellowArr[maxYellowRadiusIndex] - m_NewYellowArr[0]));
            //Y正轴

            //因为黄的一定比红的大,所以最大半径用黄的
            var penRed = new Pen(Color.Orange, borderWidth);
            //画弧
            g.DrawArc(penRed, m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[1],
                m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[1], m_NewRedArr[1] * 2, m_NewRedArr[1] * 2, 0, 90); //二象限
            g.DrawArc(penRed, m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[2],
                m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[2], m_NewRedArr[2] * 2, m_NewRedArr[2] * 2, 90, 90);
            //三象限
            g.DrawArc(penRed, m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[3],
                m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[3], m_NewRedArr[3] * 2, m_NewRedArr[3] * 2, 180, 90);
            //四象限
            g.DrawArc(penRed, m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[0],
                m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[0], m_NewRedArr[0] * 2, m_NewRedArr[0] * 2, 270, 90);
            //一象限
            //画线  
            g.DrawLine(penRed,
                new Point(m_NewYellowArr[maxYellowRadiusIndex] + m_NewRedArr[0], m_NewYellowArr[maxYellowRadiusIndex]),
                new Point(m_NewYellowArr[maxYellowRadiusIndex] + m_NewRedArr[1], m_NewYellowArr[maxYellowRadiusIndex]));
            //X正轴
            g.DrawLine(penRed,
                new Point(m_NewYellowArr[maxYellowRadiusIndex], m_NewYellowArr[maxYellowRadiusIndex] + m_NewRedArr[1]),
                new Point(m_NewYellowArr[maxYellowRadiusIndex], m_NewYellowArr[maxYellowRadiusIndex] + m_NewRedArr[2]));
            //Y负轴
            g.DrawLine(penRed,
                new Point(m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[2], m_NewYellowArr[maxYellowRadiusIndex]),
                new Point(m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[3], m_NewYellowArr[maxYellowRadiusIndex]));
            //X负轴
            g.DrawLine(penRed,
                new Point(m_NewYellowArr[maxYellowRadiusIndex], m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[3]),
                new Point(m_NewYellowArr[maxYellowRadiusIndex], m_NewYellowArr[maxYellowRadiusIndex] - m_NewRedArr[0]));
            //Y正轴

            var img = new Bitmap(m_NewYellowArr[maxYellowRadiusIndex] * 2 + borderWidth,
                m_NewYellowArr[maxYellowRadiusIndex] * 2 + borderWidth);

            ImageCodecInfo myImageCodecInfo = GetEncoder(ImageFormat.Png);

            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(Encoder.Quality, 200L);

            myEncoderParameters.Param[0] = myEncoderParameter;

            bitmap.Save(@"C:\Users\digihail\Desktop\test.png", myImageCodecInfo, myEncoderParameters);
        }

        private void ConvertArr(int[] yellowArr, int[] redArr, out int[] newYellowArr, out int[] newRedArr)
        {
            var total = 0;
            var combineArr = yellowArr.Concat(redArr).ToArray();
            var index = GetMaxRadiusIndex(combineArr); //得到最大的半径
            var ratio = (double)500 / combineArr[index]; //将最大半径设为100,计算比例
            var newYellowRadius1 = Convert.ToInt32(yellowArr[0] * ratio);
            var newYellowRadius2 = Convert.ToInt32(yellowArr[1] * ratio);
            var newYellowRadius3 = Convert.ToInt32(yellowArr[2] * ratio);
            var newYellowRadius4 = Convert.ToInt32(yellowArr[3] * ratio);
            var newRedRadius1 = Convert.ToInt32(redArr[0] * ratio);
            var newRedRadius2 = Convert.ToInt32(redArr[1] * ratio);
            var newRedRadius3 = Convert.ToInt32(redArr[2] * ratio);
            var newRedRadius4 = Convert.ToInt32(redArr[3] * ratio);
            newYellowArr = new[] { newYellowRadius1, newYellowRadius2, newYellowRadius3, newYellowRadius4 };
            newRedArr = new[] { newRedRadius1, newRedRadius2, newRedRadius3, newRedRadius4 };
        }

        private int GetMaxRadiusIndex(int[] intArr)
        {
            var index = 0;
            for (var i = 0; i < intArr.Length; i++)
            {
                if (intArr[i] > intArr[index])
                {
                    index = i;
                }
            }
            return index;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

    }
}