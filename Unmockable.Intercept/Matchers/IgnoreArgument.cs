using System;

namespace Unmockable.Matchers
{
    internal class IgnoreArgument : IArgumentMatcher, IEquatable<IgnoreArgument>
    {
        public override int GetHashCode() => throw new NotImplementedException();
        
        public bool Equals(IgnoreArgument other) => true;

        public override bool Equals(object obj) => obj is IArgumentMatcher;
    }
}