using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class EqualsArgument : IArgumentMatcher, IEquatable<ValueArgument>
    {
        private readonly Expression _pred;
        
        public EqualsArgument(Expression pred) => _pred = pred;
        
        public override int GetHashCode() => throw new NotImplementedException();

        public bool Equals(ValueArgument other) => (bool) ((Delegate) Expression.Lambda(_pred).Compile().DynamicInvoke()).DynamicInvoke(other.Value);

        public override bool Equals(object obj) => obj is ValueArgument other && Equals(other); 

        public override string ToString() => _pred.ToString();
    }
}