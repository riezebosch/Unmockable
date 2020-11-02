using System;
using System.Linq.Expressions;

namespace Unmockable
{
    /// <summary>
    /// Inject this interface and provide an Interceptor from an unit test and a Wrap at runtime.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnmockable<T>
    {
        /// <summary>
        /// Execute a method on the wrapped object that returns a result. 
        /// </summary>
        TResult Execute<TResult>(Expression<Func<T, TResult>> m);

        /// <summary>
        /// Execute a method on the wrapped object that returns no result. 
        /// </summary>
        void Execute(Expression<Action<T>> m);
    }
}