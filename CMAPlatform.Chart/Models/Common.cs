using System;
using System.Drawing;

namespace CMAPlatform.Chart.Models
{
    /// <summary>
    ///     共通类
    /// </summary>
    public class Common
    {
        /// <summary>
        ///     气泡间距
        /// </summary>
        public static double BubbleInterval = 3;

        /// <summary>
        ///     最小气泡半径
        /// </summary>
        public static double MinRadius = 10;

        /// <summary>
        ///     气泡变大系数
        /// </summary>
        public static double BiggerIndex = 3;

        /// <summary>
        ///     子气泡模式
        /// </summary>
        public static bool SubMode = false;

        /// <summary>
        ///     一次加载气泡数
        /// </summary>
        public static int SpanNum = 10;

        /// <summary>
        ///     计算角度对应的坐标象限
        /// </summary>
        /// <param name="angle">角度</param>
        /// <returns></returns>
        public static int GetQuadrant(double angle)
        {
            // 第一象限
            if (angle < Math.PI/2 && angle >= 0)
            {
                return 1;
            }
            if (angle < Math.PI && angle >= Math.PI/2)
            {
                // 第二象限
                return 2;
            }
            if (angle < Math.PI*3/2 && angle >= Math.PI)
            {
                // 第三象限
                return 3;
            }
            // 第四象限
            return 4;
        }

        /// <summary>
        ///     判断角度是否是右侧指定范围角度内
        /// </summary>
        /// <param name="targetAngle">基准角度</param>
        /// <param name="rangeAngle">角度范围</param>
        /// <param name="checkAngle">判定对象角度</param>
        /// <returns></returns>
        public static bool RightAngleCheck(double targetAngle,
            double rangeAngle,
            double checkAngle)
        {
            if (targetAngle - rangeAngle > 0)
            {
                if (checkAngle >= targetAngle - rangeAngle && checkAngle <= targetAngle)
                {
                    return true;
                }
                return false;
            }
            if (checkAngle >= 0 && checkAngle <= targetAngle ||
                checkAngle < Math.PI*2 && checkAngle >= Math.PI*2 - (rangeAngle - targetAngle))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     判断角度是否是右侧指定范围角度内
        /// </summary>
        /// <param name="targetAngle">基准角度</param>
        /// <param name="rangeAngle">角度范围</param>
        /// <param name="checkAngle">判定对象角度</param>
        /// <param name="directionType">true:顺时针，false:逆时针</param>
        /// <returns></returns>
        public static bool RightAngleCheck(double targetAngle,
            double rangeAngle,
            double checkAngle,
            bool directionType)
        {
            // 顺时针
            if (directionType)
            {
                if (targetAngle - rangeAngle > 0)
                {
                    if (checkAngle >= targetAngle - rangeAngle && checkAngle <= targetAngle)
                    {
                        return true;
                    }
                    return false;
                }
                if (checkAngle >= 0 && checkAngle <= targetAngle ||
                    checkAngle < Math.PI*2 && checkAngle >= Math.PI*2 - (rangeAngle - targetAngle))
                {
                    return true;
                }
                return false;
            }
            if (targetAngle < rangeAngle)
            {
                if (checkAngle >= targetAngle && checkAngle <= targetAngle + rangeAngle)
                {
                    return true;
                }
                return false;
            }
            if (checkAngle < Math.PI*2 && checkAngle >= targetAngle ||
                checkAngle >= 0 && checkAngle <= targetAngle - Math.PI)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     计算目标点相对于参照点的角度
        /// </summary>
        /// <param name="targetPoint">目标点</param>
        /// <param name="referencePoint">参照点</param>
        /// <returns></returns>
        public static double CalcAngle(Point targetPoint,
            Point referencePoint)
        {
            // 第一象限
            if (targetPoint.X > referencePoint.X && targetPoint.Y <= referencePoint.Y)
            {
                return Math.Tanh(Math.Abs((targetPoint.Y - referencePoint.Y)/(targetPoint.X - referencePoint.X)));
            }
            if (targetPoint.X <= referencePoint.X && targetPoint.Y < referencePoint.Y)
            {
                // 第二象限
                if (targetPoint.X - referencePoint.X != 0)
                {
                    return Math.PI -
                           Math.Tanh(Math.Abs((targetPoint.Y - referencePoint.Y)/(targetPoint.X - referencePoint.X)));
                }
                return Math.PI/2;
            }
            if (targetPoint.X < referencePoint.X && targetPoint.Y >= referencePoint.Y)
            {
                // 第三象限
                return Math.PI +
                       Math.Tanh(Math.Abs((targetPoint.Y - referencePoint.Y)/(targetPoint.X - referencePoint.X)));
            }
            // 第四象限
            if (targetPoint.X - referencePoint.X != 0)
            {
                return Math.PI*2 -
                       Math.Tanh(Math.Abs((targetPoint.Y - referencePoint.Y)/(targetPoint.X - referencePoint.X)));
            }
            return Math.PI*3/2;
        }
    }
}