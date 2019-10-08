using System.Linq.Expressions;
using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable
{
    internal static class LambdaExtensions
    {
        public static IUnmockableMatcher ToMatcher(this LambdaExpression m) => m.Body switch
        {
            MethodCallExpression body => (IUnmockableMatcher)new MethodMatcher(body),
            MemberExpression body => new PropertyMatcher(body),
            _ => throw new NotSupportedExpressionException(m.ToString())
        };
    }
}