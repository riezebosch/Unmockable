namespace Unmockable.Matchers
{
    internal class IgnoreArgument : IArgumentMatcher
    {
        public override int GetHashCode() => 0;
        public override bool Equals(object obj) => true;
    }
}