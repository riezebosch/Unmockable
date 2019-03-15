using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Setup
{
    internal class InterceptSetupAsync<T, TResult> : 
        InterceptSetup<T, Task<TResult>>, 
        IFuncResult<T, TResult>,
        IResult<T, TResult>
    {
        public InterceptSetupAsync(IIntercept<T> intercept, LambdaExpression expression) : base(intercept, expression)
        {
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result)
        {
            Result.Add(() => Task.FromResult(result));
            return this;
        }
        
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result)
        {
            Result.Add(() => Task.FromResult(result));
            return this;
        }
        
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>()
        {
            Result.Add(() => throw new TException());
            return this;
        }
    }
}