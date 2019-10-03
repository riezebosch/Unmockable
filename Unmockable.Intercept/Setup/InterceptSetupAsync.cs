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

        IResult<T, TResult> IFuncResult<T, TResult>.Returns(TResult result)
        {
            Results = new ValueResult<Task<TResult>>(Task.FromResult(result));
            return this;
        }
        
        IResult<T, TResult> IResult<T, TResult>.Then(TResult result)
        {
            Results = Results.Add(new ValueResult<Task<TResult>>(Task.FromResult(result)));
            return this;
        }
        
        IResult<T, TResult> IResult<T, TResult>.ThenThrows<TException>()
        {
            Results = Results.Add(new ExceptionResult<Task<TResult>,TException>());
            return this;
        }
    }
}