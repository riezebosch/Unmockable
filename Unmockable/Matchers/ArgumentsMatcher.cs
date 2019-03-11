using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class ArgumentsMatcher
    {
        private readonly IEnumerable<IArgumentMatcher> _arguments;

        public ArgumentsMatcher(IEnumerable<Expression> arguments) =>
            _arguments = arguments.Select(ToMatcher);

        public override int GetHashCode() =>
            _arguments.Aggregate(0, (hash, x) => hash ^ x.GetHashCode());

        public override string ToString() =>
            string.Join(", ", _arguments);

        public override bool Equals(object obj) =>
            obj is ArgumentsMatcher x && _arguments.SequenceEqual(x._arguments);

        private static IArgumentMatcher ToMatcher(Expression arg) =>
            ArgMatcherFactory.Create(arg) ?? ValueMatcherFactory.Create(arg);
    }
}