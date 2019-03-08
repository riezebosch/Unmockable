using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}