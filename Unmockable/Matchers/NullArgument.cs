using System;

namespace Unmockable.Matchers
{
    internal class NullArgument : ValueArgument
    {
        public override int GetHashCode() => throw new NotImplementedException();

        public override string ToString() => "null";

        public override bool Equals(object obj) => obj is NullArgument;

        public NullArgument() : base(null)
        {
        }
    }
}