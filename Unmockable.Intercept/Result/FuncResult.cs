using System.Linq.Expressions;

namespace Unmockable.Result
{
    internal class FuncResult<T> : IResult<T>
    {
        private readonly T _result;
        private readonly LambdaExpression _expression;

        public T Result
        {
            get
            {
                IsDone = true;
                return _result;
            }
        }

        public bool IsDone { get; private set; }
        public IResult<T> Add(IResult<T> next) => new MultipleResult<T>(_expression).Add(this).Add(next);

        public FuncResult(T result, LambdaExpression expression)
        {
            _result = result;
            _expression = expression;
        }

        public override string ToString() => Result.ToString();
    }
}