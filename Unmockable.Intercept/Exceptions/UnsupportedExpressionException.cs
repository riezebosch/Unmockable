using System;

namespace Unmockable.Exceptions
{
    public class UnsupportedExpressionException : Exception
    {
        public UnsupportedExpressionException(string message) : base(message)
        {
        }
    }
}