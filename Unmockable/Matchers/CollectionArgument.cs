using System.Collections.Generic;
using System.Linq;

namespace Unmockable.Matchers
{
    internal class CollectionArgument : ValueArgument
    {
        private readonly IEnumerable<IArgumentMatcher> _collection;

        public CollectionArgument(IEnumerable<object> collection) : base(collection) =>
            _collection = collection.Select(ValueMatcherFactory.Create);

        public override int GetHashCode() =>
            _collection.Aggregate(0, (hash, item) => hash ^ item.GetHashCode());

        public override string ToString() =>
            $"[{string.Join(", ", _collection)}]";

        public override bool Equals(object obj) =>
            obj is CollectionArgument x && _collection.SequenceEqual(x._collection);
    }
}