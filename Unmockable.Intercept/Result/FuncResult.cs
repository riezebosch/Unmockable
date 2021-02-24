using Unmockable.Matchers;

namespace Unmockable.Result
{
    internal class FuncResult<T> : IResult<T>
    {
        private readonly T _result;

        public T Value
        {
            get
            {
                IsDone = true;
                return _result;
            }
        }

        public bool IsDone { get; private set; }

        public FuncResult(T result) => _result = result;

        public virtual IResult<T> Add(IResult<T> result, IMemberMatcher matcher) => 
            new MultipleResults<T>(matcher)
                .Add(this, matcher)
                .Add(result, matcher);
        public override string ToString() => 
            Value!.ToString();
    }
}