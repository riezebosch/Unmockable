using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Result;
using Unmockable.Setup;

namespace Unmockable
{
    public class Intercept<T> : IUnmockable<T>, IIntercept<T>
    {
        private readonly SetupCache _setups = new SetupCache();

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m) => _setups.Store(new InterceptSetup<T, TResult>(this, m, new NoSetupResult<TResult>(m)));

        public IFuncResult<T, Task> Setup(Expression<Func<T, Task>> m) => _setups.Store(new InterceptSetup<T, Task>(this, m, new AsyncActionResult()));

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, Task<TResult>>> m) => _setups.Store(new InterceptSetupAsync<T, TResult>(this, m));

        public IActionResult<T> Setup(Expression<Action<T>> m) => _setups.Store(new InterceptSetup<T, Nothing>(this, m, new ActionResult()));

        TResult IUnmockable<T>.Execute<TResult>(Expression<Func<T, TResult>> m) => _setups.Load<TResult>(m).Execute();

        void IUnmockable<T>.Execute(Expression<Action<T>> m) => _setups.Load<Nothing>(m).Execute();

        IUnmockable<TResult> IUnmockable<T>.Wrap<TResult>(Expression<Func<T, TResult>> expression) =>
            _setups.Load<IUnmockable<TResult>>(expression).Execute();

        Task<IUnmockable<TResult>> IUnmockable<T>.Wrap<TResult>(Expression<Func<T, Task<TResult>>> m) => 
            _setups.Load<Task<IUnmockable<TResult>>>(m).Execute();

        public void Wrap<TResult>(Expression<Func<T, TResult>> expression, IUnmockable<TResult> with) =>
            _setups.Store(new Wrap<TResult>(expression, with));
        
        public void Wrap<TResult>(Expression<Func<T, Task<TResult>>> expression, IUnmockable<TResult> with) =>
            _setups.Store(new AsyncWrap<TResult>(expression, with));

        public void Verify() => _setups.Verify();
    }
}
