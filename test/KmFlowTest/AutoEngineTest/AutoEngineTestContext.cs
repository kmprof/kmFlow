using System.Collections.Generic;
using KmFlow.AutoEngines;

namespace KmFlowTest.AutoEngineTest
{
    /// <summary>
    ///     自动引擎的上下文
    /// </summary>
    public class AutoEngineTestContext : AutoEngineContext
    {
        private readonly TestData _testData;

        public AutoEngineTestContext(TestData testData)
        {
            _testData = testData;
        }

        /// <summary>
        ///     获取一个规则校验器
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public override IRuleValidator GetValidator(long ruleId)
        {
            return ruleId switch
            {
                2 => new RuleValidator1(_testData),
                3 => new RuleValidator2(_testData),
                _ => null
            };
        }
    }

    /// <summary>
    ///     规则校验器1
    /// </summary>
    public class RuleValidator1 : IRuleValidator
    {
        private readonly TestData _testData;

        public RuleValidator1(TestData testData)
        {
            _testData = testData;
        }

        public bool Validate()
        {
            _testData.Record.Add("规则1已执行");
            return true;
        }

        public string GetLineCode()
        {
            return string.Empty;
        }
    }

    /// <summary>
    ///     规则校验器2
    /// </summary>
    public class RuleValidator2 : IRuleValidator
    {
        private string _code;
        private readonly TestData _testData;

        public RuleValidator2(TestData testData)
        {
            _testData = testData;
        }

        public bool Validate()
        {
            switch (_testData.Value)
            {
                case "A":
                    _code = "005";
                    break;
                case "B":
                    _code = "003";
                    break;
                default:
                    return false;
            }

            _testData.Record.Add("规则2已执行");
            return true;
        }

        public string GetLineCode()
        {
            return _code;
        }
    }

    public class TestData
    {
        /// <summary>
        ///     模拟数据
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     记录
        /// </summary>
        public List<string> Record { get; set; }
    }
}