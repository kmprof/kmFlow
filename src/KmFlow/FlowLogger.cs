using System;

namespace KmFlow
{
    /// <summary>
    ///     流程中使用的日志
    /// </summary>
    public static class FlowLogger
    {
        /// <summary>
        ///     日志记录器
        /// </summary>
        private static IFlowLogger _flowLogger = new DefaultFlowLogger();

        /// <summary>
        ///     设置日志记录器
        /// </summary>
        /// <param name="logger"></param>
        public static void SetLogger(IFlowLogger logger)
        {
            _flowLogger = logger;
        }

        /// <summary>
        ///     记录调试信息
        /// </summary>
        /// <param name="message"></param>
        internal static void Debug(string message)
        {
            _flowLogger.Debug(message);
        }

        /// <summary>
        ///     记录错误信息
        /// </summary>
        /// <param name="message"></param>
        internal static void Error(string message)
        {
            _flowLogger.Error(message);
        }

        /// <summary>
        ///     记录基本信息
        /// </summary>
        /// <param name="message"></param>
        internal static void Info(string message)
        {
            _flowLogger.Info(message);
        }
    }

    /// <summary>
    ///     默认的日志记录器
    /// </summary>
    internal class DefaultFlowLogger : IFlowLogger
    {
        public void Debug(string message)
        {
            Console.WriteLine($"log debug [{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}] :{message}");
        }

        public void Error(string message)
        {
            Console.WriteLine($"log error [{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}] :{message}");
        }

        public void Info(string message)
        {
            Console.WriteLine($"log info [{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}] :{message}");
        }
    }
}