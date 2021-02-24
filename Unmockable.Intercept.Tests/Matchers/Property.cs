using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Property
    {
        [Fact]
        public static void EqualsProperty() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup( x => x.Dummy)
                .Returns(5)
                .Execute( x => x.Dummy)
                .Should()
                .Be(5);

        [Fact]
        public static void NotEqualsOtherProperty() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Dummy)
                .Returns(5)
                .Invoking(m => m.Execute(x => x.Other))
                .Should()
                .Throw<SetupNotFoundException>();

        [Fact]
        public static void NotEqualsMethod() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Dummy)
                .Returns(5)
                .Invoking(m => m.Execute(x => x.Foo()))
                .Should()
                .Throw<SetupNotFoundException>();
    }
}
