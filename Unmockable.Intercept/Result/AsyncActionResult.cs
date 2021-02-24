using System.Threading.Tasks;
using Unmockable.Matchers;

namespace Unmockable.Result
{
    internal class AsyncActionResult : FuncResult<Task>
    {
        public AsyncActionResult() : 
            base(Task.CompletedTask)
        {
        }
        public override IResult<Task> Add(IResult<Task> result, IMemberMatcher matcher) => 
            result;
        public override string ToString() => 
            "Task";
    }
}