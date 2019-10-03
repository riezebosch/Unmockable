using System.Collections.Generic;
using Unmockable.Exceptions;

namespace Unmockable.Result
{
    internal class MultipleResult<T> : IResult<T>
    {
        private readonly Queue<IResult<T>> _results = new Queue<IResult<T>>();

        public T Result => _results.Count > 0 ? _results.Dequeue().Result : throw new OutOfSetupException();
        public bool IsDone => _results.Count == 0;
        public IResult<T> Add(IResult<T> next)
        {
            _results.Enqueue(next);
            return this;
        }
    }
}