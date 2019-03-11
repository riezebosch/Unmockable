using System.Collections.Generic;
using System.Linq.Expressions;

namespace Unmockable
{
    class MethodCache
    {
        private readonly Dictionary<int, object> _cache = new Dictionary<int, object>();

        public TMethod Methods<TMethod>(LambdaExpression m)
        {
            var key = m.ToKey();
            if (_cache.TryGetValue(key, out var method))
            {
                return (TMethod) method;
            }

            method = m.Compile();
            return (TMethod)(_cache[key] = method);
        }
    }
}