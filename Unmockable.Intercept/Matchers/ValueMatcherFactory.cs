using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal static class ValueMatcherFactory
    {
        public static IArgumentMatcher Create(Expression arg) => Create(Expression.Lambda(arg).Compile().DynamicInvoke());

        public static IArgumentMatcher Create(object value)
        {
            switch (value)
            {
                case null:
                    return new NullArgument();
                case IEnumerable collection:
                    return new CollectionArgument(collection.Cast<object>());
                default:
                    return new ValueArgument(value);
            }
        }
    }
}