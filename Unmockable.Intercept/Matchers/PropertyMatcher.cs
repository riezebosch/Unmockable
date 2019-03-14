using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class PropertyMatcher : IUnmockableMatcher, IEquatable<PropertyMatcher>
    {
        private readonly MemberExpression _expression;

        public PropertyMatcher(MemberExpression expression)
        {
            _expression = expression;
        }

        public override int GetHashCode() => _expression.Member.GetHashCode();
        public bool Equals(PropertyMatcher other) => _expression.Member.Name == other._expression.Member.Name;

        public override bool Equals(object obj) => obj is PropertyMatcher other && Equals(other);

    }
}