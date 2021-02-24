using System.Threading.Tasks;
using Unmockable.Matchers;
using Unmockable.Result;

namespace Unmockable.Setup
{
    internal class SetupAsync<T, TResult> : 
        Setup<T, Task<TResult>>, 
        IFuncResult<T, TResult>,
        IResult<T, TResult>
    {
        public SetupAsync(IIntercept<T> interceptor, IMemberMatcher expression) :
            base(interceptor, expression, new UninitializedResult<Task<TResult>>(expression))
        {
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result) => 
            Return(new AsyncFuncResult<TResult>(result));
        IResult<T, TResult> IFuncResult<T, TResult>.Throws<TException>() => 
            Return(new ExceptionResult<Task<TResult>,TException>());
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result) => 
            Return(new AsyncFuncResult<TResult>(result));
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>() => 
            Return(new ExceptionResult<Task<TResult>,TException>());
        
        private IResult<T, TResult> Return(IResult<Task<TResult>> result)
        {
            Result = Result.Add(result, Expression);
            return this;
        }
    }
}