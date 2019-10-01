using System;

namespace Unmockable.Matchers
{
    internal class NullArgument : ValueArgument, IEquatable<NullArgument>
    {
        public override int GetHashCode() => throw new NotImplementedException();

        public override string ToString() => "null";

        public bool Equals(NullArgument other) => true;

        public override bool Equals(object obj) => obj is NullArgument other && Equals(other);

        public NullArgument() : base(null)
        {
        }
    }
}