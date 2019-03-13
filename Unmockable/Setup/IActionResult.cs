using System;

namespace Unmockable.Setup
{
    public interface IActionResult : IIntercept
    {
        IIntercept Throws<TException>() 
            where TException: Exception, new();
    }

    public interface IActionResult<T> : IIntercept<T>
    {
        IIntercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}