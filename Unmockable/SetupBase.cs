using System;
using System.Linq.Expressions;

namespace Unmockable
{
    internal abstract class SetupBase<TIntercept> : ISetup
    {
        private Exception _exception;
        protected TIntercept Intercept { get; }
        public LambdaExpression Expression { get; }
        public bool IsExecuted { get; private set; }

        protected SetupBase(TIntercept intercept, LambdaExpression expression)
        {
            Intercept = intercept;
            Expression = expression;
        }

        public void Execute()
        {
            IsExecuted = true;
            if (_exception != null)
            {
                throw _exception;
            }
        }

        public TIntercept Throws<TException>() where TException : Exception, new()
        {
            _exception = new TException();
            return Intercept;
        }
    }
}