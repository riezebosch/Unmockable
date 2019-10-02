using System;
using System.Threading.Tasks;
using Unmockable.Setup;

namespace Unmockable.Result
{
    internal class First<T> : INextResult<T>
    {
        public T Result => throw new InvalidOperationException();

        public INextResult<T>? Next { get; set; } = Default();

        private static INextResult<T>? Default() =>
            typeof(T) == typeof(Nothing) ? new NothingResult() as INextResult<T> :
            typeof(T) == typeof(Task) ? new ValueResult<T>((T) (object) Task.CompletedTask) :
            null;
    }
}