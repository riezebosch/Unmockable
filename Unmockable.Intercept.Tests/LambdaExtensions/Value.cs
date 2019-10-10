using System;
using System.Linq.Expressions;
using FluentAssertions;
using Xunit;

namespace Unmockable.Tests.LambdaExtensions
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
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(i);

            m.ToMatcher().Should().Be(n.ToMatcher());
        }

        [Fact]
        public static void EqualsResult()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(new Func<int>(() => 3)());

            m.ToMatcher().Should().Be(n.ToMatcher());
        }
    }
}