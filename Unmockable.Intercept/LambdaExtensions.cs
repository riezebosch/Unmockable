using System.Linq.Expressions;
using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable
{
    internal static class LambdaExtensions
    {
        public static IUnmockableMatcher ToMatcher(this LambdaExpression m) => 
            new MethodMatcher(m.Body as MethodCallExpression ?? throw new NotSupportedExpressionException(m.ToString()));
    }
}