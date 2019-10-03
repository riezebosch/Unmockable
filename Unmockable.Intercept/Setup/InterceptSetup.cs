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

        public InterceptSetup(IIntercept<T> intercept, LambdaExpression expression)
        {
            Results = typeof(TResult) == typeof(Task) ? (IResult<TResult>)new AsyncActionResult() :
                typeof(TResult) == typeof(Nothing) ? (IResult<TResult>)new ActionResult() :
                new NoSetupResult<TResult>(expression);
            Expression = expression;
            _intercept = intercept;
        }

        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => _intercept.Setup(m);

        IFuncResult<T, TNewResult> ISetupFunc<T>.Setup<TNewResult>(Expression<Func<T, TNewResult>> m) => _intercept.Setup(m);
        
        IFuncResult<T, TResultNew> ISetupFuncAsync<T>.Setup<TResultNew>(Expression<Func<T, Task<TResultNew>>> m) => _intercept.Setup(m);

        IResult<T, TResult> IFuncResult<T, TResult>.Throws<TException>()
        {
            Results = Results.Add(new ExceptionResult<TResult,TException>());
            return this;
        }
        
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>()
        {
            Results = Results.Add(new ExceptionResult<TResult,TException>());
            return this;
        }
        
        IVoidResult<T> IActionResult<T>.Throws<TException>()
        {
            Results = Results.Add(new ExceptionResult<TResult,TException>());
            return this;
        }

        IVoidResult<T> IVoidResult<T>.ThenThrows<TException>()
        {
            Results = Results.Add(new ExceptionResult<TResult,TException>());
            return this;
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result)
        {
            Results = Results.Add(new FuncResult<TResult>(result));
            return this;
        }
        
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result)
        {
            Results = Results.Add(new FuncResult<TResult>(result));
            return this;
        }

        public TResult Execute() => Results.Result;
    }
}
