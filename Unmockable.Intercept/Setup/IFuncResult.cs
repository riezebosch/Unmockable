using System;

namespace Unmockable.Setup
{
    public interface IFuncResult<T, in TResult>
    {
        IResult<T, TResult> Returns(TResult result);
        
        IIntercept<T> Throws<TException>() 
            where TException: Exception, new();
    }

    public interface IResult<T, in TResult> : IIntercept<T>
    {
        IResult<T, TResult> Then(TResult result);
    }
}