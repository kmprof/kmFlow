using System;
using System.Collections.Generic;
using System.Linq;
using KmFlow;
using Xunit;

namespace KmFlowTest.Preprocessors
{
    public class AddSeparateIdTest
    {
        /// <summary>
        /// 只有一个分流
        /// </summary>
        [Fact]
        public void simple_test()
        {
            var nodes = new[]
            {
                new Node() { Id = 1,Logic =(LogicType)1, Name = "开始", OrClear = false, NodeType = (NodeType)1, SeparateNodeId = 0 },
                new Node() { Id = 2,Logic =(LogicType)1, Name = "常规1", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 3,Logic =(LogicType)1, Name = "分流", OrClear = false, NodeType = (NodeType)3, SeparateNodeId = 0 },
                new Node() { Id = 4,Logic =(LogicType)1, Name = "常规2", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 5,Logic =(LogicType)1, Name = "常规3", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 6,Logic =(LogicType)1, Name = "合并", OrClear = false, NodeType = (NodeType)4, SeparateNodeId = 0 },
                new Node() { Id = 7,Logic =(LogicType)1, Name = "结束", OrClear = false, NodeType = (NodeType)2, SeparateNodeId = 0 }
            };
            var lines = new[]
            {
                new Line {Id = 1, TailNodeId = 1,HeadNodeId = 2, LineType = (LineType)1},
                new Line {Id = 2, TailNodeId = 2,HeadNodeId = 3, LineType = (LineType)1},
                new Line {Id = 3, TailNodeId = 3,HeadNodeId = 4, LineType = (LineType)1},
                new Line {Id = 4, TailNodeId = 3,HeadNodeId = 5, LineType = (LineType)1},
                new Line {Id = 5, TailNodeId = 4,HeadNodeId = 6, LineType = (LineType)1},
                new Line {Id = 6, TailNodeId = 5,HeadNodeId = 6, LineType = (LineType)1},
                new Line {Id = 7, TailNodeId = 6,HeadNodeId = 7, LineType = (LineType)1}
            };
            var preprocessor = new Preprocessor();
            preprocessor.Load(nodes, lines);
            try
            {
                var target = new List<Node> { new Node() { Id = 4, SeparateNodeId = 3 }, new Node() { Id = 5, SeparateNodeId = 3 }, new Node() { Id = 6, SeparateNodeId = 3 } };
                preprocessor.Pretreatment();
                //var da = preprocessor.ReverseLine;
                var result = preprocessor.ToUpdateNode.Where(x => x.SeparateNodeId != 0);
                Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new NodeWithSeparateNodeEqualityComparer());
            }
            catch (Exception e)
            {
                Assert.True(false);
            }
        }
        /// <summary>
        /// 多层
        /// </summary>
        [Fact]
        public void multilayer_test()
        {
            var nodes = new[]
            {
                new Node() { Id = 1,Logic =(LogicType)1, Name = "开始", OrClear = false, NodeType = (NodeType)1, SeparateNodeId = 0 },
                new Node() { Id = 2,Logic =(LogicType)1, Name = "分流1", OrClear = false, NodeType = (NodeType)3, SeparateNodeId = 0 },
                new Node() { Id = 3,Logic =(LogicType)1, Name = "常规1", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 4,Logic =(LogicType)1, Name = "常规2", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 5,Logic =(LogicType)1, Name = "常规3", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 6,Logic =(LogicType)1, Name = "分流2", OrClear = false, NodeType = (NodeType)3, SeparateNodeId = 0 },
                new Node() { Id = 7,Logic =(LogicType)1, Name = "常规4", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 8,Logic =(LogicType)1, Name = "常规5", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 9,Logic =(LogicType)1, Name = "常规6", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 10,Logic =(LogicType)1, Name = "合并2", OrClear = false, NodeType = (NodeType)4, SeparateNodeId = 0 },
                new Node() { Id = 11,Logic =(LogicType)1, Name = "合并1", OrClear = false, NodeType = (NodeType)4, SeparateNodeId = 0 },
                new Node() { Id = 12,Logic =(LogicType)1, Name = "结束", OrClear = false, NodeType = (NodeType)2, SeparateNodeId = 0 },
            };
            var lines = new[]
            {
                new Line {Id = 1, TailNodeId = 1,HeadNodeId = 2, LineType = (LineType)1},
                new Line {Id = 2, TailNodeId = 2,HeadNodeId = 3, LineType = (LineType)1},
                new Line {Id = 3, TailNodeId = 2,HeadNodeId = 6, LineType = (LineType)1},
                new Line {Id = 4, TailNodeId = 3,HeadNodeId = 4, LineType = (LineType)1},
                new Line {Id = 5, TailNodeId = 4,HeadNodeId = 5, LineType = (LineType)1},
                new Line {Id = 6, TailNodeId = 5,HeadNodeId = 11, LineType = (LineType)1},
                new Line {Id = 7, TailNodeId = 6,HeadNodeId = 7, LineType = (LineType)1},
                new Line {Id = 8, TailNodeId = 6,HeadNodeId = 8, LineType = (LineType)1},
                new Line {Id = 9, TailNodeId = 8,HeadNodeId = 9, LineType = (LineType)1},
                new Line {Id = 10, TailNodeId = 7,HeadNodeId = 10, LineType = (LineType)1},
                new Line {Id = 11, TailNodeId = 9,HeadNodeId = 10, LineType = (LineType)1},
                new Line {Id = 12, TailNodeId = 10,HeadNodeId = 11, LineType = (LineType)1},
                new Line {Id = 13, TailNodeId = 11,HeadNodeId = 12, LineType = (LineType)1}
            };
            var preprocessor = new Preprocessor();
            preprocessor.Load(nodes, lines);
            try
            {
                var target = new List<Node>
                {
                    new Node() { Id = 1, SeparateNodeId = 0 },
                    new Node() { Id = 2, SeparateNodeId = 0 },
                    new Node() { Id = 3, SeparateNodeId = 2 },
                    new Node() { Id = 4, SeparateNodeId = 2 },
                    new Node() { Id = 5, SeparateNodeId = 2 },
                    new Node() { Id = 6, SeparateNodeId = 2 },
                    new Node() { Id = 11, SeparateNodeId = 2 },
                    new Node() { Id = 7, SeparateNodeId = 6 },
                    new Node() { Id = 8, SeparateNodeId = 6 },
                    new Node() { Id = 9, SeparateNodeId = 6 },
                    new Node() { Id = 10, SeparateNodeId = 6 },
                    new Node() { Id = 12, SeparateNodeId = 0 },
                };
                preprocessor.Pretreatment();
                var da = preprocessor.ReverseLine;
                var result = preprocessor.ToUpdateNode;
                Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new NodeWithSeparateNodeEqualityComparer());
            }
            catch (Exception e)
            {
                Assert.True(false);
            }
        }
        /// <summary>
        /// 两个分流部分有交叉
        /// </summary>
        [Fact]
        public void multilayer_error_test()
        {
            var nodes = new[]
            {
                new Node() { Id = 1,Logic =(LogicType)1, Name = "开始", OrClear = false, NodeType = (NodeType)1, SeparateNodeId = 0 },
                new Node() { Id = 2,Logic =(LogicType)1, Name = "分流1", OrClear = false, NodeType = (NodeType)3, SeparateNodeId = 0 },
                new Node() { Id = 3,Logic =(LogicType)1, Name = "常规1", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 4,Logic =(LogicType)1, Name = "常规2", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 5,Logic =(LogicType)1, Name = "常规3", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 6,Logic =(LogicType)1, Name = "分流2", OrClear = false, NodeType = (NodeType)3, SeparateNodeId = 0 },
                new Node() { Id = 7,Logic =(LogicType)1, Name = "常规4", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 8,Logic =(LogicType)1, Name = "常规5", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 9,Logic =(LogicType)1, Name = "常规6", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0 },
                new Node() { Id = 10,Logic =(LogicType)1, Name = "合并2", OrClear = false, NodeType = (NodeType)4, SeparateNodeId = 0 },
                new Node() { Id = 11,Logic =(LogicType)1, Name = "合并1", OrClear = false, NodeType = (NodeType)4, SeparateNodeId = 0 },
                new Node() { Id = 12,Logic =(LogicType)1, Name = "结束", OrClear = false, NodeType = (NodeType)2, SeparateNodeId = 0 },
            };
            var lines = new[]
            {
                new Line {Id = 1, TailNodeId = 1,HeadNodeId = 2, LineType = (LineType)1},
                new Line {Id = 2, TailNodeId = 2,HeadNodeId = 3, LineType = (LineType)1},
                new Line {Id = 3, TailNodeId = 2,HeadNodeId = 6, LineType = (LineType)1},
                new Line {Id = 4, TailNodeId = 3,HeadNodeId = 4, LineType = (LineType)1},
                new Line {Id = 5, TailNodeId = 4,HeadNodeId = 5, LineType = (LineType)1},
                new Line {Id = 6, TailNodeId = 5,HeadNodeId = 11, LineType = (LineType)1},
                new Line {Id = 7, TailNodeId = 6,HeadNodeId = 7, LineType = (LineType)1},
                new Line {Id = 8, TailNodeId = 6,HeadNodeId = 8, LineType = (LineType)1},
                new Line {Id = 9, TailNodeId = 8,HeadNodeId = 9, LineType = (LineType)1},
                new Line {Id = 10, TailNodeId = 7,HeadNodeId = 10, LineType = (LineType)1},
                new Line {Id = 11, TailNodeId = 9,HeadNodeId = 10, LineType = (LineType)1},
                new Line {Id = 12, TailNodeId = 10,HeadNodeId = 11, LineType = (LineType)1},
                new Line {Id = 13, TailNodeId = 11,HeadNodeId = 12, LineType = (LineType)1},
                new Line {Id = 14, TailNodeId = 4,HeadNodeId = 7, LineType = (LineType)1}
            };
            var preprocessor = new Preprocessor();
            preprocessor.Load(nodes, lines);
            try
            {
                var ex = Assert.Throws<FlowException>(() =>
                {
                    preprocessor.Pretreatment();
                });
                Assert.Equal("两个分流部分有交叉", ex.Message);
            }
            catch (Exception e)
            {

                Assert.True(false);
            }
        }
    }
}