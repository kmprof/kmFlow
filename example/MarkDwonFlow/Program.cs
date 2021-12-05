﻿using System;
using KmFlow;

namespace MarkDwonFlow
{
    internal class Program
    {
        private static readonly Node[] _nodes =
        {
            new Node {Id = 0, Name = "", NodeType = NodeType.Start, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 1, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 2, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 3, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 4, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 5, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 6, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 5},
            new Node {Id = 8, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 5},
            new Node {Id = 9, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 5},
            new Node
            {
                Id = 10, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 5,
                Logic = LogicType.And
            },
            new Node {Id = 11, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 12, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 2},
            new Node
            {
                Id = 13, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 2,
                Logic = LogicType.And
            },
            new Node {Id = 14, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 15, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 2},
            new Node {Id = 16, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 15},
            new Node {Id = 17, Name = "", NodeType = NodeType.Separate, OrClear = false, SeparateNodeId = 15},
            new Node {Id = 18, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 17},
            new Node {Id = 19, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 17},
            new Node
            {
                Id = 20, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 17,
                Logic = LogicType.And
            },
            new Node
            {
                Id = 21, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 15,
                Logic = LogicType.And
            },
            new Node
            {
                Id = 22, Name = "", NodeType = NodeType.Merge, OrClear = false, SeparateNodeId = 2,
                Logic = LogicType.And
            },
            new Node {Id = 23, Name = "", NodeType = NodeType.General, OrClear = false, SeparateNodeId = 0},
            new Node {Id = 24, Name = "", NodeType = NodeType.End, OrClear = false, SeparateNodeId = 0}
        };

        private static readonly Line[] _lines =
        {
            new Line {Id = 1, TailNodeId = 0, HeadNodeId = 1},
            new Line {Id = 2, TailNodeId = 1, HeadNodeId = 2},
            new Line {Id = 3, TailNodeId = 2, HeadNodeId = 3},
            new Line {Id = 4, TailNodeId = 2, HeadNodeId = 4},
            new Line {Id = 5, TailNodeId = 3, HeadNodeId = 5},
            new Line {Id = 6, TailNodeId = 5, HeadNodeId = 6},
            new Line {Id = 7, TailNodeId = 5, HeadNodeId = 8},
            new Line {Id = 8, TailNodeId = 6, HeadNodeId = 9},
            new Line {Id = 9, TailNodeId = 8, HeadNodeId = 10},
            new Line {Id = 10, TailNodeId = 9, HeadNodeId = 10},
            new Line {Id = 11, TailNodeId = 10, HeadNodeId = 13},
            new Line {Id = 12, TailNodeId = 4, HeadNodeId = 11},
            new Line {Id = 13, TailNodeId = 11, HeadNodeId = 12},
            new Line {Id = 14, TailNodeId = 12, HeadNodeId = 4, LineType = LineType.Reverse},
            new Line {Id = 15, TailNodeId = 12, HeadNodeId = 13},
            new Line {Id = 16, TailNodeId = 13, HeadNodeId = 22},
            new Line {Id = 17, TailNodeId = 2, HeadNodeId = 14},
            new Line {Id = 18, TailNodeId = 14, HeadNodeId = 15},
            new Line {Id = 19, TailNodeId = 15, HeadNodeId = 16},
            new Line {Id = 20, TailNodeId = 15, HeadNodeId = 17},
            new Line {Id = 21, TailNodeId = 16, HeadNodeId = 21},
            new Line {Id = 22, TailNodeId = 17, HeadNodeId = 18},
            new Line {Id = 23, TailNodeId = 17, HeadNodeId = 19},
            new Line {Id = 24, TailNodeId = 18, HeadNodeId = 20},
            new Line {Id = 25, TailNodeId = 19, HeadNodeId = 20},
            new Line {Id = 26, TailNodeId = 20, HeadNodeId = 21},
            new Line {Id = 27, TailNodeId = 21, HeadNodeId = 22},
            new Line {Id = 28, TailNodeId = 22, HeadNodeId = 23},
            new Line {Id = 29, TailNodeId = 23, HeadNodeId = 24},
            new Line {Id = 30, TailNodeId = 3, HeadNodeId = 1, LineType = LineType.Reverse},
            new Line {Id = 31, TailNodeId = 9, HeadNodeId = 3, LineType = LineType.Reverse},
            new Line {Id = 32, TailNodeId = 9, HeadNodeId = 1, LineType = LineType.Reverse},
            new Line {Id = 33, TailNodeId = 23, HeadNodeId = 1, LineType = LineType.Reverse},
            new Line {Id = 34, TailNodeId = 19, HeadNodeId = 1, LineType = LineType.Reverse}
        };

        private static void Main(string[] args)
        {
            var flowChart = new FlowChart(_nodes, _lines);
            var en = new MarkdownEngine();
            var process = new Process(flowChart, en);
            process.Load();
            process.Run(1);
            var result = @$"
```mermaid
graph TD
{en.MarkdownTxt}
```";
            Console.ReadKey();
        }
    }
}