using Unmockable.Setup;

namespace Unmockable
{
    public interface IIntercept<T> : 
        ISetupAction<T>,
        ISetupFunc<T>,
        IUnmockable<T>
    {
        void Verify();
    }
}