using System;

namespace Unmockable.Result
{
    internal class ExceptionResult<T, TException> : INextResult<T> where TException: Exception, new()
    {
        public T Result => throw new TException();
        public INextResult<T>? Next { get; set; }
    }
}