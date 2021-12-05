using System;
using System.Collections.Generic;
using System.Linq;
using KmFlow;
using Xunit;

namespace KmFlowTest
{

    public class OrNotClearTest: DefaultEngineTestBase
    {
        private readonly Node[] _nodes =
        {
            new Node {Id = 1, Name = "", NodeType = NodeType.Start, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 2, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 3, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 4, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 5, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 6, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 3},
            new Node {Id = 7, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 3},
            new Node
            {
                Id = 8, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 3, Logic = LogicType.Or
            },
            new Node {Id = 9, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 18, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 9},
            new Node
            {
                Id = 19, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 9, Logic = LogicType.Or
            },
            new Node
            {
                Id = 20, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 2, Logic = LogicType.Or
            },
            new Node {Id = 11, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 5},
            new Node {Id = 12, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 5},
            new Node {Id = 14, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 5},
            new Node {Id = 15, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 5},
            new Node {Id = 16, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 5},
            new Node
            {
                Id = 17, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 5, Logic = LogicType.Or
            },
            new Node {Id = 22, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 23, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 22},
            new Node {Id = 24, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 22},
            new Node
            {
                Id = 25, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 22,
                Logic = LogicType.Or
            },
            new Node {Id = 21, Name = "", NodeType = NodeType.End, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 26, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 27, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 28, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 2},
        };

        private readonly Line[] _lines =
        {
            new Line {Id = 1, TailNodeId = 1, HeadNodeId = 2},
            new Line {Id = 2, TailNodeId = 2, HeadNodeId = 3},
            new Line {Id = 3, TailNodeId = 2, HeadNodeId = 4},
            new Line {Id = 4, TailNodeId = 2, HeadNodeId = 5},
            new Line {Id = 5, TailNodeId = 3, HeadNodeId = 6},
            new Line {Id = 6, TailNodeId = 3, HeadNodeId = 7},
            new Line {Id = 7, TailNodeId = 6, HeadNodeId = 8},
            new Line {Id = 8, TailNodeId = 7, HeadNodeId = 8},
            new Line {Id = 9, TailNodeId = 4, HeadNodeId = 9},
            new Line {Id = 11, TailNodeId = 5, HeadNodeId = 11},
            new Line {Id = 12, TailNodeId = 5, HeadNodeId = 12},
            new Line {Id = 14, TailNodeId = 11, HeadNodeId = 14},
            new Line {Id = 15, TailNodeId = 12, HeadNodeId = 15},
            new Line {Id = 17, TailNodeId = 14, HeadNodeId = 17},
            new Line {Id = 18, TailNodeId = 15, HeadNodeId = 16},
            new Line {Id = 19, TailNodeId = 16, HeadNodeId = 17},
            new Line {Id = 20, TailNodeId = 9, HeadNodeId = 18},
            new Line {Id = 21, TailNodeId = 18, HeadNodeId = 19},
            new Line {Id = 22, TailNodeId = 19, HeadNodeId = 26},
            new Line {Id = 23, TailNodeId = 8, HeadNodeId = 22},
            new Line {Id = 24, TailNodeId = 22, HeadNodeId = 23},
            new Line {Id = 25, TailNodeId = 22, HeadNodeId = 24},
            new Line {Id = 26, TailNodeId = 23, HeadNodeId = 25},
            new Line {Id = 27, TailNodeId = 24, HeadNodeId = 25},
            new Line {Id = 28, TailNodeId = 25, HeadNodeId = 28},
            new Line {Id = 29, TailNodeId = 20, HeadNodeId = 21},
            new Line {Id = 30, TailNodeId = 17, HeadNodeId = 27},
            new Line {Id = 31, TailNodeId = 26, HeadNodeId = 20},
            new Line {Id = 32, TailNodeId = 27, HeadNodeId = 20},
            new Line {Id = 33, TailNodeId = 28, HeadNodeId = 20}
        };

        public OrNotClearTest()
        {
            SetLine(_lines);
            SetNodes(_nodes);
        }

