using System;

namespace Unmockable.Exceptions
{
    public class NoSetupException : Exception
    {
        public NoSetupException(string message) : base(message)
        {
        }
    }
}