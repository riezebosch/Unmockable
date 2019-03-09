using System;
using System.Linq.Expressions;

namespace Unmockable
{
    public interface ISetup<T>
    {
        IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m);
    }
}