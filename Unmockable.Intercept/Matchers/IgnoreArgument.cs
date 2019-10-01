using System;

namespace Unmockable.Matchers
{
    internal class IgnoreArgument : IArgumentMatcher, IEquatable<IArgumentMatcher>
    {
        public override int GetHashCode() => throw new NotImplementedException();
        
        public bool Equals(IArgumentMatcher other) => true;

        public override bool Equals(object obj) => obj is IArgumentMatcher other && Equals(other);

        public override string ToString() => "<ignore>";
    }
}