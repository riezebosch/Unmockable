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
        protected readonly Queue<Func<TResult>> Result = new Queue<Func<TResult>>();
        
        private readonly IIntercept<T> _intercept;
        public LambdaExpression Expression { get; }
        public bool IsExecuted => !Result.Any();

        public InterceptSetup(IIntercept<T> intercept, LambdaExpression expression)
        {
            _intercept = intercept;
            Expression = expression;
            Result.Enqueue(DefaultResult(expression));
        }

        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => _intercept.Setup(m);

        IFuncResult<T, TNewResult> ISetupFunc<T>.Setup<TNewResult>(Expression<Func<T, TNewResult>> m) => _intercept.Setup(m);
        
        IFuncResult<T, TResultNew> ISetupFuncAsync<T>.Setup<TResultNew>(Expression<Func<T, Task<TResultNew>>> m) => _intercept.Setup(m);
        
        public IIntercept<T> Throws<TException>() where TException : Exception, new()
        {
            Result.Clear();
            Result.Enqueue(() => throw new TException());
            
            return _intercept;
        }
        
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>()
        {
            Result.Enqueue(() => throw new TException());
            return this;
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result)
        {
            Result.Clear();
            Result.Enqueue(() => result);
            
            return this;
        }
        
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result)
        {
            Result.Enqueue(() => result);
            return this;
        }
        
        public TResult Execute()
        {
            return Result.Dequeue()();
        }

        private static Func<TResult> DefaultResult(LambdaExpression expression)
        {
            if (typeof(TResult) == typeof(Task))
                return () => (TResult)(object)Task.CompletedTask;

            if (typeof(TResult) == typeof(Nothing))
                return () => (TResult)(object)default(Nothing);
            
            return () => throw new NoResultConfiguredException(expression.ToString());
        }
    }
}
