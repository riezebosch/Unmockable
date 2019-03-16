using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Result
{
    internal class Done<T> : INextResult<T>
    {
        private readonly LambdaExpression _expression;
        public T Result => throw new NoMoreResultsSetupException(_expression.ToString());

        public Done(LambdaExpression expression)
        {
            _expression = expression;
        }

        public INextResult<T> Next
        {
            get => this;
            set { }
        }
    }
}