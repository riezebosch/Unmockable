using System;
using Unmockable.Matchers;

namespace Unmockable.Exceptions
{
    public class OutOfResultsException : Exception
    {
        internal OutOfResultsException(IMemberMatcher expression):
            base(expression.ToString())
        {
        }
    }
}