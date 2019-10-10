using System;
using System.Linq.Expressions;
using FluentAssertions;
using Xunit;

namespace Unmockable.Tests.LambdaExtensions
{
    public static class Null
    {
        [Fact]
        public static void EqualsNull()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3, null);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(3, null);

            m.ToMatcher().Should().Be(n.ToMatcher());
        }

        [Fact]
        public static void NotInterfereWithOtherArguments()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3, null);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(4, null);

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }
        
        [Fact]
        public static void NotEqualValue()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3, null);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(3, new Person());

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }
        
        [Fact]
        public static void ValueDoesNotEqualNull()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(3, new Person());
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo(3, null);

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }
    }
}