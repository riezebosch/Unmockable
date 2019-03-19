using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;
using Unmockable.Result;

namespace Unmockable.Setup
{
    internal class InterceptSetup<T, TResult> : 
        ISetup<TResult>, 
        IFuncResult<T, TResult>, 
        IResult<T, TResult>,
        IActionResult<T>
    {
        protected ResultContext<TResult> Results { get; } = new ResultContext<TResult>();

        private readonly IIntercept<T> _intercept;

        public LambdaExpression Expression { get; }
        
        public bool IsExecuted => Results.IsDone;

        public InterceptSetup(IIntercept<T> intercept, LambdaExpression expression)
        {
            Expression = expression;
            _intercept = intercept;
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
        
        public TResult Execute() => Results.HasNext 
            ? Results.Next()
            : throw new NoResultsSetupException(Expression.ToString());
    }
}
