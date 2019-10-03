using System;

namespace Unmockable.Setup
{
    public interface IActionResult<T> : IIntercept<T>
    {
        IVoidResult<T> Throws<TException>() 
            where TException: Exception, new();
    }

    public interface IVoidResult<T> : IIntercept<T>
    {
        IVoidResult<T> ThenThrows<TException>() where TException : Exception, new();
    }
}