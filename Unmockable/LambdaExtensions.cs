using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable
{
    public static class LambdaExtensions
    {
        public static int ToKey(this LambdaExpression m)
        {
            var call = m.Body as MethodCallExpression ?? throw new NotInstanceMethodCallException(m.ToString());
            return call.ToString().GetHashCode();
        }
        
        public static MethodMatcher ToMatcher(this LambdaExpression m)
        {
            return new MethodMatcher(m); 
        }
    }
}