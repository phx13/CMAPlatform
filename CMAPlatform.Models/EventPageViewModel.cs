namespace CMAPlatform.Models
{
    public class EventPageViewModel
    {
        /// <summary>
        ///     编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     视角高度
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        ///     视角Yaw
        /// </summary>
        public double Yaw { get; set; }

        /// <summary>
        ///     视角Pitch
        /// </summary>
        public double Pitch { get; set; }

        /// <summary>
        ///     是否默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}