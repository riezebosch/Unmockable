using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Unmockable.Tests
{
    public class SomeUnmockableObject
    {
        public int Dummy { get; set; }

        public int Foo() => Dummy;
        public int Foo(int i) => Dummy = i;
        public int Foo(int i, IDisposable _) => Dummy;
        public int Foo(IEnumerable<int> items) => Dummy = items.Sum();
        public int Foo(IEnumerable<IEnumerable<int>> _) => Dummy;
        public int Foo(Person p) => Dummy;
        public Task<int> FooAsync() => Task.FromResult(Dummy);
        public void Bar(int i) => Dummy = 10;
        public async Task BarAsync() => await FooAsync();
        public static void ThrowStatic() => throw new FileNotFoundException();
    }
}