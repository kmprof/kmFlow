using System.Collections.Generic;
using System.Linq;
using KmFlow;

namespace MarkDwonFlow
{
    /// <summary>
    ///     用于生成Markdown的流程图(mermaid)
    /// </summary>
    public class MarkdownEngine : AbstractEngine
    {
        /// <summary>
        ///     生成的Markdown文本
        /// </summary>
        public string MarkdownTxt { get; private set; }

        /// <summary>
        ///     记录
        /// </summary>
        private readonly List<string> _record = new List<string>();

        public override void Execute(long lineId)
        {
            MarkdownExecute();
        }

        private void MarkdownExecute()
        {
            foreach (var line in FlowChart.Lines)
            {
                var headNode = FlowChart.Nodes.First(x => x.Id == line.HeadNodeId);
                var tailNode = FlowChart.Nodes.First(x => x.Id == line.TailNodeId);
                var headNodeType = GetNodeType(headNode);
                var tailNodeType = GetNodeType(tailNode);
                _record.Add(
                    $"{tailNodeType}{tailNode.Id}(({tailNodeType}{tailNode.Id})) --{line.Id}-->{headNodeType}{headNode.Id}(({headNodeType}{headNode.Id})) ");
            }

            MarkdownTxt = string.Join("\r\n", _record);
        }

        private string GetNodeType(Node input)
        {
            return input.NodeType switch
            {
                NodeType.Start => "Start",
                NodeType.End => "End",
                NodeType.Separate => "S",
                NodeType.Merge when input.Logic == LogicType.And => "MA",
                NodeType.Merge when input.Logic == LogicType.Or && input.OrClear => "MOC",
                NodeType.Merge when input.Logic == LogicType.Or && !input.OrClear => "MON",
                NodeType.General => "G",
                _ => "未知"
            };
        }
    }
}