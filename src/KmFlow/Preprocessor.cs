using System.Collections.Generic;
using System.Linq;
using KmFlow.Preprocessors;

namespace KmFlow
{
    /// <summary>
    ///     流程预处理器
    /// </summary>
    public class Preprocessor
    {
        /// <summary>
        ///     流程中所有的节点
        /// </summary>
        public Node[] Nodes { get; private set; }

        /// <summary>
        ///     流程中所有的连线
        /// </summary>
        public Line[] Lines { get; private set; }

        /// <summary>
        ///     反向线
        /// </summary>
        public List<Line> ReverseLine { get; set; }
        /// <summary>
        ///     反向线和节点的关系
        /// </summary>
        public List<ReverseClearNode> ReverseLineAndNodes { get; set; }
        /// <summary>
        ///     待更新的节点
        /// </summary>
        public List<Node> ToUpdateNode { get; private set; }

        /// <summary>
        ///     加载所需数据
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <param name="lines">连线集合</param>
        public void Load(Node[] nodes, Line[] lines)
        {
            Nodes = nodes;
            Lines = lines;
        }

        /// <summary>
        ///     预处理
        /// </summary>
        /// <returns></returns>
        public void Pretreatment()
        {
            ReverseLine=new List<Line>();
            ToUpdateNode = new List<Node>();
            ReverseLineHandler();
            UpdateNodeHandler();
            FindReverseLineSeparateNodeHandler();
        }

        private void ReverseLineHandler()
        {
            var reverseLineHandler = new MarkReverseLine(Nodes, Lines);
            reverseLineHandler.Mark();
            foreach (var line in reverseLineHandler.Lines.Where(x => x.IsOldReverse == false && x.LineType == LineType.Reverse))
            {
                Lines.First(x => x.Id == line.Id).LineType = LineType.Reverse;
                ReverseLine.Add(line);
            }

            
        }

        private void UpdateNodeHandler()
        {
            var separateNodeAnalysis = new AddSeparateId(Nodes, Lines);
            separateNodeAnalysis.Add();
            ToUpdateNode = separateNodeAnalysis.Nodes.ToList();
        }
        private void FindReverseLineSeparateNodeHandler()
        {
            var separateNodeAnalysis = new FindReverseLineSeparateNode(Nodes, Lines);
            separateNodeAnalysis.Find();
            ReverseLineAndNodes = separateNodeAnalysis.ReverseLineAndNodes;
        }
    }
}