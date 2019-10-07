using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Result
{
    internal class AsyncFuncResult<T> : IResult<Task<T>>
    {
        private readonly T _result;
        private readonly LambdaExpression _expression;

        public AsyncFuncResult(T result, LambdaExpression expression)
        {
            _result = result;
            _expression = expression;
        }

        public Task<T> Result
        {
            get
            {
                IsDone = true;
                return Task.FromResult(_result);
            }
        }
        public bool IsDone { get; private set; }
        public IResult<Task<T>> Add(IResult<Task<T>> next) => new MultipleResult<Task<T>>(_expression).Add(this).Add(next);
        public override string ToString() => _result.ToString();
    }
}