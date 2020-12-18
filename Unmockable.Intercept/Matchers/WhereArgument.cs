using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class WhereArgument : IArgumentMatcher, IEquatable<ValueArgument>
    {
        private readonly LambdaExpression _pred;
        
        public WhereArgument(LambdaExpression pred) => _pred = pred;

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => throw new InvalidOperationException();

        public bool Equals(ValueArgument? other) =>
            other != null
            && (bool) _pred.Compile().DynamicInvoke(other.Value);

        public override bool Equals(object obj) =>
            Equals(obj as ValueArgument); 

        public override string ToString() =>
            _pred.ToString();
    }
}