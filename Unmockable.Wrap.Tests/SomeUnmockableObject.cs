using System.Threading.Tasks;

namespace Unmockable.Wrap.Tests
{
    public class SomeUnmockableObject
    {
        public int Dummy { get; set; }

        public int Foo() => Dummy;
        public int Foo(int i) => Dummy = i;
        public Task<int> FooAsync() => Task.FromResult(Dummy);
        public void Bar(int i) => Dummy = i;

        public SomeUnmockableObject Nested() => new SomeUnmockableObject();
        public Task<SomeUnmockableObject> NestedAsync() => Task.Run(() => new SomeUnmockableObject());
    }
}