namespace Unmockable.Setup
{
    public interface IIntercept<T>: ISetupAction<T>, ISetupFunc<T>, ISetupFuncAsync<T>
    {
    }
}