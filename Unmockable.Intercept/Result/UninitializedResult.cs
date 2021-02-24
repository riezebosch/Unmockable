using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable.Result
{
    internal class UninitializedResult<T> : IResult<T>
    {
        private readonly IMemberMatcher _expression;

        public UninitializedResult(IMemberMatcher expression) => 
            _expression = expression;

        public T Value => 
            throw new UninitializedException(_expression);
        public bool IsDone => 
            false;
        public IResult<T> Add(IResult<T> result, IMemberMatcher matcher) => 
            result;
        public override string ToString() => 
            "no results setup";
    }
}