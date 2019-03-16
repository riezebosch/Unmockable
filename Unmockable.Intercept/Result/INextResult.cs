namespace Unmockable.Result
{
    public interface INextResult<T>
    {
        T Result { get; }
        INextResult<T> Next { get; set; }
    }
}