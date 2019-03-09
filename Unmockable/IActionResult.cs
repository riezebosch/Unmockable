using System;

namespace Unmockable
{
    public interface IActionResult<T> : ISetup<T>
    {
        Intercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}