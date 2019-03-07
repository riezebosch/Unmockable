using System;

namespace Unmockable.Exceptions
{
    public class NotSetupException : Exception
    {
        public NotSetupException(string message) : base(message)
        {
        }
    }
}