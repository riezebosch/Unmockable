namespace Unmockable.Result
{
    public class ValueResult<T> : INextResult<T>
    {
        private INextResult<T> _next;
        public T Result { get; }

        public INextResult<T> Next
        {
            get => _next ?? this;
            set => _next = value;
        }

        public ValueResult(T result) => Result = result;
    }
}