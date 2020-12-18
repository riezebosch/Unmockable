using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Unmockable.Matchers
{
    internal class WithArgument : IArgumentMatcher, IEquatable<ValueArgument>
    {
        private readonly LambdaExpression _pred;
        
        public WithArgument(LambdaExpression pred) => _pred = pred;

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => throw new InvalidOperationException();

        public bool Equals(ValueArgument other)
        {
            InvokeAndUnwrap(other);
            return true;
        }

        public override bool Equals(object obj) =>
            Equals(obj as ValueArgument ?? throw new ArgumentException($"only supports {nameof(ValueArgument)}", nameof(obj))); 

        public override string ToString() => _pred.ToString();

        private void InvokeAndUnwrap(ValueArgument other)
        {
            try
            {
                var invoke = _pred.Compile();
                invoke.DynamicInvoke(other.Value);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException!;
            }
        }
    }
}