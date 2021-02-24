using FluentAssertions;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Ignore
    {
        [Fact]
        public static void IgnoreValue() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(Arg.Ignore<int>()))
                .Returns(5)
                .Execute(x => x.Foo(3))
                .Should()
                .Be(5);

        [Fact]
        public static void String() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(Arg.Ignore<int>()))
                .ToString()
                .Should()
                .Contain("Foo(ignore): no results setup");
    }
}