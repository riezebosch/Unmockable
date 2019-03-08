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

        public void Bar()
        {
        }

        public void Throw()
        {
            throw new NotImplementedException();
        }

        public Task<int> Wait() => Task.FromResult(4);

        public Task<int> ThrowAsync() => throw new NotImplementedException();
    }
}