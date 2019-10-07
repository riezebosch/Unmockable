using System.Linq.Expressions;

namespace Unmockable.Setup
{
    public class Wrap<T> : ISetup<IUnmockable<T>>
    {
        private readonly IUnmockable<T> _with;

        public Wrap(LambdaExpression expression, IUnmockable<T> with)
        {
            Expression = expression;
            _with = with;
        }

        public IUnmockable<T> Execute()
        {
            IsExecuted = true;
            return _with;
        }

        public LambdaExpression Expression { get; }
        public bool IsExecuted { get; private set; }
    }
}