
namespace KmFlow
{
    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        ///     开始节点
        /// </summary>
        Start = 1,

        /// <summary>
        ///     结束节点
        /// </summary>
        End = 2,

        /// <summary>
        ///     分流节点
        /// </summary>
        Separate = 3,

        /// <summary>
        ///     合并节点
        /// </summary>
        Merge = 4,

        /// <summary>
        ///     常规节点
        /// </summary>
        General = 5
    }
}
