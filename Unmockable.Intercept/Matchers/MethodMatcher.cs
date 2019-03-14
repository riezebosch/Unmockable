using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class MethodMatcher : IUnmockableMatcher, IEquatable<MethodMatcher>
    {
        private readonly MethodCallExpression _body;
        private readonly ArgumentsMatcher _arguments;

        public MethodMatcher(MethodCallExpression m)
        {
            _body = m;
            _arguments = new ArgumentsMatcher(_body.Arguments);
        }

        public override int GetHashCode() =>
            _body.Method.DeclaringType.GetHashCode() ^ _body.Method.Name.GetHashCode();

        public bool Equals(MethodMatcher other) => _body.Method.DeclaringType == other._body.Method.DeclaringType &&
                                                   _body.Method.Name.Equals(other._body.Method.Name) &&
                                                   _arguments.Equals(other._arguments);

        public override bool Equals(object obj) => obj is MethodMatcher other && Equals(other);  

        public override string ToString() =>
            $"{_body.Method.Name}({_arguments})";
    }
}