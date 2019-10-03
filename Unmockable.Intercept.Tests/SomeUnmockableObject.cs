using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unmockable.Tests
{
    public class SomeUnmockableObject
    {
        public int Dummy { get; set; }

        public int Foo() => Dummy;
        public int Foo(int i) => Dummy = i;
        public int Foo(IEnumerable<int> items) => Dummy = items.Sum();
        public int Foo(IEnumerable<IEnumerable<int>> _) => Dummy;
        public int Foo(int i, Person p) => Dummy = p.Age + i;
        public Task<int> FooAsync() => Task.FromResult(Dummy);
        public Task<int> FooAsync(int i) => Task.FromResult(i);
        public void Bar(int i) => Dummy = i;
        public async Task BarAsync() => await FooAsync();
    }
}