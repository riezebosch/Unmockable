using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class MethodMatcher : IUnmockableMatcher
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

        public override bool Equals(object obj) =>
            obj is MethodMatcher rhs &&
            _body.Method.DeclaringType == rhs._body.Method.DeclaringType &&
            _body.Method.Name.Equals(rhs._body.Method.Name) &&
            _arguments.Equals(rhs._arguments);

        public override string ToString() =>
            $"{_body.Method.Name}({_arguments})";
    }
}