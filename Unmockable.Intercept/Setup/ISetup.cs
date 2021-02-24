using Unmockable.Matchers;

namespace Unmockable.Setup
{
    internal interface ISetup
    {
        IMemberMatcher Expression { get; }
        bool IsExecuted { get; }
    }

    internal interface ISetup<out TResult> : ISetup
    {
        TResult Execute();
    }
}