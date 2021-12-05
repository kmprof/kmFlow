using System.Collections.Generic;
using System.Linq;
using KmFlow;
using Xunit;

namespace KmFlowTest
{
    /// <summary>
    ///     常规测试
    /// </summary>
    public class NormalTest : DefaultEngineTestBase
    {
        private readonly Node[] _nodes =
        {
            new Node {Id = 1, Name = "开始", NodeType = NodeType.Start, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 2, Name = "N2", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 3, Name = "N3", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 4, Name = "S4", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 5, Name = "N5", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 4},
            new Node {Id = 6, Name = "N6", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 4},
            new Node
            {
                Id = 7, Name = "M7", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 4,
                Logic = LogicType.And
            },
            new Node {Id = 8, Name = "N8", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 9, Name = "S9", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 10, Name = "N10", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 9},
            new Node {Id = 11, Name = "N11", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 9},
            new Node
            {
                Id = 12, Name = "M12", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 9,
                Logic = LogicType.Or
            },
            new Node {Id = 13, Name = "N13", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 14, Name = "S14", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 15, Name = "N15", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 14},
            new Node {Id = 16, Name = "N16", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 14},
            new Node
            {
                Id = 17, Name = "M17", NodeType = NodeType.Merge, OrClear = true, SeparateNodeId = 14,
                Logic = LogicType.Or
            },
            new Node {Id = 18, Name = "N18", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 19, Name = "结束", NodeType = NodeType.End, OrClear = false, SeparateNodeId = 0}
        };

        private readonly Line[] _lines =
        {
            new Line {Id = 1, HeadNodeId = 2, TailNodeId = 1},
            new Line {Id = 2, HeadNodeId = 3, TailNodeId = 2},
            new Line {Id = 3, HeadNodeId = 4, TailNodeId = 3},
            new Line {Id = 4, HeadNodeId = 5, TailNodeId = 4},
            new Line {Id = 5, HeadNodeId = 6, TailNodeId = 4},
            new Line {Id = 6, HeadNodeId = 7, TailNodeId = 5},
            new Line {Id = 7, HeadNodeId = 7, TailNodeId = 6},
            new Line {Id = 8, HeadNodeId = 8, TailNodeId = 7},
            new Line {Id = 9, HeadNodeId = 9, TailNodeId = 8},
            new Line {Id = 10, HeadNodeId = 10, TailNodeId = 9},
            new Line {Id = 11, HeadNodeId = 11, TailNodeId = 9},
            new Line {Id = 12, HeadNodeId = 12, TailNodeId = 10},
            new Line {Id = 13, HeadNodeId = 12, TailNodeId = 11},
            new Line {Id = 14, HeadNodeId = 13, TailNodeId = 12},
            new Line {Id = 15, HeadNodeId = 14, TailNodeId = 13},
            new Line {Id = 16, HeadNodeId = 15, TailNodeId = 14},
            new Line {Id = 17, HeadNodeId = 16, TailNodeId = 14},
            new Line {Id = 18, HeadNodeId = 17, TailNodeId = 15},
            new Line {Id = 19, HeadNodeId = 17, TailNodeId = 16},
            new Line {Id = 20, HeadNodeId = 18, TailNodeId = 17},
            new Line {Id = 21, HeadNodeId = 19, TailNodeId = 18}
        };

        public NormalTest()
        {
            SetLine(_lines);
            SetNodes(_nodes);
        }

