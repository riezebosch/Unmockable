using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Result;

namespace Unmockable.Setup
{
    internal class InterceptSetup<T, TResult> : 
        ISetup<TResult>, 
        IFuncResult<T, TResult>, 
        IResult<T, TResult>,
        IVoidResult<T>,
        IActionResult<T>
    {
        protected IResult<TResult> Results { get; set; } 

        private readonly IIntercept<T> _intercept;

        public LambdaExpression Expression { get; }
        
        public bool IsExecuted => Results.IsDone;

        public InterceptSetup(IIntercept<T> intercept, LambdaExpression expression, IResult<TResult> result)
        {
            Results = result;
            Expression = expression;
            _intercept = intercept;
        }

        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => _intercept.Setup(m);

        IFuncResult<T, TNewResult> ISetupFunc<T>.Setup<TNewResult>(Expression<Func<T, TNewResult>> m) => _intercept.Setup(m);
        
        IFuncResult<T, TResultNew> ISetupFuncAsync<T>.Setup<TResultNew>(Expression<Func<T, Task<TResultNew>>> m) => _intercept.Setup(m);

        IResult<T, TResult> IFuncResult<T, TResult>.Throws<TException>() => Add(new ExceptionResult<TResult, TException>(Expression));

        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>() => Add(new ExceptionResult<TResult,TException>(Expression));
        
        IVoidResult<T> IActionResult<T>.Throws<TException>() => Add(new ExceptionResult<TResult,TException>(Expression));

        IVoidResult<T> IVoidResult<T>.ThenThrows<TException>() => Add(new ExceptionResult<TResult,TException>(Expression));

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result) => Add(new FuncResult<TResult>(result, Expression));
        
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result) => Add(new FuncResult<TResult>(result, Expression));

        public TResult Execute() => Results.Result;
        
        private InterceptSetup<T, TResult>  Add(IResult<TResult> result)
        {
            Results = Results.Add(result);
            return this;
        }
    }
}
