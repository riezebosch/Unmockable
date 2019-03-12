using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable
{
    public class Intercept : IStatic
    {
        private readonly SetupCache _cache = new SetupCache();
        
        public IFuncResult<TResult> Setup<TResult>(Expression<Func<TResult>> m)
        {
            return _cache.ToCache(new StaticSetup<TResult>(this, m));
        }

        TResult IStatic.Execute<TResult>(Expression<Func<TResult>> m)
        {
            var setup = _cache.FromCache<StaticSetup<TResult>>(m);
            setup.Execute();
            
            return setup.Result;
        }

        public void Verify()
        {
            _cache.Verify();
        }
    }
    
    public class Intercept<T> : IUnmockable<T>, ISetup<T>
    {
        private readonly IDictionary<MethodMatcher, InterceptSetup<T>> _setups = new Dictionary<MethodMatcher, InterceptSetup<T>>();

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m)
        {
            return ToCache(new InterceptSetup<T, TResult>(this, m));
        }

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, Task<TResult>>> m)
        {
            return ToCache(new InterceptSetupAsync<T, TResult>(this, m));
        }
        
        public IActionResult<T> Setup(Expression<Action<T>> m)
        {
            return ToCache(new InterceptSetup<T>(this, m));
        }

        TResult IUnmockable<T>.Execute<TResult>(Expression<Func<T, TResult>> m)
        {
            var setup = (InterceptSetup<T, TResult>)FromCache(m);
            setup.Execute();
            
            return setup.Result;
        }
        
        Task<TResult> IUnmockable<T>.Execute<TResult>(Expression<Func<T, Task<TResult>>> m)
        {
            var setup = (InterceptSetupAsync<T, TResult>)FromCache(m);
            setup.Execute();
            
            return setup.Result;
        }

        void IUnmockable<T>.Execute(Expression<Action<T>> m)
        {
            var setup = FromCache(m);
            setup.Execute();
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

        private TItem ToCache<TItem>(TItem setup) where TItem: InterceptSetup<T>
        {
            _setups[setup.Expression.ToMatcher()] = setup;
            return setup;
        }

        private InterceptSetup<T> FromCache(LambdaExpression m)
        {
            var key = m.ToMatcher();
            if (!_setups.TryGetValue(key, out var setup))
            {
                throw new NoSetupException(key.ToString());
            }

            return setup;
        }
    }
}
