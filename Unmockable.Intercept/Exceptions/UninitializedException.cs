using System;
using Unmockable.Matchers;

namespace Unmockable.Exceptions
{
    public class UninitializedException : Exception
    {
        internal UninitializedException(IMemberMatcher message):
            base(message.ToString())
        {
        }
    }
}