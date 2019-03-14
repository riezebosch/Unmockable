using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class EqualsArgument : IArgumentMatcher, IEquatable<ValueArgument>
    {
        private readonly Delegate _pred;
        
        public EqualsArgument(Expression pred) => _pred = (Delegate) Expression.Lambda(pred).Compile().DynamicInvoke();
        
        public override int GetHashCode() => throw new NotImplementedException();

        public bool Equals(ValueArgument other) => (bool) _pred.DynamicInvoke(other.Value);

        public override bool Equals(object obj) => obj is ValueArgument other && Equals(other); 

        public override string ToString() => _pred.ToString();
    }
}