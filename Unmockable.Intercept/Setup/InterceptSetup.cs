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
        protected Queue<Func<TResult>> Result = new Queue<Func<TResult>>();
        protected IIntercept<T> Intercept { get; }
        public LambdaExpression Expression { get; }
        public bool IsExecuted => !Result.Any();

        public InterceptSetup(IIntercept<T> intercept, LambdaExpression expression)
        {
            Intercept = intercept;
            Expression = expression;
            Result.Enqueue(InitializeResult(expression));
        }

        private static Func<TResult> InitializeResult(LambdaExpression expression)
        {
            if (typeof(TResult) == typeof(Task))
                return () => (TResult)(object)Task.CompletedTask;

            if (typeof(TResult) == typeof(Nothing))
                return () => (TResult)(object)default(Nothing);
            
            return () => throw new NoResultConfiguredException(expression.ToString());
        }

        public IIntercept<T> Throws<TException>() where TException : Exception, new()
        {
            Result.Clear();
            Result.Enqueue(() => throw new TException());
            
            return Intercept;
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result)
        {
            Result.Clear();
            Result.Enqueue(() => result);
            
            return this;
        }

        public TResult Execute()
        {
            return Result.Dequeue()();
        }

        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => Intercept.Setup(m);

        IFuncResult<T, TNewResult> ISetupFunc<T>.Setup<TNewResult>(Expression<Func<T, TNewResult>> m) => Intercept.Setup(m);
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result)
        {
            Result.Enqueue(() => result);
            return this;
        }
    }
}