        /// <summary>
        ///     测试开始的节点结果
        /// </summary>
        [Fact(DisplayName = "开始节点——结果")]
        public void start_result()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 1, OperatingNodeType = OperatingNodeType.NormalFinish}
            };
            var result = test_result(new List<long>(), 1);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        /// <summary>
        ///     测试开始走过的线
        /// </summary>
        [Fact(DisplayName = "开始节点——连线")]
        public void start_passed_line()
        {
            var target = new List<PassedLine> {new PassedLine(1)};
            var result = test_passed_line(new List<long>(), 1);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());
        }

        /// <summary>
        ///     测试开始_当前节点
        /// </summary>
        [Fact(DisplayName = "开始节点——当前节点")]
        public void start_current()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 2, OperatingNodeType = OperatingNodeType.Working}
            };
            var result = test_current(new List<long>(), 1);
            Assert.Equal(target, result, new ResultNodeEqualityComparer());
        }

        #region 普通节点到普通节点

        [Fact(DisplayName = "普通节点到普通节点节点——结果")]
        public void general_node_to_general_node_result()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 2, OperatingNodeType = OperatingNodeType.NormalFinish}
            };
            var result = test_result(new List<long> {2}, 2);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "普通节点到普通节点——连线")]
        public void general_node_to_general_node_passed_line()
        {
            var target = new List<PassedLine> {new PassedLine(2)};
            var result = test_passed_line(new List<long> {2}, 2);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());
        }

        [Fact(DisplayName = "普通节点到普通节点当前——节点")]
        public void general_node_to_general_node_current()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 3, OperatingNodeType = OperatingNodeType.Working}
            };
            var result = test_current(new List<long> {2}, 2);
            Assert.Equal(target, result, new ResultNodeEqualityComparer());
        }

        #endregion

        #region 分流节点到普通节点

        [Fact(DisplayName = "分流节点到普通节点节点——结果")]
        public void separate_node_to_general_node_result()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 3, OperatingNodeType = OperatingNodeType.NormalFinish},
                new ResultNode {Id = 4, OperatingNodeType = OperatingNodeType.AutoFinish}
            };
            var result = test_result(new List<long> {3}, 3);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "分流节点到普通节点节——连线")]
        public void separate_node_to_general_passed_line()
        {
            var target = new List<PassedLine> {new PassedLine(3), new PassedLine(4), new PassedLine(5)};
            var result = test_passed_line(new List<long> {3}, 3);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());
        }

        [Fact(DisplayName = "分流节点到普通节点——当前节点")]
        public void separate_node_to_general_node_current()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 5, OperatingNodeType = OperatingNodeType.Working},
                new ResultNode {Id = 6, OperatingNodeType = OperatingNodeType.Working}
            };
            var result = test_current(new List<long> {3}, 3);
            Assert.Equal(target, result, new ResultNodeEqualityComparer());
        }

        #endregion

        #region 且合并不通过

        [Fact(DisplayName = "且合并节点不通过结果")]
        public void merge_node_not_finish_and_result()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 5, OperatingNodeType = OperatingNodeType.NormalFinish}
            };
            var result = test_result(new List<long> {5, 6}, 6);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "且合并节点不通过经过的线")]
        public void merge_node_not_finish_and_passed_line()
        {
            var target = new List<PassedLine> {new PassedLine(6)};
            var result = test_passed_line(new List<long> {5, 6}, 6);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());
        }

        [Fact(DisplayName = "且合并节点不通过当前节点")]
        public void merge_node_not_finish_and_current()
        {
            var result = test_current(new List<long> {5, 6}, 6);
            Assert.Empty(result);
        }

        #endregion

        #region 且合并通过

        [Fact(DisplayName = "且合并节点通过结果")]
        public void merge_node_finish_and_result()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 6, OperatingNodeType = OperatingNodeType.NormalFinish},
                new ResultNode {Id = 7, OperatingNodeType = OperatingNodeType.AutoFinish}
            };
            var result = test_result(new List<long> {6}, 7);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "且合并节点通过经过的线")]
        public void merge_node_finish_and_passed_line()
        {
            var target = new List<PassedLine> {new PassedLine(7), new PassedLine(8)};
            var result = test_passed_line(new List<long> {6}, 7);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());
        }

        [Fact(DisplayName = "且合并节点通过当前节点")]
        public void merge_node_finish_and_current()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 8, OperatingNodeType = OperatingNodeType.Working}
            };
            var result = test_current(new List<long> {6}, 7);
            Assert.Equal(target, result, new ResultNodeEqualityComparer());
        }

        #endregion

        #region 或合并清空

        [Fact(DisplayName = "或合并清空——结果")]
        public void merge_node_finish_or_clear_result()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 15, OperatingNodeType = OperatingNodeType.NormalFinish},
                new ResultNode {Id = 16, OperatingNodeType = OperatingNodeType.BrotherFinish},
                new ResultNode {Id = 17, OperatingNodeType = OperatingNodeType.AutoFinish}
            };
            var result = test_result(new List<long> {15, 16}, 18);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "或合并清空——经过的线")]
        public void merge_node_finish_or_clear_passed_line()
        {
            var target = new List<PassedLine> {new PassedLine(18), new PassedLine(20)};
            var result = test_passed_line(new List<long> {15, 16}, 18);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());
        }

        [Fact(DisplayName = "或合并清空——当前节点")]
        public void merge_node_finish_or_clear_current()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 18, OperatingNodeType = OperatingNodeType.Working}
            };
            var result = test_current(new List<long> {15, 16}, 18);
            Assert.Equal(target, result, new ResultNodeEqualityComparer());
        }

        #endregion

        #region 或合并不清空

        [Fact(DisplayName = "或合并不清空——结果")]
        public void merge_node_finish_or_not_clear_result()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 10, OperatingNodeType = OperatingNodeType.NormalFinish}
            };
            var result = test_result(new List<long> {10, 11}, 12);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "或合并不清空——经过的线")]
        public void merge_node_finish_or_not_clear_passed_line()
        {
            var target = new List<PassedLine> {new PassedLine(12), new PassedLine(14)};
            var result = test_passed_line(new List<long> {10, 11}, 12);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());
        }

        [Fact(DisplayName = "或合并不清空——当前节点2")]
        public void merge_node_finish_or_not_clear_current()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 12, OperatingNodeType = OperatingNodeType.OrFinish},
                new ResultNode {Id = 13, OperatingNodeType = OperatingNodeType.Working}
            };
            var result = test_current(new List<long> {10, 11}, 12);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        #endregion

        #region 或合并不清空多余节点

        [Fact(DisplayName = "或合并不清空多余节点——结果")]
        public void merge_node_finish_or_not_clear_other_result()
        {
            var target = new List<ResultNode>
            {
                new ResultNode {Id = 11, OperatingNodeType = OperatingNodeType.NormalFinish},
                new ResultNode {Id = 12, OperatingNodeType = OperatingNodeType.OrAllFinish}
            };
            var result = test_result(new List<KeyValuePair<long, OperatingNodeType>>
            {
                new KeyValuePair<long, OperatingNodeType>(11, OperatingNodeType.Working),
                new KeyValuePair<long, OperatingNodeType>(12, OperatingNodeType.OrFinish)
            }, 13);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "或合并不清空多余节点——经过的线")]
        public void merge_node_finish_or_not_clear_other_passed_line()
        {
            var target = new List<PassedLine> {new PassedLine(13)};
            var result = test_passed_line(new List<KeyValuePair<long, OperatingNodeType>>
            {
                new KeyValuePair<long, OperatingNodeType>(11, OperatingNodeType.Working),
                new KeyValuePair<long, OperatingNodeType>(12, OperatingNodeType.OrFinish)
            }, 13);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());
        }

        [Fact(DisplayName = "或合并不清空多余节点——当前节点")]
        public void merge_node_finish_or_not_clear_other_current()
        {
            var result = test_current(new List<KeyValuePair<long, OperatingNodeType>>
            {
                new KeyValuePair<long, OperatingNodeType>(11, OperatingNodeType.Working),
                new KeyValuePair<long, OperatingNodeType>(12, OperatingNodeType.OrFinish)
            }, 13);
            Assert.Empty(result);
        }

        #endregion
    }
}