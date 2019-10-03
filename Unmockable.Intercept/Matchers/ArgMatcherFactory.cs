using System;
using System.Linq;
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
                    nameof(Arg.Where) => new LambdaArgument(call.Arguments.Single()),
                    _ => throw new NotImplementedException()
                },
                _ => null
            };
    }
}