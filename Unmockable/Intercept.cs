using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;

namespace Unmockable
{
    public class Intercept<T> : IUnmockable<T>, ISetup<T>
    {
        private readonly IDictionary<MethodMatcher, InterceptSetup<T>> _setups = new Dictionary<MethodMatcher, InterceptSetup<T>>();

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m)
        {
            var setup = new InterceptSetup<T, TResult>(this, m);
            _setups[m.ToMatcher()] = setup;

            return setup;
        }

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, Task<TResult>>> m)
        {
            var setup = new InterceptSetupAsync<T, TResult>(this, m);
            _setups[m.ToMatcher()] = setup;

            return setup;
        }
        
        public IActionResult<T> Setup(Expression<Action<T>> m)
        {
            var setup = new InterceptSetup<T>(this, m);
            _setups[m.ToMatcher()] = setup;

            return setup;
        }

        public TResult Execute<TResult>(Expression<Func<T, TResult>> m)
        {
            return ((InterceptSetup<T, TResult>)Do(m)).Result;
        }
        
        public Task<TResult> Execute<TResult>(Expression<Func<T, Task<TResult>>> m)
        {
            return ((InterceptSetupAsync<T, TResult>)Do(m)).Result;
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
            var key = m.ToMatcher();
            if (!_setups.TryGetValue(key, out var setup))
            {
                throw new NoSetupException(key.ToString());
            }

            return setup.Execute();
        }
    }
}
