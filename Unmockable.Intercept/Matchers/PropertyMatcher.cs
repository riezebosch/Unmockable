using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class PropertyMatcher : IMemberMatcher, IEquatable<PropertyMatcher>
    {
        private readonly MemberExpression _body;

        public PropertyMatcher(MemberExpression body) =>
            _body = body;
        
        public override int GetHashCode() =>
            _body.Member.GetHashCode();
        
        public override bool Equals(object obj) =>
            Equals(obj as PropertyMatcher);

        public bool Equals(PropertyMatcher? other) => 
            other != null
            && _body.Member == other._body.Member;
        
        public override string ToString() =>
            _body.Member.Name;
    }
}
