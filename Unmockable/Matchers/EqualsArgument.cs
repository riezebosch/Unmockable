using System;
using System.Linq.Expressions;

namespace Unmockable.Matchers
{
    internal class EqualsArgument : IArgumentMatcher
    {
        private readonly Delegate _pred;
        
        public EqualsArgument(Expression pred) => _pred = (Delegate) Expression.Lambda(pred).Compile().DynamicInvoke();
        
        public override int GetHashCode() => throw new NotImplementedException();

        public override bool Equals(object obj) => obj is ValueArgument arg && (bool) _pred.DynamicInvoke(arg.Value);

        public override string ToString() => _pred.ToString();
    }
}