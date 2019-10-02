using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal static class ValueMatcherFactory
    {
        public static IArgumentMatcher Create(Expression arg) => Create(Expression.Lambda(arg).Compile().DynamicInvoke());

        public static IArgumentMatcher Create(object value) => value switch
            {
                null => new NullArgument() as IArgumentMatcher,
                IEnumerable collection => new CollectionArgument(collection.Cast<object>()),
                _ => new ValueArgument(value)
            };
    }
}