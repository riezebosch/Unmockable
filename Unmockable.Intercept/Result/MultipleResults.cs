using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Result
{
    internal class MultipleResults<T> : IResult<T>
    {
        private readonly LambdaExpression _expression;
        private readonly Queue<IResult<T>> _results = new Queue<IResult<T>>();

        public MultipleResults(LambdaExpression expression) => _expression = expression;

        public T Result => !IsDone ? _results.Dequeue().Result : throw new OutOfResultsException(_expression);
        public bool IsDone => !_results.Any();
        public IResult<T> NewResult(IResult<T> next)
        {
            _results.Enqueue(next);
            return this;
        }

        public override string ToString() => string.Join(", ", _results);
    }
}