using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;

namespace Unmockable
{
    internal class InterceptSetupAsync<T, TResult> : InterceptSetup<T>, IFuncResult<T, TResult>
    {
        private Func<Task<TResult>> _result;
        public Task<TResult> Result => _result();

        public InterceptSetupAsync(Intercept<T> intercept, Expression expression) : base(
            intercept, expression)
        {
            _result = () => throw new NoResultConfiguredException(expression.ToString());
        }

        public Intercept<T> Returns(TResult result)
        {
            _result = () => Task.FromResult(result);
            return Intercept;
        }
    }
}