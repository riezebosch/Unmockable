using System;
using Unmockable.Matchers;

namespace Unmockable.Exceptions
{
    public class SetupNotFoundException : Exception
    {
        internal SetupNotFoundException(IMemberMatcher message):
            base(message.ToString())
        {
        }
    }
}