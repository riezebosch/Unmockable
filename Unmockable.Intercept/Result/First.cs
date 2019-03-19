using System;
using System.Threading.Tasks;
using Unmockable.Setup;

namespace Unmockable.Result
{
    internal class First<T> : INextResult<T>
    {
        public T Result => throw new InvalidOperationException();

        public INextResult<T> Next { get; set; } = Default();

        private static INextResult<T> Default()
        {
            if (typeof(T) == typeof(Nothing))
            {
                return new ValueResult<T>(default(T));
            }

            if (typeof(T) == typeof(Task))
            {
                return new ValueResult<T>((T)(object)Task.CompletedTask);
            }

            return null;
        }
    }
}