using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Unmockable.Matchers;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Collections
    {
        [Fact]
        public static void UnwrapCollections()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(new[] {1, 2, 3});
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(new List<int> {1, 2, 3});

            m.ToMatcher().Should().Be(n.ToMatcher());
        }

        [Fact]
        public static void UnwrapNestedCollections()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(new[] {new[] {1, 2}, new[] {3, 4}});
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(new[] {new[] {1, 2}, new[] {3, 4}});

            m.ToMatcher().Should().Be(n.ToMatcher());
        }
        
        [Fact]
        public static void OnlyEqualsCollections()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(new[] {1, 2, 3});
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(Arg.Ignore<IEnumerable<int>>());

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }
    }
}