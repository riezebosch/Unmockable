using System;

namespace Unmockable
{
    public interface IFuncResult<in TResult>
    {
        Intercept Returns(TResult result);
        
        Intercept Throws<TException>() 
            where TException: Exception, new();
    }

    public interface IFuncResult<T, in TResult>
    {
        Intercept<T> Returns(TResult result);
        
        Intercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}