using KmFlow;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KmFlowTest
{

    internal class TestCommon
    {
        /// <summary>
        ///     创建只包含Id的节点集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<Node> CreateIdNodes(long[] input)
        {
            var result = new List<Node>();
            foreach (var item in input) result.Add(new Node { Id = item });
            return result;
        }
    }

    internal class ResultNodeEqualityComparer : IEqualityComparer<ResultNode>
    {
        public bool Equals([AllowNull] ResultNode x, [AllowNull] ResultNode y)
        {
            return x.Id == y.Id && x.OperatingNodeType == y.OperatingNodeType;
        }

        public int GetHashCode([DisallowNull] ResultNode obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class LineEqualityComparer : IEqualityComparer<Line>
    {
        public bool Equals(Line x, Line y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Line obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class PassedLineEqualityComparer : IEqualityComparer<PassedLine>
    {
        public bool Equals([AllowNull] PassedLine x, [AllowNull] PassedLine y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] PassedLine obj)
        {
            return obj.Id.GetHashCode();
        }
    }
    public class NodeWithSeparateNodeEqualityComparer : IEqualityComparer<Node>
    {
        public bool Equals([AllowNull] Node x, [AllowNull] Node y)
        {
            return x.Id == y.Id && x.SeparateNodeId == y.SeparateNodeId;
        }

        public int GetHashCode([DisallowNull] Node obj)
        {
            return obj.Id.GetHashCode();
        }
    }
    public class ReverseClearNodeComparer : IEqualityComparer<ReverseClearNode>
    {
        public bool Equals(ReverseClearNode x, ReverseClearNode y)
        {
            return x.LineId == y.LineId &&x.NodeId==y.NodeId;
        }

        public int GetHashCode(ReverseClearNode obj)
        {
            return obj.LineId.GetHashCode();
        }
    }
}