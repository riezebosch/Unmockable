using System.Linq.Expressions;
using Unmockable.Matchers;

namespace Unmockable
{
    internal static class LambdaExtensions
    {
        public static int ToKey(this LambdaExpression m)
        {
            return m.ToString().GetHashCode();
        }
        
        public static MethodMatcher ToMatcher(this LambdaExpression m)
        {
            return new MethodMatcher(m); 
        }
    }
}