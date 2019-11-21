using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Unmockable.Setup
{
    public interface ISetupFunc<T>
    {
        IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m);
        IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, Task<TResult>>> m);
    }
}