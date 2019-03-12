using System;

namespace Unmockable.Setup
{
    public interface IActionResult
    {
    
    }
    
    public interface IActionResult<T> : ISetup<T>
    {
        Intercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}