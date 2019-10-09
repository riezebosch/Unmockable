using System;
using System.Diagnostics.CodeAnalysis;

namespace Unmockable.Matchers
{
    internal class NullArgument : IArgumentMatcher, IEquatable<NullArgument>
    {
        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => throw new InvalidOperationException();

        public override string ToString() => "null";

        public bool Equals(NullArgument other) => true;

        public override bool Equals(object obj) => obj is NullArgument other && Equals(other);
    }
}