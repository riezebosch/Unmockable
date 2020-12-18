using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal static class ArgMatcherFactory
    {
        public static IArgumentMatcher? Create(Expression arg) =>
            arg is MethodCallExpression call && call.Method.DeclaringType == typeof(Arg)
                ? call.Method.Name switch
                {
                    nameof(Arg.Ignore) => new IgnoreArgument() as IArgumentMatcher,
                    nameof(Arg.Where) => new WhereArgument((LambdaExpression) call.Arguments[0]),
                    nameof(Arg.With) => new WithArgument((LambdaExpression) call.Arguments[0]),
                    _ => throw new InvalidOperationException()
                }
                : null;
    }
}
