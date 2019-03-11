using System;
using System.Linq.Expressions;

namespace Unmockable
{
    public class Static : IStatic
    {
        private readonly MethodCache _cache = new MethodCache(); 
        public TResult Execute<TResult>(Expression<Func<TResult>> m)
        {
            return _cache.Methods<Func<TResult>>(m).Invoke();
        }
    }
}