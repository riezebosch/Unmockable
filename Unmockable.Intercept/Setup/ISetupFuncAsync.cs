using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Setup
{
    public interface ISetupFuncAsync<T>
    {
        IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, Task<TResult>>> m);
    }
}