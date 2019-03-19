using System;

namespace Unmockable.Exceptions
{
    public class NoResultsSetupException : Exception
    {
        public NoResultsSetupException(string message) : base(message)
        {
        }
    }
}