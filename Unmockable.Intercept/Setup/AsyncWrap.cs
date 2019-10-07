using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Setup
{
    public class AsyncWrap<T> : ISetup<Task<IUnmockable<T>>>
    {
        private readonly IUnmockable<T> _with;

        public AsyncWrap(LambdaExpression expression, IUnmockable<T> with)
        {
            Expression = expression;
            _with = with;
        }

        public Task<IUnmockable<T>> Execute()
        {
            IsExecuted = true;
            return Task.FromResult(_with);
        }

        public LambdaExpression Expression { get; }
        public bool IsExecuted { get; private set; }
    }
}