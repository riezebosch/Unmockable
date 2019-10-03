using System;

namespace Unmockable.Matchers
{
    internal class IgnoreArgument : IArgumentMatcher, IEquatable<IArgumentMatcher>
    {
        public override int GetHashCode() => throw new NotImplementedException();
        
        public bool Equals(IArgumentMatcher? other) => true;

        public override bool Equals(object obj) => Equals(obj as IArgumentMatcher);

        public override string ToString() => "<ignore>";
    }
}