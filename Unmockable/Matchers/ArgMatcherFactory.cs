using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal static class ArgMatcherFactory
    {
        public static IArgumentMatcher Create(Expression arg)
        {
            if (arg is MethodCallExpression call && call.Method.DeclaringType == typeof(Arg))
            {
                switch (call.Method.Name)
                {
                    case "Ignore":
                        return new IgnoreArgument();
                    case "Equals":
                        return new EqualsArgument(call.Arguments[0]);
                }
            }

            return null;
        }
    }
}