using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Setup
{
    internal class InterceptSetup<T, TResult> : 
        ISetup<TResult>, 
        IFuncResult<T, TResult>, 
        IResult<T, TResult>,
        IActionResult<T>
    {
        protected ResultMachine<TResult> Results { get; } = new ResultMachine<TResult>();

        private readonly IIntercept<T> _intercept;
        
        public LambdaExpression Expression { get; }
        
        public bool IsExecuted => Results.IsExecuted;

        public InterceptSetup(IIntercept<T> intercept, LambdaExpression expression)
        {
            _intercept = intercept;
            Expression = expression;
        }

        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => _intercept.Setup(m);

        IFuncResult<T, TNewResult> ISetupFunc<T>.Setup<TNewResult>(Expression<Func<T, TNewResult>> m) => _intercept.Setup(m);
        
        IFuncResult<T, TResultNew> ISetupFuncAsync<T>.Setup<TResultNew>(Expression<Func<T, Task<TResultNew>>> m) => _intercept.Setup(m);
        
        public IIntercept<T> Throws<TException>() where TException : Exception, new()
        {
            Results.Add<TException>();
            return _intercept;
        }
        
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>()
        {
            Results.Add<TException>();
            return this;
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result)
        {
            Results.Add(result);
            return this;
        }
        
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result)
        {
            Results.Add(result);
            return this;
        }
        
        public TResult Execute()
        {
            return Results.NextResult(Expression)();
        }
    }
}
