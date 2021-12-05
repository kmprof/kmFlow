using System.Collections.Generic;
using System.Linq;

namespace KmFlow
{
    /// <summary>
    ///     流程图
    /// </summary>
    public class FlowChart
    {
        /// <summary>
        ///     流程中所有的节点
        /// </summary>
        public Node[] Nodes { get; }

        /// <summary>
        ///     流程中所有的连线
        /// </summary>
        public Line[] Lines { get; }

        /// <summary>
        ///     构造方法
        /// </summary>
        /// <param name="nodes">流程图所有的节点</param>
        /// <param name="lines">流程图所有的连线</param>
        public FlowChart(Node[] nodes, Line[] lines)
        {
            Nodes = nodes;
            Lines = lines;
        }

        /// <summary>
        ///     头部节点
        /// </summary>
        public Node GetLineHeadNode(long lineId)
        {
            var line = Lines.First(x => x.Id == lineId);
            return Nodes.First(x => x.Id == line.HeadNodeId);
        }

        /// <summary>
        ///     尾部节点
        /// </summary>
        public Node GetLineTailNode(long lineId)
        {
            var line = Lines.First(x => x.Id == lineId);
            return Nodes.First(x => x.Id == line.TailNodeId);
        }

        /// <summary>
        ///     获取 NetworkNodes
        ///     NetworkNode(网里的节点)是指：从分流节点开始到对应的合并节点之间所有的节点
        /// </summary>
        /// <param name="inputNode">分流节点</param>
        /// <param name="result">网里的节点</param>
        /// <returns></returns>
        private void GetNetworkNodes(Node inputNode, List<Node> result)
        {
            if (inputNode.NodeType != NodeType.Separate)
                throw new FlowException($"{nameof(inputNode)}必须为分流节点");
            GetNetworkNodes(inputNode.Id, result);
        }

        /// <summary>
        ///     获取 NetworkNodes
        ///     NetworkNode(网里的节点)是指：从分流节点开始到对应的合并节点之间所有的节点
        /// </summary>
        /// <param name="separateNodeId">分流节点Id</param>
        /// <param name="result">网里的节点</param>
        /// <returns></returns>
        private void GetNetworkNodes(long separateNodeId, List<Node> result)
        {
            var notSeparateNodes = Nodes.Where(x => x.SeparateNodeId == separateNodeId);
            result.AddRange(notSeparateNodes);
            var subSeparateNodes =
                Nodes.Where(x => x.SeparateNodeId == separateNodeId && x.NodeType == NodeType.Separate);
            foreach (var item in subSeparateNodes) GetNetworkNodes(item, result);
        }

        /// <summary>
        ///     获取一个节点所在的网中的所有的节点
        /// </summary>
        /// <param name="inputNode"></param>
        /// <returns></returns>
        public List<Node> GetNetworkNodes(Node inputNode)
        {
            var separateNode = GetNodeSeparateNode(inputNode);
            var result = new List<Node>();
            if (separateNode != null)
                GetNetworkNodes(separateNode, result);
            return result;
        }

        /// <summary>
        ///     获取一个分流节点所在的网中的所有的节点
        /// </summary>
        /// <param name="separateNodeId"></param>
        /// <returns></returns>
        public List<Node> GetNetworkNodes(long separateNodeId)
        {
            var result = new List<Node>();
            GetNetworkNodes(separateNodeId, result);
            return result;
        }

        /// <summary>
        ///     获取一个节点的分流节点
        /// </summary>
        /// <param name="inputNode"></param>
        /// <returns></returns>
        public Node GetNodeSeparateNode(Node inputNode)
        {
            return Nodes.FirstOrDefault(x => x.Id == inputNode.SeparateNodeId);
        }
    }
}