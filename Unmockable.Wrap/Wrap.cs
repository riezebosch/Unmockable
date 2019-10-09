using System;
using System.Linq.Expressions;

namespace Unmockable
{
    public class Wrap<T> : IUnmockable<T>
    {
        private readonly T _item;

        public Wrap(T item) => _item = item;

        TResult IUnmockable<T>.Execute<TResult>(Expression<Func<T, TResult>> m) => m.Compile().Invoke(_item);

        void IUnmockable<T>.Execute(Expression<Action<T>> m) => m.Compile().Invoke(_item);
    }
}