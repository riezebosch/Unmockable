using System;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Value
    {
        [Fact]
        public static void EqualsValue() => Interceptor
            .For<SomeUnmockableObject>()
            .Setup(x => x.Foo(3))
            .Returns(5)
            .Execute(y => y.Foo(3))
            .Should()
            .Be(5);

        [Fact]
        public static void NotEqualsValue() => Interceptor
            .For<SomeUnmockableObject>()
            .Setup(x => x.Foo(3))
            .Returns(5)
            .Invoking(x => x.Execute(y => y.Foo(4)))
            .Should()
            .Throw<SetupNotFoundException>();

        [Fact]
        public static void Null() => Interceptor
            .For<SomeUnmockableObject>()
            .Setup(x => x.Foo(3, new Person()))
            .Returns(5)
            .Invoking(x => x.Execute(y => y.Foo(3, null)))
            .Should()
            .Throw<SetupNotFoundException>();
  
        [Fact]
        public static void EqualsCapturedOuterVariable()
        {
            const int i = 3;
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(3))
                .Returns(5)
                .Execute(x => x.Foo(i))
                .Should()
                .Be(5);
        }

        [Fact]
        public static void EqualsResult()
        {
            Func<int> func = () => 3;
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(3))
                .Returns(5)
                .Execute(x => x.Foo(func()))
                .Should()
                .Be(5);
        }
    }
}