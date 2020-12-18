using System;
using System.Linq.Expressions;

namespace Unmockable.Result
{
    internal class ExceptionResult<T, TException> : 
        IResult<T> where TException: Exception, new()
    {
        private readonly LambdaExpression _expression;

        public ExceptionResult(LambdaExpression expression) => 
            _expression = expression;

        public T Value
        {
            get
            {
                IsDone = true;
                throw new TException();
            }
        }

        public bool IsDone { get; private set; }
        public IResult<T> Add(IResult<T> result) => 
            new MultipleResults<T>(_expression)
                .Add(this)
                .Add(result);
        public override string ToString() => 
            typeof(TException).Name;
    }
}