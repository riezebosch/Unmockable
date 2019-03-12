using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Setup;

namespace Unmockable
{
    public class Intercept : IStatic
    {
        private readonly SetupCache _cache = new SetupCache();
        
        public IFuncResult<TResult> Setup<TResult>(Expression<Func<TResult>> m)
        {
            return _cache.ToCache(new StaticSetup<TResult>(this, m));
        }

        public IActionResult Setup(Expression<Action> m)
        {
            return _cache.ToCache(new StaticSetup(this, m));
        }

        TResult IStatic.Execute<TResult>(Expression<Func<TResult>> m)
        {
            var setup = _cache.FromCache<StaticSetup<TResult>>(m);
            setup.Execute();
            
            return setup.Result;
        }

        void IStatic.Execute(Expression<Action> m)
        {
            _cache.FromCache<StaticSetup>(m).Execute();
        }

        public void Verify()
        {
            _cache.Verify();
        }
    }
    
    public class Intercept<T> : IUnmockable<T>, ISetup<T>
    {
        private readonly SetupCache _setups = new SetupCache();

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m)
        {
            return _setups.ToCache(new InterceptSetup<T, TResult>(this, m));
        }

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, Task<TResult>>> m)
        {
            return _setups.ToCache(new InterceptSetupAsync<T, TResult>(this, m));
        }
        
        public IActionResult<T> Setup(Expression<Action<T>> m)
        {
            return _setups.ToCache(new InterceptSetup<T>(this, m));
        }

        TResult IUnmockable<T>.Execute<TResult>(Expression<Func<T, TResult>> m)
        {
            var setup = _setups.FromCache<InterceptSetup<T, TResult>>(m);
            setup.Execute();
            
            return setup.Result;
        }
        
        Task<TResult> IUnmockable<T>.Execute<TResult>(Expression<Func<T, Task<TResult>>> m)
        {
            var setup = _setups.FromCache<InterceptSetupAsync<T, TResult>>(m);
            setup.Execute();
            
            return setup.Result;
        }

        void IUnmockable<T>.Execute(Expression<Action<T>> m)
        {
            var setup = _setups.FromCache<InterceptSetup<T>>(m);
            setup.Execute();
        }

        public void Verify()
        {
           _setups.Verify();
        }
    }
}
