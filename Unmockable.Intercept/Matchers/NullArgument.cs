using System;

namespace Unmockable.Matchers
{
    internal class NullArgument : IArgumentMatcher, IEquatable<NullArgument>
    {
        public override int GetHashCode() => throw new InvalidOperationException();

        public override string ToString() => "null";

        public bool Equals(NullArgument other) => true;

        public override bool Equals(object obj) => obj is NullArgument other && Equals(other);
    }
}