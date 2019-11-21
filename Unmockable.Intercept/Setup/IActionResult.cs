using System;

namespace Unmockable.Setup
{
    public interface IActionResult<T> : IIntercept<T>
    {
        IVoidResult<T> Throws<TException>() 
            where TException: Exception, new();
    }
}