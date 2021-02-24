using System.Collections.Generic;
using System.Linq;
using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable.Result
{
    internal class MultipleResults<T> : IResult<T>
    {
        private readonly IMemberMatcher _expression;
        private readonly Queue<IResult<T>> _results = new Queue<IResult<T>>();

        public MultipleResults(IMemberMatcher expression) => 
            _expression = expression;

        public T Value => 
            !IsDone 
                ? _results.Dequeue().Value 
                : throw new OutOfResultsException(_expression);
        public bool IsDone => 
            !_results.Any();
        
        public IResult<T> Add(IResult<T> result, IMemberMatcher matcher)
        {
            _results.Enqueue(result);
            return this;
        }

        public override string ToString() => 
            string.Join(", ", _results);
    }
}