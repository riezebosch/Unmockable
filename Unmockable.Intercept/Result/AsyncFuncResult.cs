using System.Threading.Tasks;

namespace Unmockable.Result
{
    internal class AsyncFuncResult<T> : FuncResult<Task<T>>
    {
        public AsyncFuncResult(T result) : 
            base(Task.FromResult(result))
        {
        }

        public override string ToString() => Value.Result!.ToString();
    }
}