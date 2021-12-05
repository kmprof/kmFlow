using System.Linq;
using System;

namespace KmFlow
{
    /// <summary>
    ///     流程引擎
    /// </summary>
    public abstract class AbstractEngine
    {
        /// <summary>
        ///     开始节点（启动节点）
        /// </summary>
        private Node _startNode;
        /// <summary>
        ///     流程图
        /// </summary>
        protected FlowChart FlowChart { get; private set; }

        /// <summary>
        ///     流程
        /// </summary>
        protected Process Process { get; private set; }

        /// <summary>
        ///     输入流程图和流程
        /// </summary>
        /// <param name="flowChart">流程图</param>
        /// <param name="process">流程</param>
        internal void Input(FlowChart flowChart, Process process)
        {
            FlowChart = flowChart;
            Process = process;
        }

        /// <summary>
        ///     执行
        /// </summary>
        /// <param name="lineId">线的Id</param>
        public abstract void Execute(long lineId);

        /// <summary>
        ///     实际执行的方法
        /// </summary>
        /// <param name="lineId"></param>
        protected void ExecuteInner(long lineId)
        {
            ExecuteBefore(lineId);
            ExecuteHandler(lineId);
            ExecuteAfter(lineId);
        }

        /// <summary>
        ///     执行之前需要处理的事情
        /// </summary>
        /// <param name="lineId"></param>
        protected void ExecuteBefore(long lineId)
        {
            _startNode = FlowChart.GetLineTailNode(lineId);
            //if (_startNode.NodeType == NodeType.Start)
            //    SaveNodeToCurrentNodes(_startNode);
            SaveNodeToResult(_startNode, OperatingNodeType.NormalFinish);
            ReverseClearNode(lineId);
        }

        /// <summary>
        ///     执行之后，扫尾
        /// </summary>
        /// <param name="lineId"></param>
        protected void ExecuteAfter(long lineId)
        {
            var orAllFinish =
                Process.Result.FirstOrDefault(x => x.OperatingNodeType == OperatingNodeType.OrAllFinish);
            if (orAllFinish != null) //多层or 不清空节点 上一层结束时，结束下一层的or 合并
                ClearOrFinishNode();
        }

        /// <summary>
        ///     实际执行的方法
        /// </summary>
        /// <param name="lineId"></param>
        protected void ExecuteHandler(long lineId)
        {
            SaveLineToPassedLine(lineId);
            var headNode = FlowChart.GetLineHeadNode(lineId);
            switch (headNode.NodeType)
            {
                case NodeType.Start:
                    StartNodeHandler(headNode);
                    break;
                case NodeType.End:
                    EndNodeHandler(headNode);
                    break;
                case NodeType.Separate:
                    SeparateNodeHandler(headNode);
                    break;
                case NodeType.Merge:
                    MergeNodeHandler(headNode);
                    break;
                case NodeType.General:
                    GeneralNodeHandler(headNode);
                    return;
            }
        }

        /// <summary>
        ///     反向连线,清空节点
        /// </summary>
        /// <param name="lineId"></param>
        protected void ReverseClearNode(long lineId)
        {
            var separateNodes = Process.ReverseClearNodes.Where(x => x.LineId == lineId);
            var newHistoryCurrentNodes = Process.HistoryCurrentNodes.Where(x => x.Id != _startNode.Id).ToList();
            if (newHistoryCurrentNodes.Count > 0)
                foreach (var item in separateNodes)
                {
                    var networkNodes = FlowChart.GetNetworkNodes(item.NodeId);
                    foreach (var node in newHistoryCurrentNodes)
                        if (networkNodes.Exists(x => x.Id == node.Id))
                            if (!Process.Result.Exists(x => x.Id == node.Id
                                                            && x.OperatingNodeType == OperatingNodeType.ReverseClear))
                                SaveNodeToResult(FlowChart.Nodes.First(x => x.Id == node.Id),
                                    OperatingNodeType.ReverseClear);
                }
        }

        /// <summary>
        ///     开始节点处理器
        /// </summary>
        /// <param name="node">开始节点</param>
        protected virtual void StartNodeHandler(Node node)
        {
            SaveNodeToCurrentNodes(node);
        }

        /// <summary>
        ///     结束节点处理器
        /// </summary>
        /// <param name="node">结束节点</param>
        protected virtual void EndNodeHandler(Node node)
        {
            SaveNodeToCurrentNodes(node, OperatingNodeType.AutoFinish);
            //SaveNodeToResult(node, OperatingNodeType.AutoFinish);
        }

        /// <summary>
        ///     分流节点处理器
        /// </summary>
        /// <param name="node">分流节点</param>
        protected virtual void SeparateNodeHandler(Node node)
        {
            SaveNodeToResult(node, OperatingNodeType.AutoFinish);
            var lines = FlowChart.Lines.Where(x => x.TailNodeId == node.Id);
            foreach (var item in lines) ExecuteHandler(item.Id);
        }

        /// <summary>
        ///     合并节点处理器
        /// </summary>
        /// <param name="node">合并节点</param>
        protected virtual void MergeNodeHandler(Node node)
        {
            if (node.Logic == LogicType.And)
                AndMergeNodeHandler(node);
            else
                OrMergeNodeHandler(node);
        }

        /// <summary>
        ///     且合并处理器
        /// </summary>
        /// <param name="node">合并节点</param>
        protected virtual void AndMergeNodeHandler(Node node)
        {
            var networkNodes = FlowChart.GetNetworkNodes(node)
                .Where(x => x.NodeType == NodeType.General && x.Id != _startNode.Id);
            foreach (var item in networkNodes)
                if (Process.HistoryCurrentNodes.Any(x => x.Id == item.Id))
                    return;
            SaveNodeToResult(node, OperatingNodeType.AutoFinish);
            var nextLine = FlowChart.Lines.First(x => x.TailNodeId == node.Id);
            ExecuteHandler(nextLine.Id);
        }

