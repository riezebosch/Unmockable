using System;
using System.Linq.Expressions;

namespace Unmockable.Setup
{
    internal class StaticSetup<TResult> : SetupBase<Intercept>, IFuncResult<TResult>
    {
        public StaticSetup(Intercept intercept, Expression<Func<TResult>> expression) : base(intercept, expression)
        {
        }

        Intercept IFuncResult<TResult>.Returns(TResult result)
        {
            Result = result;
            return Intercept;
        }

        public TResult Result { get; private set; }
    }
}