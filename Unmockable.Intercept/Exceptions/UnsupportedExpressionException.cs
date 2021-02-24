using System;
using System.Linq.Expressions;

namespace Unmockable.Exceptions
{
    public class UnsupportedExpressionException : Exception
    {
        public UnsupportedExpressionException(LambdaExpression message):
            base(message.ToString())
        {
        }
    }
}