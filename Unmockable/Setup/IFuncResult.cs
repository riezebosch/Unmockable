using System;

namespace Unmockable.Setup
{
    public interface IFuncResult<T, in TResult>
    {
        IIntercept<T> Returns(TResult result);
        
        IIntercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}