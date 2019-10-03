using System;

namespace Unmockable.Setup
{
    public interface IFuncResult<T, in TResult>
    {
        IResult<T, TResult> Returns(TResult result);
        
        IResult<T, TResult> Throws<TException>() 
            where TException: Exception, new();
    }
}