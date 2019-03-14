using System.Linq.Expressions;

namespace Unmockable.Setup
{
    internal interface ISetup
    {
        LambdaExpression Expression { get; }
        bool IsExecuted { get; }
    }

    internal interface ISetup<out TResult> : ISetup
    {
        TResult Execute();
    }
}