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

        public InterceptSetup<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m)
        {
            var setup = new InterceptSetup<T, TResult>(this, m);
            _setups[m.ToKey()] = setup;

            return setup;
        }
        
        public InterceptSetup<T> Setup(Expression<Action<T>> m)
        {
            var setup = new InterceptSetup<T>(this, m);
            _setups[m.ToKey()] = setup;

            return setup;
        }

        public TResult Execute<TResult>(Expression<Func<T, TResult>> m)
        {
            return ((InterceptSetup<T, TResult>)Do(m)).Result;
        }

        public void Execute(Expression<Action<T>> m)
        {
            Do(m);
        }

        public void Verify()
        {
            var not = _setups
                .Values
                .Where(x => !x.IsExecuted)
                .Select(x => x.Expression);
            if (not.Any())
            {
                throw new NotExecutedException(not);
            }
        }

        private InterceptSetup<T> Do(LambdaExpression m)
        {
            var key = m.ToKey();
            if (!_setups.TryGetValue(key, out var setup))
            {
                throw new NotSetupException(m.ToString());
            }

            return setup.Execute();
        }
    }
}
