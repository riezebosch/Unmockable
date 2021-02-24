using System;
using System.Linq.Expressions;
using FluentAssertions;
using Unmockable.Matchers;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Value
    {
        [Fact]
        public static void EqualsValue()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(4);

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }

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