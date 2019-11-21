namespace Unmockable.Matchers
{
    internal class NullArgument : IArgumentMatcher
    {
        private NullArgument()
        {
        }
        
        public override string ToString() => "null";

        public static IArgumentMatcher Single { get; } = new NullArgument();
    }
}