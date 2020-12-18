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

        public FuncResult(T result, LambdaExpression expression)
        {
            _result = result;
            _expression = expression;
        }

        public virtual IResult<T> Next(IResult<T> next) => 
            new MultipleResults<T>(_expression)
                .Next(this)
                .Next(next);
        public override string ToString() => Result!.ToString();
    }
}