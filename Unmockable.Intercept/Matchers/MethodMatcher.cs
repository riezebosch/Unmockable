using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class MethodMatcher : IMemberMatcher, IEquatable<MethodMatcher>
    {
        private readonly MethodCallExpression _body;
        private readonly ArgumentsMatcher _arguments;

        public MethodMatcher(MethodCallExpression m)
        {
            _body = m;
            _arguments = new ArgumentsMatcher(_body.Arguments);
        }

        public override int GetHashCode() => HashCode.Combine(_body.Method.DeclaringType, _body.Method.Name);

        public bool Equals(MethodMatcher? other) => 
            other != null
            && _body.Method.DeclaringType == other._body.Method.DeclaringType
            && _body.Method.Name.Equals(other._body.Method.Name) 
            && _arguments.Equals(other._arguments);

        public override bool Equals(object obj) => Equals(obj as MethodMatcher);  

        public override string ToString() => $"{_body.Method.DeclaringType!.Name}.{_body.Method.Name}({_arguments})";
    }
}
