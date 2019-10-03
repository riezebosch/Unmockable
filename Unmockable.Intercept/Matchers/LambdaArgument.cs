using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class LambdaArgument : IArgumentMatcher, IEquatable<ValueArgument>
    {
        private readonly Expression _pred;
        
        public LambdaArgument(Expression pred) => _pred = pred;
        
        public override int GetHashCode() => throw new NotImplementedException();

        public bool Equals(ValueArgument? other) => other != null && (bool) ((Delegate) Expression.Lambda(_pred).Compile().DynamicInvoke()).DynamicInvoke(other.Value);

        public override bool Equals(object obj) => Equals(obj as ValueArgument); 

        public override string ToString() => _pred.ToString();
    }
}