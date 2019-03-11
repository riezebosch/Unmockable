using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;

namespace Unmockable
{
    internal class InterceptSetup<T> : IActionResult<T>
    {
        private Exception _exception;
        protected Intercept<T> Intercept { get; }
        public Expression Expression { get; }


        public bool IsExecuted { get; private set; }

        public InterceptSetup(Intercept<T> intercept, Expression expression)
        {
            Expression = expression;
            Intercept = intercept;
        }
        
        public Intercept<T> Throws<TException>() 
            where TException: Exception, new()
        {
            _exception = new TException();
            return Intercept;
        }

        public InterceptSetup<T> Execute()
        {
            IsExecuted = true;
            if (_exception != null)
            {
                throw _exception;
            }

            return this;
        }

        public IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m)
        {
            return Intercept.Setup(m);
        }

        public IActionResult<T> Setup(Expression<Action<T>> m)
        {
            return Intercept.Setup(m);
        }
    }

    internal class InterceptSetup<T, TResult> : InterceptSetup<T>, IFuncResult<T, TResult>
    {
        private Func<TResult> _result;
        public TResult Result => _result();

        public InterceptSetup(Intercept<T> intercept, Expression expression) : base(intercept, expression)
        {
            _result = typeof(TResult) == typeof(Task)
                ? (Func<TResult>) (() => (TResult) (object) Task.CompletedTask)
                : () => throw new NoResultConfiguredException(expression.ToString());
        }

        public Intercept<T> Returns(TResult result)
        {
            _result = () => result;
            return Intercept;
        }
    }
}