using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Setup;

namespace Unmockable
{
    public class Intercept<T> : IUnmockable<T>, IIntercept<T>
    {
        private readonly SetupCache _setups = new SetupCache();

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m) => _setups.Store(new InterceptSetup<T, TResult>(this, m));

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, Task<TResult>>> m) => _setups.Store(new InterceptSetupAsync<T, TResult>(this, m));

        public IActionResult<T> Setup(Expression<Action<T>> m) => _setups.Store(new InterceptSetup<T, Unit>(this, m));

        TResult IUnmockable<T>.Execute<TResult>(Expression<Func<T, TResult>> m) => _setups.Load<TResult>(m).Execute();

        Task<TResult> IUnmockable<T>.Execute<TResult>(Expression<Func<T, Task<TResult>>> m) => _setups.Load<Task<TResult>>(m).Execute();

        void IUnmockable<T>.Execute(Expression<Action<T>> m) => _setups.Load<Unit>(m).Execute();

        public void Verify() => _setups.Verify();
    }
}
