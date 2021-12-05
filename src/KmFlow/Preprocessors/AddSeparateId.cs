using System;
using System.Collections.Generic;
using System.Linq;

namespace KmFlow.Preprocessors
{
    /// <summary>
    ///     添加分流节点Id处理程序
    /// </summary>
    /// <remarks>
    ///     基于深度优先算法实现
    /// </remarks>
    public class AddSeparateId
    {
        /// <summary>
        ///     流程中所有的节点
        /// </summary>
        public Node[] Nodes { get; set; }

        /// <summary>
        ///     流程中所有的连线
        /// </summary>
        public List<LineInPreprocessor> Lines { get; }

        /// <summary>
        ///     运行中的所有节点栈
        /// </summary>
        private readonly Stack<Node> _running;

        /// <summary>
        ///     运行中的分流节点栈
        /// </summary>
        private readonly Stack<Node> _separateStack;

        /// <summary>
        ///     运行中的合并节点栈
        /// </summary>
        private readonly Stack<Node> _mergeStack;

        public AddSeparateId(Node[] nodes, Line[] lines)
        {
            Nodes = nodes;
            Lines = lines.Select(x => new LineInPreprocessor(x)).ToList();
            _running = new Stack<Node>();
            _separateStack = new Stack<Node>();
            _mergeStack = new Stack<Node>();
        }

        /// <summary>
        ///     添加
        /// </summary>
        public void Add()
        {
            var startNodes = Nodes.Where(x => x.NodeType == NodeType.Start).ToList();
            if (!startNodes.Any()) throw new FlowException("流程缺少开始节点");
            foreach (var startNode in startNodes)
            {
                _running.Push(startNode);
                var line = GetNodeLine(startNode);
                while (line != null)
                {
                    AddInner(line);
                    line = GetNodeLine(startNode);
                }
            }
        }

        /// <summary>
        ///     添加的处理方法
        /// </summary>
        /// <param name="line">正在处理的连线</param>
        private void AddInner(LineInPreprocessor line)
        {
            long separateNodeId = 0;
            if (_separateStack.TryPeek(out var separateNode))
                separateNodeId = separateNode.Id;
            line.IsVisited = true;
            var processingNode = LineHeadNode(line); //正在处理中的节点
            switch (processingNode.NodeType)
            {
                case NodeType.Start:
                    break;
                case NodeType.End:
                    ProcessInStack();
                    break;
                case NodeType.Separate:
                {
                    processingNode.SeparateNodeId = separateNodeId;
                    _separateStack.Push(processingNode);
                    _running.Push(processingNode);
                    ProcessInStack();
                    break;
                }
                case NodeType.Merge:
                    processingNode.SeparateNodeId = separateNodeId;
                    if (!_mergeStack.Contains(processingNode))
                    {
                        _mergeStack.Push(processingNode);
                        //if (_separateStack.Count != _mergeStack.Count) 
                        //    throw new FlowException("两个分流部分有交叉");
                    }

                    break;
                case NodeType.General:
                {
                    processingNode.SeparateNodeId = separateNodeId;
                    _running.Push(processingNode);
                    ProcessInStack();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     从栈里读取数据
        /// </summary>
        private void ProcessInStack()
        {
            while (_running.TryPeek(out var processingNode))
            {
                var newLine = GetNodeLine(processingNode);
                if (newLine != null)
                {
                    AddInner(newLine);
                }
                else
                {
                    _running.Pop();
                    if (processingNode.NodeType == NodeType.Separate)
                    {
                        _separateStack.Pop();
                        processingNode = _mergeStack.Pop();
                        newLine = GetNodeLine(processingNode);
                        if (newLine == null)
                            throw new FlowException("分流节点必须有一条出线");
                        AddInner(newLine);
                    }
                }
            }
        }

        private Node LineHeadNode(LineInPreprocessor line)
        {
            return Nodes.First(x => x.Id == line.HeadNodeId);
        }

        /// <summary>
        ///     获取节点的连线
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private LineInPreprocessor GetNodeLine(Node node)
        {
            return Lines.FirstOrDefault(x =>
                x.TailNodeId == node.Id && !x.IsVisited && x.LineType == LineType.Positive);
        }
    }
}