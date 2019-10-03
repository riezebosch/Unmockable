using System;

namespace Unmockable.Result
{
    internal class ExceptionResult<T, TException> : IResult<T> where TException: Exception, new()
    {
        public T Result
        {
            get
            {
                IsDone = true;
                throw new TException();
            }
        }

        public bool IsDone { get; private set; }
        public IResult<T> Add(IResult<T> next) => new MultipleResult<T>().Add(this);
    }
}