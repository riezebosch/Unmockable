using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Result;

namespace Unmockable.Setup
{
    internal class SetupAsync<T, TResult> : 
        Setup<T, Task<TResult>>, 
        IFuncResult<T, TResult>,
        IResult<T, TResult>
    {
        public SetupAsync(IIntercept<T> interceptor, LambdaExpression expression) :
            base(interceptor, expression, new UninitializedResult<Task<TResult>>(expression))
        {
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result) => 
            Return(new AsyncFuncResult<TResult>(result, Expression));
        IResult<T, TResult> IFuncResult<T, TResult>.Throws<TException>() => 
            Return(new ExceptionResult<Task<TResult>,TException>(Expression));
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result) => 
            Return(new AsyncFuncResult<TResult>(result, Expression));
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>() => 
            Return(new ExceptionResult<Task<TResult>,TException>(Expression));
        
        private IResult<T, TResult> Return(IResult<Task<TResult>> result)
        {
            Result = Result.Add(result);
            return this;
        }
    }
}