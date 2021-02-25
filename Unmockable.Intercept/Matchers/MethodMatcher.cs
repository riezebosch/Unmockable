using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class MethodMatcher:
        IMemberMatcher,
        IEquatable<MethodMatcher>
    {
        private readonly MethodCallExpression _body;
        private readonly ArgumentsMatcher _arguments;

        public MethodMatcher(MethodCallExpression m)
        {
            _body = m;
            _arguments = new ArgumentsMatcher(_body.Arguments);
        }

        public override int GetHashCode() =>
            _body.Method.GetHashCode();

        public override bool Equals(object obj) => 
            Equals(obj as MethodMatcher);  

        public bool Equals(MethodMatcher? other) => 
            other != null
            && _body.Method == other._body.Method 
            && _arguments.Equals(other._arguments);

        public override string ToString() => 
            $"{_body.Method.Name}({_arguments})";
    }
}
