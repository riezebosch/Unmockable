using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;

namespace Unmockable.Setup
{
    internal class InterceptSetup<T, TResult> : 
        ISetup<TResult>, 
        IFuncResult<T, TResult>, 
        IActionResult<T>
    {
        protected Func<TResult> Result;
        protected IIntercept<T> Intercept { get; }
        public LambdaExpression Expression { get; }
        public bool IsExecuted { get; private set; }

        public InterceptSetup(IIntercept<T> intercept, LambdaExpression expression)
        {
            Intercept = intercept;
            Expression = expression;
            Result = InitializeResult(expression);
        }

        private static Func<TResult> InitializeResult(LambdaExpression expression)
        {
            if (typeof(TResult) == typeof(Task))
                return () => (TResult)(object)Task.CompletedTask;

            if (typeof(TResult) == typeof(Unit))
                return () => (TResult)(object)default(Unit);
            
            return () => throw new NoResultConfiguredException(expression.ToString());
        }

        public IIntercept<T> Throws<TException>() where TException : Exception, new()
        {
            Result = () => throw new TException();
            return Intercept;
        }

        public IIntercept<T> Returns(TResult result)
        {
            Result =  () => result;
            return Intercept;
        }

        public virtual TResult Execute()
        {
            IsExecuted = true;
            return Result();
        }

        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => Intercept.Setup(m);

        IFuncResult<T, TNewResult> ISetupFunc<T>.Setup<TNewResult>(Expression<Func<T, TNewResult>> m) => Intercept.Setup(m);
    }
}