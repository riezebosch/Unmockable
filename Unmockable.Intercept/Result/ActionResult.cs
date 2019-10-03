using System.Threading.Tasks;

namespace Unmockable.Result
{
    internal class ActionResult : IResult<Task>
    {
        public Task Result
        {
            get
            {
                IsDone = true;
                return Task.CompletedTask;
            }
        }

        public bool IsDone { get; private set; }
        public IResult<Task> Add(IResult<Task> next) => new MultipleResult<Task>().Add(next);
    }
}