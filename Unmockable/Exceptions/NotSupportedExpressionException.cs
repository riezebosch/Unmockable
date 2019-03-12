using System;

namespace Unmockable.Exceptions
{
    public class NotSupportedExpressionException : Exception
    {
        public NotSupportedExpressionException(string message) : base(message)
        {
        }
    }
}