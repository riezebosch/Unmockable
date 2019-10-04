using System.Collections.Generic;
using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Result
{
    internal class MultipleResult<T> : IResult<T>
    {
        private readonly LambdaExpression _expression;
        private readonly Queue<IResult<T>> _results = new Queue<IResult<T>>();

        public MultipleResult(LambdaExpression expression)
        {
            _expression = expression;
        }

        public T Result => _results.Count > 0 ? _results.Dequeue().Result : throw new OutOfResultsException(_expression);
        public bool IsDone => _results.Count == 0;
        public IResult<T> Add(IResult<T> next)
        {
            _results.Enqueue(next);
            return this;
        }
    }
}