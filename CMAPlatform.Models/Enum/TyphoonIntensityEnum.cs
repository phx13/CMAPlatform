using System;

namespace CMAPlatform.Models.Enum
{
    public enum TyphoonIntensityEnum : int
    {
        /// <summary>
        /// 热带低气压
        /// </summary>
        TD = 2,

        /// <summary>
        /// 热带风暴
        /// </summary>
        TS = 4,

        /// <summary>
        /// 强烈热带风暴
        /// </summary>
        STS = 6,

        /// <summary>
        /// 台风
        /// </summary>
        TY = 4,

        /// <summary>
        /// 强台风
        /// </summary>
        STY = 5,

        /// <summary>
        /// 超强台风
        /// </summary>
        SuperTY = 6
    }
}