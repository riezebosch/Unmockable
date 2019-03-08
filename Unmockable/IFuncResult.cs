using System;

namespace Unmockable
{
    public interface IFuncResult<T, in TResult>
    {
        Intercept<T> Returns(TResult result);

        Intercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}