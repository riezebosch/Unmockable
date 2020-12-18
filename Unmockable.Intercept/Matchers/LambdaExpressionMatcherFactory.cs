using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Matchers
{
    internal static class LambdaExpressionMatcherFactory
    {
        public static IMemberMatcher ToMatcher(this LambdaExpression m) => m.Body switch
        {
            MethodCallExpression body => new MethodMatcher(body),
            MemberExpression body => new PropertyMatcher(body),
            _ => throw new UnsupportedExpressionException(m.ToString())
        };
    }
}