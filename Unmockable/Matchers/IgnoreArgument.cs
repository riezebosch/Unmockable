using System;

namespace Unmockable.Matchers
{
    internal class IgnoreArgument : IArgumentMatcher
    {
        public override int GetHashCode() => throw new NotImplementedException();
        public override bool Equals(object obj) => true;
    }
}