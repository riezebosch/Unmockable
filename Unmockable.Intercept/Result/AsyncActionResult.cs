using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Result
{
    internal class AsyncActionResult : FuncResult<Task>
    {
        public AsyncActionResult(LambdaExpression expression) : base(Task.CompletedTask, expression)
        {
        }
        public override IResult<Task> Add(IResult<Task> next) => next;
        public override string ToString() => "Task";
    }
}