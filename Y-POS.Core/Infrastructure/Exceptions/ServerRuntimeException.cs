using System;
using System.ServiceModel;

namespace Y_POS.Core.Infrastructure.Exceptions
{
    public sealed class ServerRuntimeException : Exception
    {
        public ServerRuntimeException()
        {
        }

        public ServerRuntimeException(string message) : base(message)
        {
        }

        public ServerRuntimeException(string message, ExceptionDetail details) : base(message)
        {
            Details = details;
        }

        public ServerRuntimeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ExceptionDetail Details { get; }

        public override string StackTrace => Details != null ? Details.StackTrace : base.StackTrace;
    }
}
