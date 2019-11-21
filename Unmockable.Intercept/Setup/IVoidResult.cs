using System;

namespace Unmockable.Setup
{
    public interface IVoidResult<T> : IIntercept<T>
    {
        IVoidResult<T> ThenThrows<TException>() 
            where TException : Exception, new();
    }
}