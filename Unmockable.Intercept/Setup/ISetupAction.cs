using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Setup
{
    public interface ISetupAction<T>
    {
        IActionResult<T> Setup(Expression<Action<T>> m);
        IActionResult<T> Setup(Expression<Func<T, Task>> m);
    }
}