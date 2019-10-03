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
        public InterceptSetupAsync(IIntercept<T> intercept, LambdaExpression expression) : base(intercept, expression)
        {
        }

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result) => Add(new FuncResult<Task<TResult>>(Task.FromResult(result)));

        IResult<T, TResult> IFuncResult<T, TResult>.Throws<TException>() => Add(new ExceptionResult<Task<TResult>,TException>());

        IResult<T, TResult> IResult<T, TResult>.Then(TResult result) => Add(new FuncResult<Task<TResult>>(Task.FromResult(result)));
        
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>() => Add(new ExceptionResult<Task<TResult>,TException>());
        
        private IResult<T, TResult> Add(IResult<Task<TResult>> result)
        {
            Results = Results.Add(result);
            return this;
        }
    }
}