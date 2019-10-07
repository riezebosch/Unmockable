using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Result
{
    internal class NoSetupResult<T> : IResult<T>
    {
        private readonly LambdaExpression _expression;

        public NoSetupResult(LambdaExpression expression) => _expression = expression;

        public T Result => throw new NoResultsSetupException(_expression.ToString());
        public bool IsDone => false;
        public IResult<T> Add(IResult<T> next) => next;
        public override string ToString() => "no results setup";
    }
}