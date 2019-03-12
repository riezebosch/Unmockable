using System;
using System.Linq.Expressions;

namespace Unmockable.Setup
{
    internal interface ISetup
    {
        LambdaExpression Expression { get; }
        bool IsExecuted { get; }
        void Execute();
    }

    public interface ISetup<T>
    {
        IFuncResult<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> m);
    }
}