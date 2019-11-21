using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Result;

namespace Unmockable.Setup
{
    internal class Setup<T, TResult> : 
        ISetup<TResult>, 
        IFuncResult<T, TResult>, 
        IResult<T, TResult>,
        IVoidResult<T>,
        IActionResult<T>
    {
        protected IResult<TResult> Current { get; set; } 

        private readonly IIntercept<T> _parent;

        public LambdaExpression Expression { get; }
        
        public bool IsExecuted => Current.IsDone;

        public Setup(IIntercept<T> parent, LambdaExpression expression, IResult<TResult> result)
        {
            _parent = parent;
            Expression = expression;
            Current = result;
        }
        IFuncResult<T, TNew> ISetupFunc<T>.Setup<TNew>(Expression<Func<T, TNew>> m) => 
            _parent.Setup(m);
        IFuncResult<T, TNew> ISetupFunc<T>.Setup<TNew>(Expression<Func<T, Task<TNew>>> m) => 
            _parent.Setup(m);

        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => 
            _parent.Setup(m);
        IActionResult<T> ISetupAction<T>.Setup(Expression<Func<T, Task>> m) => 
            _parent.Setup(m);

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result) => 
            NewResult(new FuncResult<TResult>(result, Expression));
        IResult<T, TResult> IFuncResult<T, TResult>.Throws<TException>() => 
            NewResult(new ExceptionResult<TResult, TException>(Expression));
        
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result) => 
            NewResult(new FuncResult<TResult>(result, Expression));
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>() => 
            NewResult(new ExceptionResult<TResult,TException>(Expression));
        
        IVoidResult<T> IActionResult<T>.Throws<TException>() => 
            NewResult(new ExceptionResult<TResult,TException>(Expression));

        IVoidResult<T> IVoidResult<T>.ThenThrows<TException>() => 
            NewResult(new ExceptionResult<TResult,TException>(Expression));

        TResult ISetup<TResult>.Execute() => 
            Current.Result;
        
        void IIntercept<T>.Verify() => 
            _parent.Verify();
        
        TNew IUnmockable<T>.Execute<TNew>(Expression<Func<T, TNew>> m) => 
            _parent.Execute(m);
        void IUnmockable<T>.Execute(Expression<Action<T>> m) => 
            _parent.Execute(m);

        public override string ToString() => $"{Expression}: {Current}";

        private Setup<T, TResult>  NewResult(IResult<TResult> result)
        {
            Current = Current.NewResult(result);
            return this;
        }
    }
}
