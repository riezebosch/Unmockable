using System;
using System.Linq.Expressions;
using FluentAssertions;
using Unmockable.Matchers;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Where
    {
        [Fact]
        public static void Matches()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(3, Arg.Where<Person>(p => p.Age == 32));
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo(3, new Person {Age = 32});

            m.ToMatcher().Should().Be(n.ToMatcher());
        }
        
        [Fact]
        public static void NotMatches()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(3, Arg.Where<Person>(p => p.Age == 32));
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo(3, Arg.Ignore<Person>());

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }

        [Fact]
        public static void String()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(3, Arg.Where<Person>(p => p.Age == 32));
            m.ToMatcher()
                .ToString()
                .Should()
                .Contain("p => (p.Age == 32)");
        }
    }
}