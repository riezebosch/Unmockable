using System;

namespace Unmockable.Exceptions
{
    public class NoResultConfiguredException : Exception
    {
        public NoResultConfiguredException(string message) : base(message)
        {
        }
    }
}