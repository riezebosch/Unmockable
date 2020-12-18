namespace Unmockable.Result
{
    internal interface IResult<T>
    {
        T Result { get; }
        bool IsDone { get; }
        IResult<T> Next(IResult<T> next);
    }
}