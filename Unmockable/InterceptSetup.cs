using System;
using System.Linq.Expressions;

namespace Unmockable
{
    public class InterceptSetup<T>
    {
        public Expression Expression { get; }
        protected readonly Intercept<T> _intercept;
        public bool IsExecuted { get; internal set; }

        public InterceptSetup(Intercept<T> intercept, Expression expression)
        {
            Expression = expression;
            _intercept = intercept;
        }
    }
    
    public class InterceptSetup<T, TResult> : InterceptSetup<T>
    {
        private Func<TResult> _result;
        public TResult Result => _result();

        public InterceptSetup(Intercept<T> intercept, Expression expression) : base(intercept, expression)
        {
        }

        public Intercept<T> Returns(TResult result)
        {
            _result = () => result;
            return _intercept;
        }

        public Intercept<T> Throws<TException>() 
            where TException: Exception, new()
        {
            _result = () => throw new TException();
            return _intercept;
        }
    }
}