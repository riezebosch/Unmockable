using System;

namespace Unmockable.Exceptions
{
    public class NotInstanceMethodCallException : Exception
    {
        public NotInstanceMethodCallException(string message) : base(message)
        {
        }
    }
}