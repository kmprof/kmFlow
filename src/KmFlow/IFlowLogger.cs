namespace KmFlow
{
    /// <summary>
    ///     日志接口
    /// </summary>
    public interface IFlowLogger
    {
        /// <summary>
        ///     调试信息
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        ///     提示信息
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        ///     错误信息
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);
    }
}