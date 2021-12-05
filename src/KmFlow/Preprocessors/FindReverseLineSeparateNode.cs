using System.Collections.Generic;
using System.Linq;

namespace KmFlow.Preprocessors
{
    /// <summary>
    ///     找出反向线所关联的所有的分流节点
    /// </summary>
    public class FindReverseLineSeparateNode
    {
        /// <summary>
        ///     流程中所有的节点
        /// </summary>
        private readonly Node[] _nodes;
        /// <summary>
        /// 所有的连线
        /// </summary>
        private readonly Line[] _lines;
        

        /// <summary>
        ///     反向线和节点的关系
        /// </summary>
        public List<ReverseClearNode> ReverseLineAndNodes { get; set; }

        /// <summary>
        ///     构造方法
        /// </summary>
        /// <param name="nodes">节点</param>
        /// <param name="lines">连线</param>
        public FindReverseLineSeparateNode(Node[] nodes, Line[] lines)
        {
            _nodes = nodes;
            _lines = lines;
            ReverseLineAndNodes = new List<ReverseClearNode>();
        }

        public void Find()
        {
            var reverseLines = _lines.Where(x => x.LineType == LineType.Reverse).ToList();
            foreach (var line in reverseLines)
            {
                var handler = new Handler(_lines.Select(x => new LineInPreprocessor(x)).ToList(), _nodes);
                handler.Find(line);
                ReverseLineAndNodes.AddRange(handler.ReverseLineAndNodes);
                
            }
        }
        private class Handler
        {
            /// <summary>
            ///     运行中的节点
            /// </summary>
            private readonly Stack<Node> _running;
            /// <summary>
            ///     流程中所有的节点
            /// </summary>
            private readonly Node[] _nodes;
            /// <summary>
            ///     流程中所有的连线
            /// </summary>
            private List<LineInPreprocessor> Lines { get; }

            private long _tailNodeId;
            private  long _lineId;
            /// <summary>
            ///     反向线和节点的关系
            /// </summary>
            public List<ReverseClearNode> ReverseLineAndNodes { get; }
            public Handler(List<LineInPreprocessor> lines, Node[] nodes)
            {
                _running = new Stack<Node>();
                Lines = lines;
                _nodes = nodes;
                ReverseLineAndNodes = new List<ReverseClearNode>();
            }
            public void Find(Line line)
            {
                _tailNodeId = line.TailNodeId;
                _lineId = line.Id;
                var startNode = _nodes.First(x => x.Id == line.HeadNodeId);
                _running.Push(startNode);
                var newLine = Lines.FirstOrDefault(x => x.TailNodeId == startNode.Id && !x.IsVisited && x.LineType == LineType.Positive);
                while (newLine != null)
                {
                    FindInner(newLine);
                    newLine = Lines.FirstOrDefault(x => x.TailNodeId == startNode.Id && !x.IsVisited && x.LineType == LineType.Positive);
                }
            }

            /// <summary>
            ///     发现的处理方法
            /// </summary>
            /// <param name="line">正在处理的线</param>
            private void FindInner(LineInPreprocessor line)
            {
                line.IsVisited = true;
                var runningNode = LineHeadNode(line); //运行中的节点
                if (_running.Contains(runningNode) || runningNode.Id == _tailNodeId)
                {
                    ProcessInStack();
                }
                else
                {
                    if (runningNode.NodeType == NodeType.Separate )
                    {
                        Add(runningNode.Id);
                        ProcessInStack();
                    }
                    else
                    {
                        var newLine = Lines.FirstOrDefault(x => x.TailNodeId == runningNode.Id && !x.IsVisited && x.LineType == LineType.Positive);
                        if (newLine != null)
                        {
                            _running.Push(runningNode);
                            FindInner(newLine);
                        }
                        else
                        {
                            ProcessInStack();
                        }
                    }
                    
                    //var newLine = Lines.Where(x => x.TailNodeId == runningNode.Id && !x.IsVisited).ToList();
                    //if (newLine.Any())
                    //{
                    //    _running.Push(runningNode);
                    //    foreach (var item in newLine) FindInner(item);
                    //}
                    //else
                    //{
                    //    ProcessInStack();
                    //}
                }
            }

            /// <summary>
            ///     从栈里读取数据
            /// </summary>
            private void ProcessInStack()
            {
                while (_running.TryPeek(out var processingNode))
                {
                    var newLine = Lines.FirstOrDefault(x => x.TailNodeId == processingNode.Id && !x.IsVisited && x.LineType==LineType.Positive);
                    if (newLine != null)
                        FindInner(newLine);
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

            private void Add(long separateId)
            {
                if (!ReverseLineAndNodes.Exists(x => x.LineId == _lineId && x.NodeId == separateId))
                    ReverseLineAndNodes.Add(new ReverseClearNode { LineId = _lineId, NodeId = separateId });
                var nextSeparateNodes = _nodes.Where(x => x.SeparateNodeId == separateId && x.NodeType == NodeType.Separate)
                    .ToList();
                foreach (var item in nextSeparateNodes)
                {
                    Add(item.Id);
                }
            }


        }
        
    }
}