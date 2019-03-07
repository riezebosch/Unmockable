using System;

namespace Unmockable
{
    public class InterceptSetup<T, TResult>
    {
        private readonly Intercept<T> _intercept;
        private Func<TResult> _result;
        public TResult Result => _result();

        public InterceptSetup(Intercept<T> intercept)
        {
            _intercept = intercept;
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