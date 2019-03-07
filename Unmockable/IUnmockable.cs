using System;
using System.Linq.Expressions;

namespace Unmockable
{
    public interface IUnmockable<T>
    {
        TResult Execute<TResult>(Expression<Func<T, TResult>> m);
    }
}