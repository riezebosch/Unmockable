using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Result
{
    internal class UninitializedResult<T> : IResult<T>
    {
        private readonly LambdaExpression _expression;

        public UninitializedResult(LambdaExpression expression) => _expression = expression;

        public T Result => throw new UninitializedFuncException(_expression.ToString());
        public bool IsDone => false;
        public IResult<T> Add(IResult<T> next) => next;
        public override string ToString() => "no results setup";
    }
}