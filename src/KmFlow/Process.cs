using System;
using System.Collections.Generic;

namespace KmFlow
{
    /// <summary>
    ///     流程
    /// </summary>
    public class Process
    {
        /// <summary>
        ///     流程图
        /// </summary>
        public FlowChart FlowChart { get; }

        /// <summary>
        ///     流程引擎
        /// </summary>
        public AbstractEngine Engine { get; }

        /// <summary>
        ///     反向待清空的分流节点
        /// </summary>
        public ReverseClearNode[] ReverseClearNodes { get; private set; }


        /// <summary>
        ///     历史当前节点
        /// </summary>
        public ResultNode[] HistoryCurrentNodes { get; set; }

        /// <summary>
        ///     产生的结果
        /// </summary>
        public List<ResultNode> Result { get; internal set; }

        /// <summary>
        ///     新产生的当前节点
        /// </summary>
        public List<ResultNode> Current { get; internal set; }

        /// <summary>
        ///     走过的线
        /// </summary>
        public List<PassedLine> PassedLines { get; internal set; }

        /// <summary>
        ///     构造方法
        /// </summary>
        /// <param name="flowChart"></param>
        /// <param name="engine"></param>
        public Process(FlowChart flowChart, AbstractEngine engine)
        {
            FlowChart = flowChart;
            Engine = engine;
        }

        /// <summary>
        ///     构造方法
        /// </summary>
        /// <param name="flowChart"></param>
        public Process(FlowChart flowChart) : this(flowChart, new DefaultEngine())
        {
        }

        /// <summary>
        ///     加载所需数据
        /// </summary>
        /// <param name="reverseClearNodes">反向待清空的分流节点</param>
        public void Load(ReverseClearNode[] reverseClearNodes)
        {
            ReverseClearNodes = reverseClearNodes;
            Engine.Input(FlowChart, this);
        }

        /// <summary>
        ///     加载所需数据
        /// </summary>
        public void Load()
        {
            Load(Array.Empty<ReverseClearNode>());
        }

        /// <summary>
        ///     初始化返回的结果
        /// </summary>
        private void InitResult()
        {
            Result = new List<ResultNode>();
            Current = new List<ResultNode>();
            PassedLines = new List<PassedLine>();
        }

        /// <summary>
        ///     产生下一组节点
        /// </summary>
        /// <param name="currentNodes">历史当前节点</param>
        /// <param name="lineId">操作的连线</param>
        public void Run(List<ResultNode> currentNodes, long lineId)
        {
            HistoryCurrentNodes = currentNodes.ToArray();
            InitResult();
            Engine.Execute(lineId);
        }

        /// <summary>
        ///     产生下一组节点
        /// </summary>
        /// <param name="lineId">操作的连线</param>
        public void Run(long lineId)
        {
            Run(new List<ResultNode>(), lineId);
        }
    }
}