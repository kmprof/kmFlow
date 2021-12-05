using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KmFlow.AutoEngines
{
    /// <summary>
    ///     自动化流程引擎
    /// </summary>
    public class AutoEngine : AbstractEngine
    {
        private readonly AutoEngineContext _context;
        /// <summary>
        ///     新产生的当前节点
        /// </summary>
        public List<ResultNode> Current { get; internal set; }
        public AutoEngine([NotNull] AutoEngineContext context)
        {
            _context = context;
        }

        public override void Execute(long lineId)
        {
            Current ??= new List<ResultNode>();
            ExecuteInner(lineId);
            while (true)
            {
                foreach (var item in Process.Current)
                {
                    if (!Current.Exists(x => x.Id == item.Id))
                    {
                        Current.Add(item);
                    }
                }
                UpdateHistoryCurrentNodes();
                var node = Process.HistoryCurrentNodes.FirstOrDefault(x => x.NodeType == NodeType.General);
                if(node==null || node.RuleId==0)
                    break;
                var ruleValidator = _context.GetValidator(node.RuleId);
                if (ruleValidator?.Validate()==true)
                {
                    var lineCode = ruleValidator.GetLineCode();
                    if (string.IsNullOrEmpty(lineCode))
                    {
                        if (FlowChart.Lines.Count(x => x.TailNodeId == node.Id) == 1)
                        {
                            var line = FlowChart.Lines.First(x => x.TailNodeId == node.Id);
                            ExecuteInner(line.Id);
                        }
                        else
                        {
                            var temp = FlowChart.Nodes.First(x => x.Id == node.Id);
                            throw new FlowException($"节点有【{temp.Name}】多条出线,规则验证必须返回编码");
                        }
                    }
                    else
                    {
                        var line = FlowChart.Lines.FirstOrDefault(x => x.TailNodeId == node.Id && x.Code == lineCode);
                        if (line == null)
                        {
                            var temp = FlowChart.Nodes.First(x => x.Id == node.Id);
                            throw new FlowException($"节点【{temp.Name}】没有编码为【{lineCode}】的出线");
                        }
                        ExecuteInner(line.Id);
                    }
                }
                else
                {
                    break;
                }
            }

            Process.Current = Current;
        }
        /// <summary>
        /// 更新历史当前节点
        /// </summary>
        private void UpdateHistoryCurrentNodes()
        {
            var result=
                Process.HistoryCurrentNodes.Where(item => !Process.Result.Exists(x => x.Id == item.Id)).ToList();

            result.AddRange(Process.Current.Where(item => !Process.Result.Exists(x => x.Id == item.Id)));
            Process.HistoryCurrentNodes = result.ToArray();

        }
    }
}