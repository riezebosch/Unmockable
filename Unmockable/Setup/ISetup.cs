using System.Linq.Expressions;

namespace Unmockable.Setup
{
    internal interface ISetup
    {
        LambdaExpression Expression { get; }
        bool IsExecuted { get; }

        void Execute();
    }

    internal interface ISetup<out TResult> : ISetup
    {
        TResult Result { get; }
    }
}