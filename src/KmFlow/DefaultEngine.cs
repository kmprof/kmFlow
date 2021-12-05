namespace KmFlow
{
    /// <summary>
    ///     默认的流程引擎
    /// </summary>
    internal sealed class DefaultEngine : AbstractEngine
    {
        /// <summary>
        ///     执行
        /// </summary>
        /// <param name="lineId">连线Id</param>
        public override void Execute(long lineId)
        {
            ExecuteInner(lineId);
        }
    }
}