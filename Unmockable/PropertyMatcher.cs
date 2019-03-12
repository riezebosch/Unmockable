using System.Linq.Expressions;

namespace Unmockable
{
    internal class PropertyMatcher : IUnmockableMatcher
    {
        private readonly MemberExpression _expression;

        public PropertyMatcher(MemberExpression expression)
        {
            _expression = expression;
        }

        public override int GetHashCode() => _expression.Member.GetHashCode();
        public override bool Equals(object obj) => 
            obj is PropertyMatcher rhs && 
            _expression.Member.Name == rhs._expression.Member.Name;
    }
}