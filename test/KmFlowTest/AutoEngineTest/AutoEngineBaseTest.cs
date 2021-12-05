using KmFlow;
using System.Collections.Generic;
using System;
using Xunit;
using KmFlow.AutoEngines;
using System.Linq;

namespace KmFlowTest.AutoEngineTest
{
    public class AutoEngineBaseTest
    {
        [Fact(DisplayName = "基本测试")]
        public void test()
        {
            var nodes = new[]
            {
                new Node { Id = 1,Logic =(LogicType)1, Name = "开始", OrClear = false, NodeType = (NodeType)1, SeparateNodeId = 0,RuleId = 1 },
                new Node { Id = 2,Logic =(LogicType)1, Name = "常规1", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0,RuleId = 2 },
                new Node { Id = 3,Logic =(LogicType)1, Name = "常规2", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0,RuleId = 3 },
                new Node { Id = 4,Logic =(LogicType)1, Name = "常规3", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0,RuleId = 4 },
                new Node { Id = 5,Logic =(LogicType)1, Name = "结束", OrClear = false, NodeType = (NodeType)2, SeparateNodeId = 0,RuleId = 5},
                new Node { Id = 6,Logic =(LogicType)1, Name = "常规4", OrClear = false, NodeType = (NodeType)5, SeparateNodeId = 0,RuleId =6 }
            };
            var lines = new[]
            {
                new Line {Id = 1, TailNodeId = 1,HeadNodeId = 2, LineType = (LineType)1, Code = ""},
                new Line {Id = 2, TailNodeId = 2,HeadNodeId = 3, LineType = (LineType)1, Code = "002"},
                new Line {Id = 3, TailNodeId = 3,HeadNodeId = 4, LineType = (LineType)1, Code = "003"},
                new Line {Id = 4, TailNodeId = 4,HeadNodeId = 5, LineType = (LineType)1, Code = ""},
                new Line {Id = 5, TailNodeId = 3,HeadNodeId = 6, LineType = (LineType)1, Code = "005"},
                new Line {Id = 6, TailNodeId = 6,HeadNodeId = 4, LineType = (LineType)1, Code = ""}
            };
            var data = new TestData(){ Value = "A", Record = new List<string>()};
            var context = new AutoEngineTestContext(data);
            var flowChart = new FlowChart(nodes,lines);
            var process = new Process(flowChart,new AutoEngine(context));
            //var data = new TestData() { Name = "Jack", Record = new List<string>() };
            process.Load();
            try
            {
                process.Run(new List<ResultNode>(),1);
                var result = process.Result;
                var target = new List<ResultNode>
                {
                    new ResultNode {Id = 1, OperatingNodeType = OperatingNodeType.NormalFinish},
                    new ResultNode {Id = 2, OperatingNodeType = OperatingNodeType.NormalFinish},
                    new ResultNode {Id = 3, OperatingNodeType = OperatingNodeType.NormalFinish}
                };
                Assert.Equal(target.OrderBy(x => x.Id), result.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
            }
            catch (Exception e)
            {
                Assert.True(false);
            }
        }
    }
}