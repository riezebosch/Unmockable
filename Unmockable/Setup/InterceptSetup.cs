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
        private Func<TResult> _result;
        
        private Exception _exception;
        protected IIntercept<T> Intercept { get; }
        public LambdaExpression Expression { get; }
        public bool IsExecuted { get; private set; }

        public InterceptSetup(IIntercept<T> intercept, LambdaExpression expression)
        {
            Intercept = intercept;
            Expression = expression;
            _result = typeof(TResult) == typeof(Task)
                ? (Func<TResult>) (() => (TResult) (object) Task.CompletedTask)
                : () => throw new NoResultConfiguredException(expression.ToString());
        }

        public void Execute()
        {
            IsExecuted = true;
            if (_exception != null)
            {
                throw _exception;
            }
        }

        public IIntercept<T> Throws<TException>() where TException : Exception, new()
        {
            _exception = new TException();
            return Intercept;
        }

        public IIntercept<T> Returns(TResult result)
        {
            Result = result;
            return Intercept;
        }

        public virtual TResult Result 
        {
            get
            {
                Execute();
                return _result();
            }

            protected set => _result = () => value;
        }

        IActionResult<T> ISetupAction<T>.Setup(Expression<Action<T>> m) => Intercept.Setup(m);

        IFuncResult<T, TNewResult> ISetupFunc<T>.Setup<TNewResult>(Expression<Func<T, TNewResult>> m) => Intercept.Setup(m);
    }
}