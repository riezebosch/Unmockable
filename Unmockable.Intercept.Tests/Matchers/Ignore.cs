using System;
using System.Linq.Expressions;
using FluentAssertions;
using Unmockable.Matchers;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Ignore
    {
        [Fact]
        public static void IgnoreValue()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.Ignore<int>());
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo(3);

            m.ToMatcher().Should().Be(n.ToMatcher());
        }
        
        [Fact]
        public static void String()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.Ignore<int>());
            m.ToMatcher()
                .ToString()
                .Should()
                .Contain("<ignore>");
        }
    }
}