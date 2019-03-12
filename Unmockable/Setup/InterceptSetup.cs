using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;

namespace Unmockable.Setup
{
    internal class InterceptSetup<T> : SetupBase<Intercept<T>>, IActionResult<T>
    {
        public InterceptSetup(Intercept<T> intercept, LambdaExpression expression) : base(intercept, expression)
        {
        }
        
        IFuncResult<T, TResult> ISetupFunc<T>.Setup<TResult>(Expression<Func<T, TResult>> m) => Intercept.Setup(m);

        public IActionResult<T> Setup(Expression<Action<T>> m) => Intercept.Setup(m);
    }

    internal class InterceptSetup<T, TResult> : InterceptSetup<T>, IFuncResult<T, TResult>
    {
        private Func<TResult> _result;
        public TResult Result => _result();

        public InterceptSetup(Intercept<T> intercept, LambdaExpression expression) : base(intercept, expression)
        {
            _result = typeof(TResult) == typeof(Task)
                ? (Func<TResult>) (() => (TResult) (object) Task.CompletedTask)
                : () => throw new NoResultConfiguredException(expression.ToString());
        }

        Intercept<T> IFuncResult<T, TResult>.Returns(TResult result)
        {
            _result = () => result;
            return Intercept;
        }
    }
}