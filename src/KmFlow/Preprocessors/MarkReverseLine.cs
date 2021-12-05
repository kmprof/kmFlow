using System.Collections.Generic;
using System.Linq;

namespace KmFlow.Preprocessors
{
    /// <summary>
    ///     找出反向的节点
    /// </summary>
    /// <remarks>
    ///     使用了深度优先算法
    /// </remarks>
    internal class MarkReverseLine
    {
        /// <summary>
        ///     流程中所有的节点
        /// </summary>
        private readonly Node[] _nodes;

        /// <summary>
        ///     流程中所有的连线
        /// </summary>
        public List<LineInPreprocessor> Lines { get; }

        /// <summary>
        ///     运行中的节点
        /// </summary>
        private readonly Stack<Node> _running;
        

        /// <summary>
        ///     构造方法
        /// </summary>
        /// <param name="nodes">节点</param>
        /// <param name="lines">连线</param>
        public MarkReverseLine(Node[] nodes, Line[] lines)
        {
            _nodes = nodes;
            Lines = lines.Select(x => new LineInPreprocessor(x)).ToList();
            _running = new Stack<Node>();
        }

        /// <summary>
        ///     标记
        /// </summary>
        public void Mark()
        {
            var startNodes = _nodes.Where(x => x.NodeType == NodeType.Start).ToList();
            if (!startNodes.Any())
                throw new FlowException("流程缺少开始节点");
            foreach (var startNode in startNodes)
            {
                _running.Push(startNode);
                var line = Lines.FirstOrDefault(x => x.TailNodeId == startNode.Id && !x.IsVisited);
                while (line != null)
                {
                    MarkInner(line);
                    line = Lines.FirstOrDefault(x => x.TailNodeId == startNode.Id && !x.IsVisited);
                }
            }
        }

        /// <summary>
        ///     标记的处理方法
        /// </summary>
        /// <param name="line">正在处理的线</param>
        private void MarkInner(LineInPreprocessor line)
        {
            line.IsVisited = true;
            var runningNode = LineHeadNode(line); //运行中的节点
            if (_running.Contains(runningNode))
            {
                //如果正在操作的节点在正在运行节点栈中，说明该线连接后形成一个圈，因此该线是一个反向线
                line.LineType = LineType.Reverse;
                ProcessInStack();
            }
            else
            {
                var newLine = Lines.FirstOrDefault(x => x.TailNodeId == runningNode.Id && !x.IsVisited);
                if (newLine != null)
                {
                    _running.Push(runningNode);
                    MarkInner(newLine);
                }
                else
                {
                    ProcessInStack();
                }
            }
        }

        /// <summary>
        ///     从栈里读取数据
        /// </summary>
        private void ProcessInStack()
        {
            while (_running.TryPeek(out var processingNode))
            {
                var newLine = Lines.FirstOrDefault(x => x.TailNodeId == processingNode.Id && !x.IsVisited);
                if (newLine != null)
                    MarkInner(newLine);
                else
                    _running.Pop();
            }
        }

        /// <summary>
        ///     获取线的头部节点
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private Node LineHeadNode(LineInPreprocessor line)
        {
            return _nodes.First(x => x.Id == line.HeadNodeId);
        }
    }
}