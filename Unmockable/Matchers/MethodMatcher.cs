using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Matchers
{
    internal class MethodMatcher
    {
        private readonly MethodCallExpression _call;
        private readonly ArgumentsMatcher _arguments;

        public MethodMatcher(LambdaExpression m)
        {
            _call = m.Body as MethodCallExpression ?? throw new NotInstanceMethodCallException(m.ToString());
            _arguments = new ArgumentsMatcher(_call.Arguments);
        }

        public override int GetHashCode() =>
            _call.Method.Name.GetHashCode();

        public override bool Equals(object obj) =>
            obj is MethodMatcher m &&
            _call.Method.Name.Equals(m._call.Method.Name) &&
            _arguments.Equals(m._arguments);

        public override string ToString() =>
            $"{_call.Method.Name}({_arguments})";
    }
}