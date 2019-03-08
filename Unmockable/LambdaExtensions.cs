using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable
{
    public static class LambdaExtensions
    {
        public static int ToKey(this LambdaExpression m)
        {
            var call = (m.Body as MethodCallExpression) ?? throw new NotInstanceMethodCallException(m.ToString());
            return call.Arguments.Aggregate(call.Method.Name.GetHashCode(), 
                (hash, arg) => hash ^ Hash(arg));
        }

        private static int Hash(Expression arg)
        {
            var value = Expression.Lambda(arg).Compile().DynamicInvoke();
            switch (value)
            {
                case null:
                    return 0;
                case IEnumerable collection:
                    return collection.Cast<object>().Aggregate(0, (hash, item) => hash ^ item.GetHashCode());
                default:
                    return value.GetHashCode();
            }
        }
    }
}