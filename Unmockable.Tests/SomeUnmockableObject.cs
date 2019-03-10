using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unmockable.Tests
{
    public class SomeUnmockableObject
    {
        public int Foo() => 3;
        public int Foo(int i) => i;
        public int Foo(int i, IDisposable _) => i;
        public int Foo(IEnumerable<int> items) => items.Sum();
        public int Foo(IEnumerable<IEnumerable<int>> _) => throw new NotImplementedException();
        public Task<int> FooAsync() => Task.FromResult(9);
        public void Bar() => throw new NotImplementedException();
        public Task BarAsync() => Task.CompletedTask;
    }
}