using System;

namespace Unmockable.Tests
{
    public class SomeUnmockableObject
    {
        public int Foo() => 3;
    
        public int Foo(int i) => i;
        public int Foo(int i, IDisposable some) => 5;

        public void Bar()
        {
        }

        public void Throw()
        {
            throw new NotImplementedException();
        }
    }
}