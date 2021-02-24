using System;
using System.Diagnostics.CodeAnalysis;

namespace Unmockable.Matchers
{
    internal class IgnoreArgument : IArgumentMatcher, IEquatable<IArgumentMatcher>
    {
        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => throw new InvalidOperationException();
        
        public bool Equals(IArgumentMatcher? other) => true;

        public override bool Equals(object obj) => Equals(obj as IArgumentMatcher);

        public override string ToString() => "ignore";
    }
}