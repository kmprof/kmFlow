namespace KmFlow
{
    /// <summary>
    ///     线
    /// </summary>
    public class Line
    {
        /// <summary>
        ///     Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     头部节点Id
        /// </summary>
        public long HeadNodeId { get; set; }

        /// <summary>
        ///     尾部节点Id
        /// </summary>
        public long TailNodeId { get; set; }

        /// <summary>
        ///     线的类型
        /// </summary>
        public LineType LineType { get; set; } = LineType.Positive;

        /// <summary>
        ///     编码
        /// </summary>
        public string Code { get; set; }
    }
}