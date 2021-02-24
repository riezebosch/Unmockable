using Unmockable.Matchers;

namespace Unmockable.Result
{
    internal interface IResult<T>
    {
        T Value { get; }
        bool IsDone { get; }
        IResult<T> Add(IResult<T> result, IMemberMatcher matcher);
    }
}