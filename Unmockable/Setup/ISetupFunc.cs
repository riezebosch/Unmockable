using System;
using System.Linq.Expressions;

namespace Unmockable.Setup
{
    public interface ISetupFunc
    {
        IFuncResult<TResult> Setup<TResult>(Expression<Func<TResult>> m);
    }
    
    public interface ISetupFunc<T>
    {
        IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m);
    }
}