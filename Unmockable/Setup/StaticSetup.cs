using System;
using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Setup
{
    internal class StaticSetup : SetupBase<IIntercept>, IActionResult
    {
        public StaticSetup(IIntercept intercept, Expression<Action> expression) : base(intercept, expression)
        {
        }

        public IActionResult Setup(Expression<Action> m) => Intercept.Setup(m);
        public IFuncResult<TResult> Setup<TResult>(Expression<Func<TResult>> m) => Intercept.Setup(m);
    }
    
    internal class StaticSetup<TResult> : SetupBase<IIntercept>, ISetup<TResult>, IFuncResult<TResult>
    {
        private Func<TResult> _result;

        public StaticSetup(IIntercept intercept, Expression<Func<TResult>> expression) : base(intercept, expression)
        {
            _result = () => throw new NoResultConfiguredException(expression.ToString());
        }

        IIntercept IFuncResult<TResult>.Returns(TResult result)
        {
            _result = () => result;
            return Intercept;
        }

        
        public TResult Result
        {
            get
            {
                Execute();
                return _result();
            }
        }
    }
}