using System;
using System.Linq.Expressions;

namespace Unmockable.Exceptions
{
    public class OutOfResultsException : Exception
    {
        public OutOfResultsException(LambdaExpression expression) : base(expression.ToString())
        {
        }
    }
}