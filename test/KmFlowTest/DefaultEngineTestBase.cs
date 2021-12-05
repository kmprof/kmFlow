using System;
using System.Collections.Generic;
using System.Linq;
using KmFlow;

namespace KmFlowTest
{
    public class DefaultEngineTestBase
    {
        protected Node[] Nodes;
        protected Line[] Lines;
        protected ReverseClearNode[] ReverseClearNodes = Array.Empty<ReverseClearNode>();

        protected void SetNodes(Node[] nodes)
        {
            Nodes = nodes;
        }

        protected void SetLine(Line[] lines)
        {
            Lines = lines;
        }
        protected void SetReverseClearNode(ReverseClearNode[] reverseClearNodes)
        {
            ReverseClearNodes = reverseClearNodes;
        }
        protected List<ResultNode> test_result(List<KeyValuePair<long, OperatingNodeType>> currentNodes, long lineId)
        {
            var resultNodes = from current in currentNodes
                              join node in Nodes on current.Key equals node.Id
                              select new ResultNode
                              {
                                  Id = node.Id,
                                  NodeType = node.NodeType,
                                  OperatingNodeType = current.Value,
                                  SeparateNodeId = node.SeparateNodeId
                              };
            return test_result(resultNodes.ToList(), lineId);
        }

        protected List<ResultNode> test_result(List<long> currentNodes, long lineId)
        {
            var resultNodes = from current in currentNodes
                              join node in Nodes on current equals node.Id
                              select new ResultNode
                              {
                                  Id = node.Id,
                                  NodeType = node.NodeType,
                                  OperatingNodeType = OperatingNodeType.Working,
                                  SeparateNodeId = node.SeparateNodeId
                              };
            return test_result(resultNodes.ToList(), lineId);
        }

        protected List<ResultNode> test_result(List<ResultNode> currentNodes, long lineId)
        {
            var flowChart = new FlowChart(Nodes, Lines);
            var process = new Process(flowChart);
            process.Load(ReverseClearNodes);
            process.Run(currentNodes, lineId);
            var result = process.Result;
            var passedLine = process.PassedLines;
            var current = process.Current;
            return result;
        }

        protected List<PassedLine> test_passed_line(List<KeyValuePair<long, OperatingNodeType>> currentNodes,
            long lineId)
        {
            var resultNodes = from current in currentNodes
                              join node in Nodes on current.Key equals node.Id
                              select new ResultNode
                              {
                                  Id = node.Id,
                                  NodeType = node.NodeType,
                                  OperatingNodeType = current.Value,
                                  SeparateNodeId = node.SeparateNodeId
                              };
            return test_passed_line(resultNodes.ToList(), lineId);
        }

        protected List<PassedLine> test_passed_line(List<long> currentNodes, long lineId)
        {
            var resultNodes = from current in currentNodes
                              join node in Nodes on current equals node.Id
                              select new ResultNode
                              {
                                  Id = node.Id,
                                  NodeType = node.NodeType,
                                  OperatingNodeType = OperatingNodeType.Working,
                                  SeparateNodeId = node.SeparateNodeId
                              };
            return test_passed_line(resultNodes.ToList(), lineId);
        }

        protected List<PassedLine> test_passed_line(List<ResultNode> currentNodes, long lineId)
        {
            var flowChart = new FlowChart(Nodes, Lines);
            var process = new Process(flowChart);
            process.Load(ReverseClearNodes);
            process.Run(currentNodes, lineId);
            var result = process.Result;
            var passedLine = process.PassedLines;
            var current = process.Current;
            return passedLine;
        }

        protected List<ResultNode> test_current(List<KeyValuePair<long, OperatingNodeType>> currentNodes, long lineId)
        {
            var resultNodes = from current in currentNodes
                              join node in Nodes on current.Key equals node.Id
                              select new ResultNode
                              {
                                  Id = node.Id,
                                  NodeType = node.NodeType,
                                  OperatingNodeType = current.Value,
                                  SeparateNodeId = node.SeparateNodeId
                              };
            return test_current(resultNodes.ToList(), lineId);
        }

        protected List<ResultNode> test_current(List<long> currentNodes, long lineId)
        {
            var resultNodes = from current in currentNodes
                              join node in Nodes on current equals node.Id
                              select new ResultNode
                              {
                                  Id = node.Id,
                                  NodeType = node.NodeType,
                                  OperatingNodeType = OperatingNodeType.Working,
                                  SeparateNodeId = node.SeparateNodeId
                              };
            return test_current(resultNodes.ToList(), lineId);
        }

        protected List<ResultNode> test_current(List<ResultNode> currentNodes, long lineId)
        {
            var flowChart = new FlowChart(Nodes, Lines);
            var process = new Process(flowChart);
            process.Load(ReverseClearNodes);
            process.Run(currentNodes, lineId);
            var result = process.Result;
            var passedLine = process.PassedLines;
            var current = process.Current;
            return current;
            //Assert.Equal(target.OrderBy(x => x.Id), current.OrderBy(x => x.Id), new ResultNodeEqualityComparer());
        }
    }
}