using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Result;
using Unmockable.Setup;
using Void = Unmockable.Result.Void;

namespace Unmockable
{
    public class Intercept<T> : IUnmockable<T>, IIntercept<T>
    {
        private readonly SetupCache _setups = new SetupCache();

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m) => 
            _setups.Store(new InterceptSetup<T, TResult>(this, m, new UninitializedResult<TResult>(m)));

        public IFuncResult<T, Task> Setup(Expression<Func<T, Task>> m) => 
            _setups.Store(new InterceptSetup<T, Task>(this, m, new AsyncActionResult(m)));

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, Task<TResult>>> m) => 
            _setups.Store(new InterceptSetupAsync<T, TResult>(this, m));

        public IActionResult<T> Setup(Expression<Action<T>> m) => 
            _setups.Store(new InterceptSetup<T, Void>(this, m, new ActionResult(m)));

        TResult IUnmockable<T>.Execute<TResult>(Expression<Func<T, TResult>> m) => 
            _setups.Load<TResult>(m).Execute();

        void IUnmockable<T>.Execute(Expression<Action<T>> m) => 
            _setups.Load<Void>(m).Execute();

        public void Verify() => 
            _setups.Verify();
    }
}
