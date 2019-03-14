using System;

namespace Unmockable.Setup
{
    public interface IActionResult<T> : IIntercept<T>
    {
        IIntercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}