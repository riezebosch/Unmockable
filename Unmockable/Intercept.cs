using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable
{
    public class Intercept<T> : IUnmockable<T>, IStatic, ISetup<T>
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

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<TResult>> m)
        {
            var setup = new InterceptSetup<T, TResult>(this, m);
            _setups[m.ToMatcher()] = setup;

            return setup;
        }

        TResult IUnmockable<T>.Execute<TResult>(Expression<Func<T, TResult>> m)
        {
            return ((InterceptSetup<T, TResult>)Do(m)).Result;
        }
        
        Task<TResult> IUnmockable<T>.Execute<TResult>(Expression<Func<T, Task<TResult>>> m)
        {
            return ((InterceptSetupAsync<T, TResult>)Do(m)).Result;
        }

        void IUnmockable<T>.Execute(Expression<Action<T>> m)
        {
            Do(m);
        }

        TResult IStatic.Execute<TResult>(Expression<Func<TResult>> m)
        {
            return ((InterceptSetup<T, TResult>)Do(m)).Result;
        }

        public void Verify()
        {
            var not = _setups
                .Values
                .Where(x => !x.IsExecuted)
                .Select(x => x.Expression)
                .ToList();
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
