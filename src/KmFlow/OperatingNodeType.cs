using System;

namespace KmFlow
{
    /// <summary>
    ///     操作节点类型
    /// </summary>
    [Flags]
    public enum OperatingNodeType : short
    {
        /// <summary>
        ///     正常结束
        /// </summary>
        NormalFinish = 1,

        /// <summary>
        ///     流程自动结束，例如，分流节点
        /// </summary>
        AutoFinish = 2,

        /// <summary>
        ///     Or 合并时，结束兄弟节点使用
        /// </summary>
        BrotherFinish = 4,

        /// <summary>
        ///     Or 节点结束时，还有子节点未结束
        /// </summary>
        OrFinish = 32,

        /// <summary>
        ///     not clear Or 节点的全部节点结束
        /// </summary>
        OrAllFinish = 64,

        /// <summary>
        ///     正在工作中的节点
        /// </summary>
        Working = 128,

        /// <summary>
        ///     反向被清空
        /// </summary>
        ReverseClear = 256
    }
}