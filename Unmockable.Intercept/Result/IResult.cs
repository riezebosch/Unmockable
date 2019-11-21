namespace Unmockable.Result
{
    internal interface IResult<T>
    {
        T Result { get; }
        bool IsDone { get; }
        IResult<T> NewResult(IResult<T> next);
    }
}