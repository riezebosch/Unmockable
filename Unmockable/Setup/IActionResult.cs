using System;

namespace Unmockable.Setup
{
    public interface IActionResult : ISetupAction
    {
        Intercept Throws<TException>() 
            where TException: Exception, new();
    }

    public interface IActionResult<T> : ISetupFunc<T>, ISetupAction<T>
    {
        Intercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}