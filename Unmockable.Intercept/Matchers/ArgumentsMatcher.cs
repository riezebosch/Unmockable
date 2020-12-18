using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class ArgumentsMatcher : IEquatable<ArgumentsMatcher>
    {
        private readonly IEnumerable<IArgumentMatcher> _arguments;

        public ArgumentsMatcher(IEnumerable<Expression> arguments) => 
            _arguments = arguments.Select(ToMatcher);

        public override string ToString() => 
            string.Join(", ", _arguments);

        public bool Equals(ArgumentsMatcher other) =>
            _arguments!.SequenceEqual(other._arguments);

        private static IArgumentMatcher ToMatcher(Expression arg) =>
            ArgMatcherFactory.Create(arg) ?? ValueMatcherFactory.Create(arg);
    }
}