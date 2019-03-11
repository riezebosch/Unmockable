using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable
{
    public class Wrap<T> : IUnmockable<T>
    {
        private readonly T _item;
        private readonly Dictionary<int, object> _cache = new Dictionary<int, object>();

        public Wrap(T item)
        {
            _item = item;
        }

        public TResult Execute<TResult>(Expression<Func<T, TResult>> m)
        {
             return Methods<Func<T, TResult>>(m).Invoke(_item);
        }

        public Task<TResult> Execute<TResult>(Expression<Func<T, Task<TResult>>> m)
        {
            return Methods<Func<T, Task<TResult>>>(m).Invoke(_item);
        }

        public void Execute(Expression<Action<T>> m)
        {
            Methods<Action<T>>(m).Invoke(_item);
        }

        private TMethod Methods<TMethod>(LambdaExpression m)
        {
            var key = m.ToKey();
            if (_cache.TryGetValue(key, out var method))
            {
                return (TMethod) method;
            }

            method = m.Compile();
            return (TMethod)(_cache[key] = method);
        }
    }
}