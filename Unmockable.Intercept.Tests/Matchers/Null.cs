using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Null
    {
        [Fact]
        public static void EqualsNull() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(3, null))
                .Returns(5)
                .Execute(x => x.Foo(3, null))
                .Should()
                .Be(5);

        
        
        [Fact]
        public static void ValueDoesNotEqualNull() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(3, null))
                .Returns(5)
                .Invoking(m => m.Execute(x => x.Foo(3, new Person())))
                .Should()
                .Throw<SetupNotFoundException>();
    }
}