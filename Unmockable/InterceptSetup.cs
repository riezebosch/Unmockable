using System;
using System.Linq.Expressions;

namespace Unmockable
{
    public class InterceptSetup<T>
    {
        private Exception _exception;
        protected readonly Intercept<T> _intercept;
        public Expression Expression { get; }


        public bool IsExecuted { get; private set; }

        public InterceptSetup(Intercept<T> intercept, Expression expression)
        {
            Expression = expression;
            _intercept = intercept;
        }
        
        public Intercept<T> Throws<TException>() 
            where TException: Exception, new()
        {
            _exception = new TException();
            return _intercept;
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
    }
    
    public class InterceptSetup<T, TResult> : InterceptSetup<T>
    {
        private Func<TResult> _result = () => default(TResult);
        public TResult Result => _result();

        public InterceptSetup(Intercept<T> intercept, Expression expression) : base(intercept, expression)
        {
        }

        public Intercept<T> Returns(TResult result)
        {
            _result = () => result;
            return _intercept;
        }
    }
}