using System;

namespace Unmockable.Setup
{
    public interface IResult<T, in TResult> : IIntercept<T>
    {
        IResult<T, TResult> Then(TResult result);
        IResult<T, TResult> ThenThrows<TException>() where TException : Exception, new();
    }
}