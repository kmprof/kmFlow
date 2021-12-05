namespace KmFlow.AutoEngines
{
    /// <summary>
    ///     自动引擎上下文
    /// </summary>
    public abstract class AutoEngineContext
    {
        public abstract IRuleValidator GetValidator(long ruleId);
    }
}