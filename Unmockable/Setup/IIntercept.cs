namespace Unmockable.Setup
{
    public interface IIntercept: ISetupAction, ISetupFunc
    {
    }
    
    public interface IIntercept<T>: ISetupAction<T>, ISetupFunc<T>
    {
    }
}