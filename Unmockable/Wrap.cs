using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable
{
    public class Wrap<T> : IUnmockable<T>
    {
        private readonly T _item;
        private readonly MethodCache _cache = new MethodCache();

        public Wrap(T item)
        {
            _item = item;
        }

        public TResult Execute<TResult>(Expression<Func<T, TResult>> m)
        {
            return _cache.Methods<Func<T, TResult>>(m).Invoke(_item);
        }

        public Task<TResult> Execute<TResult>(Expression<Func<T, Task<TResult>>> m)
        {
            return _cache.Methods<Func<T, Task<TResult>>>(m).Invoke(_item);
        }

        public void Execute(Expression<Action<T>> m)
        {
            _cache.Methods<Action<T>>(m).Invoke(_item);
        }
    }
}