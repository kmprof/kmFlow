using System;
using System.Collections.Generic;
using System.Linq;
using KmFlow;
using Xunit;

namespace KmFlowTest.Preprocessors
{
    public class MarkReverseLineTest
    {
        [Fact]
        public void simple_test()
        {
            var nodes = new[]
            {
                new Node {Id = 1, Name = "", NodeType = NodeType.Start, OrClear = false, SeparateNodeId = 0},
                new Node {Id = 2, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 0},
                new Node {Id = 3, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 0},
                new Node {Id = 4, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 3},
                new Node {Id = 5, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 3},
                new Node {Id = 6, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 3},
                new Node
                {
                    Id = 7, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 3,
                    Logic = LogicType.And
                },
                new Node {Id = 8, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 0},
                new Node {Id = 9, Name = "", NodeType = NodeType.End, OrClear = false, SeparateNodeId = 0}
            };
            var lines = new[]
            {
                new Line {Id = 1, TailNodeId = 1, HeadNodeId = 2},
                new Line {Id = 2, TailNodeId = 2, HeadNodeId = 3},
                new Line {Id = 3, TailNodeId = 3, HeadNodeId = 4},
                new Line {Id = 4, TailNodeId = 4, HeadNodeId = 5},
                new Line {Id = 5, TailNodeId = 3, HeadNodeId = 6},
                new Line {Id = 6, TailNodeId = 5, HeadNodeId = 7},
                new Line {Id = 7, TailNodeId = 6, HeadNodeId = 7},
                new Line {Id = 8, TailNodeId = 7, HeadNodeId = 8},
                new Line {Id = 9, TailNodeId = 8, HeadNodeId = 9},
                new Line {Id = 10, HeadNodeId = 4, TailNodeId = 5},
                new Line {Id = 11, HeadNodeId = 3, TailNodeId = 5},
                new Line {Id = 12, HeadNodeId = 2, TailNodeId = 5},
                new Line {Id = 13, HeadNodeId = 2, TailNodeId = 6},
                new Line {Id = 14, HeadNodeId = 2, TailNodeId = 8}
            };
            var preprocessor = new Preprocessor();
            preprocessor.Load(nodes, lines);
            try
            {
                var target = new List<Line>
                {
                    new Line {Id = 10}, new Line {Id = 11}, new Line {Id = 12}, new Line {Id = 13}, new Line {Id = 14}
                };
                preprocessor.Pretreatment();
                var result = preprocessor.ReverseLine;
                Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new LineEqualityComparer());
            }
            catch (Exception e)
            {
                Assert.True(false);
            }
        }
    }
}