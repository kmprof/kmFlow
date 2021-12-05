using System;

namespace KmFlow
{
    /// <summary>
    /// 走过的线
    /// </summary>
   public sealed class PassedLine
    {
        public PassedLine(long id,DateTime now)
        {
            Id = id;
            OperatingTime = now;
        }
        public PassedLine(long id)
        {
            Id = id;
            OperatingTime = DateTime.Now;
        }
        /// <summary>
        /// 连线Id
        /// </summary>
        public long Id { get; private set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatingTime { get; private set; }
    }
}
