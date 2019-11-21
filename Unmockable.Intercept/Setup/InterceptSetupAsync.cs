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
        public InterceptSetupAsync(IIntercept<T> intercept, LambdaExpression expression) : base(intercept, expression, new UninitializedResult<Task<TResult>>(expression))
        {
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result) => Add(new AsyncFuncResult<TResult>(result, Expression));

        IResult<T, TResult> IFuncResult<T, TResult>.Throws<TException>() => Add(new ExceptionResult<Task<TResult>,TException>(Expression));

        IResult<T, TResult> IResult<T, TResult>.Then(TResult result) => Add(new AsyncFuncResult<TResult>(result, Expression));
        
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>() => Add(new ExceptionResult<Task<TResult>,TException>(Expression));
        
        private IResult<T, TResult> Add(IResult<Task<TResult>> result)
        {
            Results = Results.Add(result);
            return this;
        }
    }
}