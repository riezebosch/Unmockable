using System;
using System.Linq.Expressions;

namespace Unmockable.Result
{
    internal class ExceptionResult<T, TException> : IResult<T> where TException: Exception, new()
    {
        private readonly LambdaExpression _expression;

        public ExceptionResult(LambdaExpression expression) => 
            _expression = expression;

        public T Result
        {
            get
            {
                IsDone = true;
                throw new TException();
            }
        }

        public bool IsDone { get; private set; }
        public IResult<T> Next(IResult<T> next) => 
            new MultipleResults<T>(_expression).Next(this).Next(next);
        public override string ToString() => 
            typeof(TException).Name;
    }
}