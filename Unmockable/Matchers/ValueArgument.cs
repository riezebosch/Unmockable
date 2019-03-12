using System;

namespace Unmockable.Matchers
{
    internal class ValueArgument : IArgumentMatcher, IEquatable<ValueArgument>
    {
        public object Value { get; }
        public ValueArgument(object value) => Value = value;

        public override int GetHashCode() => throw new NotImplementedException();

        public override string ToString() => Value.ToString();

        public bool Equals(ValueArgument other) => Value.Equals(other.Value);

        public override bool Equals(object obj) => obj is ValueArgument other && Equals(other);
    }
}