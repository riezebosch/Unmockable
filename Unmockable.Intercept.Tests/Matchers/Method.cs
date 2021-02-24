using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Method
    {
        [Fact]
        public static void EqualsMethod() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo())
                .Returns(5)
                .Execute(x => x.Foo())
                .Should()
                .Be(5);

        [Fact]
        public static void OnlyMethodCallsSupported() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Invoking(m => m.Setup(x => 3))
                .Should()
                .Throw<UnsupportedExpressionException>()
                .WithMessage("x => 3");

        [Fact]
        public static void NotEqualsFromOtherType() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => int.Parse("a"))
                .Returns(5)
                .Invoking(m => m.Execute(x => double.Parse("a")))
                .Should()
                .Throw<SetupNotFoundException>();
    }
}