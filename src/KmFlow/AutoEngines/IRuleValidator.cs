namespace KmFlow.AutoEngines
{
    /// <summary>
    ///     规则校验器
    /// </summary>
    public interface IRuleValidator
    {
        /// <summary>
        ///     验证规则
        /// </summary>
        /// <returns>
        ///     是否检验规则
        /// </returns>
        bool Validate();

        /// <summary>
        ///     获取验证通过时使用的连
        /// </summary>
        /// <returns>
        ///     如果只有一条线，直接返回string.Empty
        /// </returns>
        string GetLineCode();
    }
}