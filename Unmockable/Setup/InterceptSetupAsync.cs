using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Setup
{
    internal class InterceptSetupAsync<T, TResult> : 
        InterceptSetup<T, Task<TResult>>, 
        IFuncResult<T, TResult>
    {
        public InterceptSetupAsync(IIntercept<T> intercept, LambdaExpression expression) : base(intercept, expression)
        {
        }

        IIntercept<T> IFuncResult<T, TResult>.Returns(TResult result)
        {
            Result = Task.FromResult(result);
            return Intercept;
        }
    }
}