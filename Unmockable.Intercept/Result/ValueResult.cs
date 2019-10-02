namespace Unmockable.Result
{
    internal class ValueResult<T> : INextResult<T>
    {
        public T Result { get; }

        public INextResult<T>? Next { get; set; }

        public ValueResult(T result)
        {
            Result = result;
            Next = this;
        }
    }
}