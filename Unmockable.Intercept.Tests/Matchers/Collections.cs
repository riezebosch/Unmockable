using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Collections
    {
        [Fact]
        public static void UnwrapCollections() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(new[] {1, 2, 3}))
                .Returns(5)
                .Execute(x => x.Foo(new[] {1, 2, 3}))
                .Should()
                .Be(5);

        [Fact]
        public static void UnwrapNestedCollections() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(new[] {new[] {1, 2}, new[] {3, 4}}))
                .Returns(5)
                .Execute(x => x.Foo(new[] {new[] {1, 2}, new[] {3, 4}}))
                .Should()
                .Be(5);

        [Fact]
        public static void OnlyEqualsCollections() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(new[] {1, 2, 3}))
                .Returns(5)
                .As<IIntercept<SomeUnmockableObject>>()
                .Invoking(m => m.Execute(x => x.Foo(3)))
                .Should()
                .Throw<SetupNotFoundException>();
    }
}