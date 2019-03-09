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
        public int Foo(int i, IDisposable some) => 5;
        public int Foo(IEnumerable<int> items) => items.Sum();

        public void Bar() { }

        public void Throw() => throw new NotImplementedException();

        public Task Wait() => Task.CompletedTask;

        public Task ThrowAsync() => throw new NotImplementedException();

        public Task<int> FooAsync() => Task.FromResult(9);

        public int Foo(int[][] items) => throw new NotImplementedException();
    }
}