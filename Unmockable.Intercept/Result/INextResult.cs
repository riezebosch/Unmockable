namespace Unmockable.Result
{
    internal interface INextResult<T>
    {
        T Result { get; }
        INextResult<T> Next { get; set; }
    }
}