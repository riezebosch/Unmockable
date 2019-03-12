using System;
using System.Linq.Expressions;

namespace Unmockable.Setup
{
    public interface ISetupAction
    {
        IActionResult Setup(Expression<Action> m);
    }
    
    public interface ISetupAction<T>
    {
        IActionResult<T> Setup(Expression<Action<T>> m);
    }
}