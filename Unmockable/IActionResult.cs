using System;

namespace Unmockable
{
    public interface IActionResult<T> : IIntercept<T>
    {
        Intercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}