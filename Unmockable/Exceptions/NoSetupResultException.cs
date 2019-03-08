using System;

namespace Unmockable.Exceptions
{
    public class NoSetupResultException : Exception
    {
        public NoSetupResultException(string message) : base(message)
        {
        }
    }
}