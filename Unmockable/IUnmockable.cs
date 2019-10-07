using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable
{
    public interface IUnmockable<T>
    {
        TResult Execute<TResult>(Expression<Func<T, TResult>> m);
        void Execute(Expression<Action<T>> m);
        IUnmockable<TResult> Wrap<TResult>(Expression<Func<T, TResult>> m);
        Task<IUnmockable<TResult>> Wrap<TResult>(Expression<Func<T, Task<TResult>>> m);
    }
}