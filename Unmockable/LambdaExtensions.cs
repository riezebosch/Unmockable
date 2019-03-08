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
                (hash, arg) => hash ^ (Expression.Lambda(arg).Compile().DynamicInvoke()?.GetHashCode() ?? 0));
        }
    }
}