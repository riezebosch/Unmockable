using System.Collections.Generic;
using System.Linq;

namespace Unmockable.Result
{
    internal class MultipleResult<T> : IResult<T>
    {
        private readonly IList<IResult<T>> _results = new List<IResult<T>>();
        private int _current;

        public T Result => _results[_current++].Result;
        public bool IsDone => _results.All(x => x.IsDone);
        public IResult<T> Add(IResult<T> next)
        {
            _results.Add(next);
            return this;
        }
    }
}