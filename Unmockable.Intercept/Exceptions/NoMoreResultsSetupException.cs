using System;

namespace Unmockable.Exceptions
{
    public class NoMoreResultsSetupException : Exception
    {
        public NoMoreResultsSetupException(string message) : base(message)
        {
        }
    }
}