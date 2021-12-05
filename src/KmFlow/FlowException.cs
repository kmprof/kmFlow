using System;
using System.Collections.Generic;
using System.Text;

namespace KmFlow
{
    /// <summary>
    /// 流程异常
    /// </summary>
    public sealed class FlowException: Exception
    {
        public FlowException() : base()
        {

        }
        public FlowException(string message) :base(message)
        {

        }
        public FlowException(string message, Exception innerException):base(message,innerException)
        {

        }
    }
}
