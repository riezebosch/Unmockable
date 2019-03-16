using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Setup;

namespace Unmockable.Result
{
    internal class First<T> : INextResult<T>
    {
        private readonly LambdaExpression _expression;
        private INextResult<T> _next;

        public First(LambdaExpression expression)
        {
            _expression = expression;
        }

        public T Result => throw new Exception();

        public INextResult<T> Next
        {
            get => _next ?? Default();
            set => _next = value;
        }

        private INextResult<T> Default()
        {
            if (typeof(T) == typeof(Nothing))
            {
                return new ValueResult<T>(default(T));
            }

            if (typeof(T) == typeof(Task))
            {
                return new ValueResult<T>((T)(object)Task.CompletedTask);
            }

            return new Done<T>(_expression);
        }
    }
}