using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;

namespace Unmockable.Setup
{
    internal class InterceptSetupAsync<T, TResult> : InterceptSetup<T>, ISetup<Task<TResult>>, IFuncResult<T, TResult>
    {
        private Func<Task<TResult>> _result;

        public InterceptSetupAsync(IIntercept<T> intercept, LambdaExpression expression) : base(intercept, expression) =>
            _result = () => throw new NoResultConfiguredException(expression.ToString());

        IIntercept<T> IFuncResult<T, TResult>.Returns(TResult result)
        {
            _result = () => Task.FromResult(result);
            return Intercept;
        }

        public Task<TResult> Result
        { 
            get
            {
                Execute();
                return _result();
            }
        }
    }
}