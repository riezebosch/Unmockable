using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;

namespace Unmockable.Setup
{
    internal class InterceptSetup<T, TResult> : 
        ISetup<TResult>, 
        IFuncResult<T, TResult>, 
        IResult<T, TResult>,
        IActionResult<T>
    {
        protected readonly IList<Func<TResult>> Result = new List<Func<TResult>>();
        
        private readonly IIntercept<T> _intercept;
        
        private int _invocation;
        
        public LambdaExpression Expression { get; }
        
        public bool IsExecuted => _invocation >= Result.Count;

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
            Result.Add(() => throw new TException());
            return _intercept;
        }
        
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>()
        {
            Result.Add(() => throw new TException());
            return this;
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result)
        {
            Result.Add(() => result);
            return this;
        }
        
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result)
        {
            Result.Add(() => result);
            return this;
        }
        
        public TResult Execute()
        {
            return NextResult()();
        }

        private Func<TResult> NextResult()
        {
            return Result.ElementAtOrDefault(_invocation++) ?? DefaultResult();
        }

        private Func<TResult> DefaultResult()
        {
            if (typeof(TResult) == typeof(Task))
                return () => (TResult)(object)Task.CompletedTask;

            if (typeof(TResult) == typeof(Nothing))
                return () => (TResult)(object)default(Nothing);
            
            return () => throw new NoMoreResultsSetupException(Expression.ToString());
        }
    }
}
