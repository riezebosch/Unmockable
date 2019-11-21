using System;

namespace Unmockable.Exceptions
{
    public class UninitializedFuncException : Exception
    {
        public UninitializedFuncException(string message) : base(message)
        {
        }
    }
}