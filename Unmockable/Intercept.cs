using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable
{
    public class Intercept<T> : IUnmockable<T>
    {
        private readonly IDictionary<int, object> _setups = new Dictionary<int, object>();

        public TResult Execute<TResult>(Expression<Func<T, TResult>> m)
        {
            var key = m.ToKey();
            if (_setups.TryGetValue(key, out var setup))
            {
                return ((InterceptSetup<T, TResult>)setup).Result;
            }

            throw new NotSetupException(m.ToString());
        }

        public InterceptSetup<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m)
        {
            var setup = new InterceptSetup<T, TResult>(this);
            _setups[m.ToKey()] = setup;

            return setup;
        }
    }
}