using System;
using System.Collections.Generic;
using System.Linq;

namespace Unmockable.Matchers
{
    internal class CollectionArgument : ValueArgument, IEquatable<CollectionArgument>
    {
        private readonly IEnumerable<IArgumentMatcher> _collection;

        public CollectionArgument(IEnumerable<object> collection) : base(collection) =>
            _collection = collection.Select(ValueMatcherFactory.Create);

        public override int GetHashCode() => throw new NotImplementedException();

        public override string ToString() =>
            $"[{string.Join(", ", _collection)}]";

        public bool Equals(CollectionArgument other) => _collection.SequenceEqual(other._collection);

        public override bool Equals(object obj) => obj is CollectionArgument other && Equals(other);
    }
}