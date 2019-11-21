using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Matchers
{
    internal static class LambdaExpressionMatcherFactory
    {
        public static IUnmockableMatcher ToMatcher(this LambdaExpression m) => m.Body switch
        {
            MethodCallExpression body => (IUnmockableMatcher)new MethodMatcher(body),
            MemberExpression body => new PropertyMatcher(body),
            _ => throw new NotSupportedExpressionException(m.ToString())
        };
    }
}