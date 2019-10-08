using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class PropertyMatcher : IUnmockableMatcher, IEquatable<PropertyMatcher>
    {
        private readonly MemberExpression _body;

        public PropertyMatcher(MemberExpression body) => _body = body;
        public bool Equals(PropertyMatcher? other) => other != null && _body.Member.Name == other._body.Member.Name;
        public override int GetHashCode() => _body.Member.Name.GetHashCode();
        public override bool Equals(object obj) => Equals(obj as PropertyMatcher);
        public override string ToString() => $"{_body.Member.DeclaringType.Name}.{_body.Member.Name}";
    }
}