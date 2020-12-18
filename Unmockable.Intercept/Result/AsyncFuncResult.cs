using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Result
{
    internal class AsyncFuncResult<T> : FuncResult<Task<T>>
    {
        public AsyncFuncResult(T result, LambdaExpression expression) : 
            base(Task.FromResult(result), expression)
        {
        }

        public override string ToString() => Result.Result!.ToString();
    }
}