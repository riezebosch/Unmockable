using System;

namespace Unmockable.Result
{
    internal interface IResult<T>
    {
        T Result { get; }
        bool IsDone { get; }
        IResult<T> Add(IResult<T> next);
    }
}