        /// <summary>
        ///     或合并节点处理器
        /// </summary>
        /// <param name="node"></param>
        protected virtual void OrMergeNodeHandler(Node node)
        {
            if (node.OrClear)
                OrMergeNodeClearHandler(node);
            else
                OrMergeNodeNotClearHandler(node);
        }

        /// <summary>
        ///     或合并节点，清理兄弟节点
        /// </summary>
        /// <param name="node">合并节点</param>
        protected virtual void OrMergeNodeClearHandler(Node node)
        {
            var networkNodes = FlowChart.GetNetworkNodes(node).Where(x => x.Id != _startNode.Id);
            foreach (var item in networkNodes)
                if (Process.HistoryCurrentNodes.Any(x => x.Id == item.Id))
                    SaveNodeToResult(item, OperatingNodeType.BrotherFinish);
            SaveNodeToResult(node, OperatingNodeType.AutoFinish);
            var nextLine = FlowChart.Lines.First(x => x.TailNodeId == node.Id);
            ExecuteHandler(nextLine.Id);
        }

        /// <summary>
        ///     或合并节点，不清空兄弟节点
        /// </summary>
        /// <param name="node"></param>
        protected virtual void OrMergeNodeNotClearHandler(Node node)
        {
            if (Process.HistoryCurrentNodes.All(x => x.Id != node.Id))
            {
                SaveNodeToCurrentNodes(node, OperatingNodeType.OrFinish);
                var nextLine = FlowChart.Lines.First(x => x.TailNodeId == node.Id);
                ExecuteHandler(nextLine.Id);
            }

            var networkNodes = FlowChart.GetNetworkNodes(node)
                .Where(x => x.Id != _startNode.Id && x.Id != node.Id);
            foreach (var item in networkNodes)
                if (Process.HistoryCurrentNodes.Any(x => x.Id == item.Id))
                    return;
            SaveNodeToResult(node, OperatingNodeType.OrAllFinish);
        }

        /// <summary>
        ///     当有一个清空节点产生时，同时要清空父或非清空节点
        /// </summary>
        private void ClearOrFinishNode()
        {
            var orFinishNodes = Process.HistoryCurrentNodes
                .Where(x => x.OperatingNodeType == OperatingNodeType.OrFinish);
            foreach (var orFinishNode in orFinishNodes)
            {
                var canSave = true;
                var networkNodes = FlowChart.GetNetworkNodes(orFinishNode.SeparateNodeId)
                    .Where(x => x.Id != _startNode.Id && x.Id != orFinishNode.Id && x.NodeType == NodeType.General);
                foreach (var item in networkNodes)
                    if (Process.HistoryCurrentNodes.Any(x => x.Id == item.Id))
                    {
                        canSave = false;
                        break;
                    }

                if (canSave)
                    if (!Process.Result.Exists(x =>
                        x.Id == orFinishNode.Id && x.OperatingNodeType == OperatingNodeType.OrAllFinish))
                        SaveNodeToResult(FlowChart.Nodes.First(x => x.Id == orFinishNode.Id),
                            OperatingNodeType.OrAllFinish);
            }
        }

        /// <summary>
        ///     普通节点处理器
        /// </summary>
        /// <param name="node">普通节点</param>
        protected virtual void GeneralNodeHandler(Node node)
        {
            SaveNodeToCurrentNodes(node);
        }

        /// <summary>
        ///     保存结果节点到当前节点列表中
        /// </summary>
        /// <param name="node"></param>
        protected void SaveNodeToCurrentNodes(Node node)
        {
            SaveNodeToCurrentNodes(node, OperatingNodeType.Working);
        }

        /// <summary>
        ///     保存结果节点到当期节点列表中
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="type">操作类型</param>
        protected void SaveNodeToCurrentNodes(Node node, OperatingNodeType type)
        {
            if (Process.Current.Exists(x => x.Id == node.Id && x.OperatingNodeType == type)) return;
            Process.Current.Add(NodeToResultNode(node, type));
        }

        /// <summary>
        ///     保存结果节点到节点结果列表中
        /// </summary>
        /// <param name="node"></param>
        /// <param name="operatingNodeType"></param>
        protected void SaveNodeToResult(Node node, OperatingNodeType operatingNodeType)
        {
            Process.Result.Add(NodeToResultNode(node, operatingNodeType));
        }

        /// <summary>
        ///     保存连线到走过的线集合中
        /// </summary>
        /// <param name="line"></param>
        protected void SaveLineToPassedLine(Line line)
        {
            Process.PassedLines.Add(new PassedLine(line.Id, DateTime.Now));
        }

        /// <summary>
        ///     保存连线到走过的线集合中
        /// </summary>
        /// <param name="lineId"></param>
        protected void SaveLineToPassedLine(long lineId)
        {
            Process.PassedLines.Add(new PassedLine(lineId, DateTime.Now));
        }

        /// <summary>
        ///     Node 转NodeResult
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatingNodeType"></param>
        /// <returns></returns>
        protected ResultNode NodeToResultNode(Node input, OperatingNodeType operatingNodeType)
        {
            return new ResultNode
            {
                Id = input.Id,
                NodeType = input.NodeType,
                OperatingNodeType = operatingNodeType,
                SeparateNodeId = input.SeparateNodeId,
                OperatingTime = DateTime.Now,
                RuleId = input.RuleId
            };
        }
    }
}