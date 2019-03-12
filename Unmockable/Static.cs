using System;
using System.Linq.Expressions;

namespace Unmockable
{
    public class Static : IStatic
    {
        private readonly MethodCache _cache = new MethodCache(); 
        TResult IStatic.Execute<TResult>(Expression<Func<TResult>> m)
        {
            return _cache.Methods<Func<TResult>>(m).Invoke();
        }

        void IStatic.Execute(Expression<Action> m)
        {
            throw new NotImplementedException();
        }
    }
}