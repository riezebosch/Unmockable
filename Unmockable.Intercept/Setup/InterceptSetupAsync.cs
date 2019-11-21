using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Result;

namespace Unmockable.Setup
{
    internal class InterceptSetupAsync<T, TResult> : 
        InterceptSetup<T, Task<TResult>>, 
        IFuncResult<T, TResult>,
        IResult<T, TResult>
    {
        public InterceptSetupAsync(IIntercept<T> parent, LambdaExpression expression) : base(parent, expression, new UninitializedResult<Task<TResult>>(expression))
        {
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result) => NewResult(new AsyncFuncResult<TResult>(result, Expression));

        IResult<T, TResult> IFuncResult<T, TResult>.Throws<TException>() => NewResult(new ExceptionResult<Task<TResult>,TException>(Expression));

        IResult<T, TResult> IResult<T, TResult>.Then(TResult result) => NewResult(new AsyncFuncResult<TResult>(result, Expression));
        
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>() => NewResult(new ExceptionResult<Task<TResult>,TException>(Expression));
        
        private IResult<T, TResult> NewResult(IResult<Task<TResult>> result)
        {
            Current = Current.NewResult(result);
            return this;
        }
    }
}