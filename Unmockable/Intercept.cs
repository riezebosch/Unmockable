using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable
{
    public class Intercept<T> : IUnmockable<T>
    {
        private readonly IDictionary<int, InterceptSetup<T>> _setups = new Dictionary<int, InterceptSetup<T>>();

        public TResult Execute<TResult>(Expression<Func<T, TResult>> m)
        {
            var key = m.ToKey();
            if (!_setups.TryGetValue(key, out var setup))
            {
                throw new NotSetupException(m.ToString());
            }

            setup.IsExecuted = true;
            return ((InterceptSetup<T, TResult>)setup).Result;
        }

        public InterceptSetup<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m)
        {
            var setup = new InterceptSetup<T, TResult>(this, m);
            _setups[m.ToKey()] = setup;

            return setup;
        }

        public void Verify()
        {
            var not = _setups.Values.Where(x => !x.IsExecuted);
            if (not.Any())
            {
                throw new NotExecutedException<T>(not);
            }
        }
    }
}