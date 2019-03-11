using System;
using System.Linq.Expressions;

namespace Unmockable
{
    public interface IStatic
    {
        TResult Execute<TResult>(Expression<Func<TResult>> m);
    }
}