using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Result;
using Unmockable.Setup;
using Void = Unmockable.Result.Void;

namespace Unmockable
{
    internal class Intercept<T> : IIntercept<T>
    {
        private readonly SetupCache _setups = new SetupCache();

        IFuncResult<T, TResult> ISetupFunc<T>.Setup<TResult>(Expression<Func<T, TResult>> m) => 
            _setups.Store(new Setup<T, TResult>(this, m, new UninitializedResult<TResult>(m)));
        IFuncResult<T, TResult> ISetupFunc<T>.Setup<TResult>(Expression<Func<T, Task<TResult>>> m) => 
            _setups.Store(new SetupAsync<T, TResult>(this, m));
        IActionResult<T> ISetupAction<T>.Setup(Expression<Func<T, Task>> m) => 
            _setups.Store(new Setup<T, Task>(this, m, new AsyncActionResult(m)));
        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => 
            _setups.Store(new Setup<T, Void>(this, m, new ActionResult(m)));

        void IIntercept<T>.Verify() => 
            _setups.Verify();

        TResult IUnmockable<T>.Execute<TResult>(Expression<Func<T, TResult>> m) => 
            _setups.Load<TResult>(m).Execute();

        void IUnmockable<T>.Execute(Expression<Action<T>> m) => 
            _setups.Load<Void>(m).Execute();
    }
}
