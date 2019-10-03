namespace Unmockable.Result
{
    internal class ValueResult<T> : IResult<T>
    {
        private readonly T _result;

        public T Result
        {
            get
            {
                IsDone = true;
                return _result;
            }
        }

        public bool IsDone { get; private set; }
        public IResult<T> Add(IResult<T> next) => new MultipleResult<T>().Add(this).Add(next);

        public ValueResult(T result)
        {
            _result = result;
        }
    }
}