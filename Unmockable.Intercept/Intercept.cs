using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Matchers;
using Unmockable.Result;
using Unmockable.Setup;
using Void = Unmockable.Result.Void;

namespace Unmockable
{
    internal class Intercept<T> : IIntercept<T>
    {
        private readonly SetupCache _setups = new SetupCache();

        IFuncResult<T, TResult> ISetupFunc<T>.Setup<TResult>(Expression<Func<T, TResult>> m) => 
            _setups.Store(Uninitialized<TResult>(m.ToMatcher()));

        private Setup<T, TResult> Uninitialized<TResult>(IMemberMatcher matcher) => 
            new Setup<T, TResult>(this, matcher, new UninitializedResult<TResult>(matcher));

        IFuncResult<T, TResult> ISetupFunc<T>.Setup<TResult>(Expression<Func<T, Task<TResult>>> m) => 
            _setups.Store(new SetupAsync<T, TResult>(this, m.ToMatcher()));
        IActionResult<T> ISetupAction<T>.Setup(Expression<Func<T, Task>> m) => 
            _setups.Store(AsyncAction(m.ToMatcher()));

        private Setup<T, Task> AsyncAction(IMemberMatcher matcher) => 
            new Setup<T, Task>(this, matcher, new AsyncActionResult());

        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => 
            _setups.Store(Action(m.ToMatcher()));

        private Setup<T, Void> Action(IMemberMatcher matcher) => 
            new Setup<T, Void>(this, matcher, new ActionResult());

        void IIntercept<T>.Verify() => 
            _setups.Verify();

        TResult IUnmockable<T>.Execute<TResult>(Expression<Func<T, TResult>> m) => 
            _setups.Load<TResult>(m.ToMatcher()).Execute();

        void IUnmockable<T>.Execute(Expression<Action<T>> m) => 
            _setups.Load<Void>(m.ToMatcher()).Execute();
    }
}
