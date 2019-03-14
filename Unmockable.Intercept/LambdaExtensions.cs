using System.Linq.Expressions;
using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable
{
    internal static class LambdaExtensions
    {
        public static int ToKey(this LambdaExpression m)
        {
            return m.ToString().GetHashCode();
        }
        
        public static IUnmockableMatcher ToMatcher(this LambdaExpression m)
        {
            switch (m.Body)
            {
                case MemberExpression arg:
                    return new PropertyMatcher(arg);
                case MethodCallExpression arg:
                    return new MethodMatcher(arg);
                default:
                    throw new NotSupportedExpressionException(m.ToString());
            }
        }
    }
}