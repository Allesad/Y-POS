using System;

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

        public ServerRuntimeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
