namespace KmFlow
{
    /// <summary>
    ///     节点
    /// </summary>
    public class Node
    {
        /// <summary>
        ///     节点Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     节点类型
        /// </summary>
        public NodeType NodeType { get; set; }

        /// <summary>
        ///     逻辑
        /// </summary>
        public LogicType Logic { get; set; }

        /// <summary>
        ///     Or合并时，是否清空兄弟节点
        /// </summary>
        public bool OrClear { get; set; }

        /// <summary>
        ///     最近分流节点的ID
        /// </summary>
        public long SeparateNodeId { get; set; }

        /// <summary>
        ///     规则Id
        /// </summary>
        public long RuleId { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(NodeType)}: {NodeType}, {nameof(Logic)}: {Logic}, {nameof(OrClear)}: {OrClear}, {nameof(SeparateNodeId)}: {SeparateNodeId}, {nameof(RuleId)}: {RuleId}";
        }
    }
}