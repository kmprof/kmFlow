using System;

namespace KmFlow
{
    /// <summary>
    ///     结果节点
    /// </summary>
    public sealed class ResultNode
    {
        /// <summary>
        ///     节点Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     节点类型
        /// </summary>
        public NodeType NodeType { get; set; }

        /// <summary>
        ///     如果该节点是分流节点的子节点，那么该字段是最近分流节点的ID
        /// </summary>
        public long SeparateNodeId { get; set; }

        /// <summary>
        ///     结果类型
        /// </summary>
        public OperatingNodeType OperatingNodeType { get; set; }

        /// <summary>
        ///     操作时间
        /// </summary>
        public DateTime OperatingTime { get; set; }

        /// <summary>
        ///     规则Id
        /// </summary>
        public long RuleId { get; set; }
    }
}