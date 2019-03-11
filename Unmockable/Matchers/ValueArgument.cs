namespace Unmockable.Matchers
{
    internal class ValueArgument : IArgumentMatcher
    {
        public object Value { get; }
        public ValueArgument(object value) => Value = value;

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();

        public override bool Equals(object obj) =>
            obj is ValueArgument x && Value.Equals(x.Value);
    }
}