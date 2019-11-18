using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal static class ArgMatcherFactory
    {
        public static IArgumentMatcher? Create(Expression arg) => arg switch
            {
                MethodCallExpression call when call.Method.DeclaringType == typeof(Arg) => call.Method.Name switch
                {
                    nameof(Arg.Ignore) => (IArgumentMatcher)new IgnoreArgument(),
                    nameof(Arg.Where) => new LambdaArgument(call.Arguments[0]),
                    _ => throw new InvalidOperationException()
                },
                _ => null
            };
    }
}
