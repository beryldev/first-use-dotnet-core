using System;

namespace Wrhs.Core.Exceptions
{
    public class WrhsException : Exception
    {
        public WrhsException(string message) : base(message)
        {
        }

        public WrhsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}