using System;

namespace Unmockable.Exceptions
{
    public class SetupNotFoundException : Exception
    {
        public SetupNotFoundException(string message) : base(message)
        {
        }
    }
}