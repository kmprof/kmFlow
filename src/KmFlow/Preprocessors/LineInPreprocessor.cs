namespace KmFlow.Preprocessors
{
    /// <summary>
    ///     预处理中的线
    /// </summary>
    public class LineInPreprocessor : Line
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public LineInPreprocessor()
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="line"></param>
        public LineInPreprocessor(Line line)
        {
            TailNodeId = line.TailNodeId;
            HeadNodeId = line.HeadNodeId;
            Id = line.Id;
            LineType = line.LineType;
            IsVisited = false;
            if (LineType == LineType.Reverse)
                IsOldReverse = true;
        }

        /// <summary>
        ///     预处理的
        /// </summary>
        public bool IsVisited { get; set; }
        /// <summary>
        /// 是否是已经存在的反向线
        /// </summary>
        public bool IsOldReverse { get; set; }
    }
}