using System.Linq.Expressions;

namespace Unmockable.Setup
{
    internal interface ISetup
    {
        LambdaExpression Expression { get; }
        bool IsExecuted { get; }
    }
}