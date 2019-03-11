namespace Unmockable.Matchers
{
    internal class NullArgument : ValueArgument
    {
        public override int GetHashCode() => 0;

        public override string ToString() => "null";

        public override bool Equals(object obj) => obj is NullArgument;

        public NullArgument() : base(null)
        {
        }
    }
}