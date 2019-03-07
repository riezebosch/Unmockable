namespace Unmockable.Tests
{
    public class SomeUnmockableObject
    {
        public int Foo() => 3;
    
        public int Foo(int i) => i;
    }
}