        #region 只有一个兄弟节点的不清空或合并
        [Fact(DisplayName = "只有一个兄弟节点的不清空或合并——结果")]
        public void one_brother_result()
        {

            var target = new List<ResultNode>()
            {
                new ResultNode(){ Id=18,OperatingNodeType=OperatingNodeType.NormalFinish},
                new ResultNode(){ Id=19,OperatingNodeType=OperatingNodeType.OrAllFinish}
            };
            var result = test_result(new List<long> { 18 }, 21);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "只有一个兄弟节点的不清空或合并——连线")]
        public void one_brother_passed_line()
        {

            var target = new List<PassedLine>() { new PassedLine(21), new PassedLine(22) };
            var result = test_passed_line(new List<long> { 18 }, 21);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());

        }

        [Fact(DisplayName = "只有一个兄弟节点的不清空或合并——当前节点")]
        public void one_brother_current()
        {
            var target = new List<ResultNode>()
            {
                new ResultNode(){ Id=19,OperatingNodeType=OperatingNodeType.OrFinish},
                new ResultNode(){ Id=26,OperatingNodeType=OperatingNodeType.Working}
            };
            var current = test_current(new List<long> { 18 }, 21);
            Assert.Equal(target.OrderBy(x => x.Id), current.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }
        #endregion

        #region 只有一个兄弟节点的不清空或合并
        [Fact(DisplayName = "多层不清空或合并——结果")]
        public void multilayer_result()
        {
            var target = new List<ResultNode>()
            {
                new ResultNode(){ Id=14,OperatingNodeType=OperatingNodeType.NormalFinish}
            };
            var result = test_result(new List<long> { 14,15 }, 17);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "多层不清空或合并——连线")]
        public void multilayer_passed_line()
        {

            var target = new List<PassedLine>() { new PassedLine(17), new PassedLine(30) };
            var result = test_passed_line(new List<long> { 14, 15 }, 17);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());

        }

        [Fact(DisplayName = "多层不清空或合并——当前节点")]
        public void multilayer_current()
        {
            var target = new List<ResultNode>()
            {
                new ResultNode(){ Id=17,OperatingNodeType=OperatingNodeType.OrFinish},
                new ResultNode(){ Id=27,OperatingNodeType=OperatingNodeType.Working}
            };
            var current = test_current(new List<long> { 14, 15 }, 17);
            Assert.Equal(target.OrderBy(x => x.Id), current.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }
        #endregion

        #region 只有一个兄弟节点的不清空或合并
        [Fact(DisplayName = "只有一个兄弟节点的不清空或合并——结果")]
        public void clear_all_result()
        {
            var target = new List<ResultNode>()
            {
                new ResultNode(){ Id=16,OperatingNodeType=OperatingNodeType.NormalFinish},
                new ResultNode(){ Id=17,OperatingNodeType=OperatingNodeType.OrAllFinish},
                new ResultNode(){ Id=20,OperatingNodeType=OperatingNodeType.OrAllFinish}
            };
            var result = test_result(new List<KeyValuePair<long, OperatingNodeType>>()
            {
                new KeyValuePair<long, OperatingNodeType>(20,OperatingNodeType.OrFinish),
                new KeyValuePair<long, OperatingNodeType>(16, OperatingNodeType.Working),
                new KeyValuePair<long, OperatingNodeType>(17,OperatingNodeType.OrFinish)
            }, 19);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }

        [Fact(DisplayName = "只有一个兄弟节点的不清空或合并——连线")]
        public void clear_all_passed_line()
        {
            var target = new List<PassedLine>() { new PassedLine(19) };
            var result = test_passed_line(new List<KeyValuePair<long, OperatingNodeType>>()
            {
                new KeyValuePair<long, OperatingNodeType>(20,OperatingNodeType.OrFinish),
                new KeyValuePair<long, OperatingNodeType>(16, OperatingNodeType.Working),
                new KeyValuePair<long, OperatingNodeType>(17,OperatingNodeType.OrFinish)
            }, 19);
            Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new PassedLineEqualityComparer());

        }

        [Fact(DisplayName = "只有一个兄弟节点的不清空或合并——当前节点")]
        public void clear_all_current()
        {
            var current = test_current(new List<KeyValuePair<long, OperatingNodeType>>()
            {
                new KeyValuePair<long, OperatingNodeType>(20,OperatingNodeType.OrFinish),
                new KeyValuePair<long, OperatingNodeType>(16, OperatingNodeType.Working),
                new KeyValuePair<long, OperatingNodeType>(17,OperatingNodeType.OrFinish)
            }, 19);
            Assert.Empty(current);
        }
        #endregion
    }
}