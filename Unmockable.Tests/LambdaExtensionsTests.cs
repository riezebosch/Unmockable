using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests
{
    public static class LambdaExtensionsTests
    {
        [Fact]
        public static void MatchExpressionByMethodName()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo();
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo();

            m.ToMatcher().Should().Be(n.ToMatcher());
        }

        [Fact]
        public static void IncludeVariables()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(4);

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }

        [Fact]
        public static void IncludeCapturedOuterVariables()
        {
            const int i = 3;
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(i);

            m.ToMatcher().Should().Be(n.ToMatcher());
        }

        [Fact]
        public static void IncludeResultFromMethodCall()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(new Func<int>(() => 3)());

            m.ToMatcher().Should().Be(n.ToMatcher());
        }

        [Fact]
        public static void OnlyMethodCallsSupported()
        {
            Expression<Func<int>> m = () => 3;
            var ex = Assert.Throws<NotInstanceMethodCallException>(() => m.ToMatcher());

            ex.Message.Should().Contain(m.ToString());
        }

        [Fact]
        public static void IgnoreNulls()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3, null);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(3, null);

            m.ToMatcher().Should().Be(n.ToMatcher());
        }

        [Fact]
        public static void IgnoreNullsDoesNotInterfereWithOtherMethods()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3, null);
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(4, null);

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }

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
        public static void IgnoreArgument()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.Ignore<int>());
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo(3);

            m.ToMatcher().Should().Be(n.ToMatcher());
        }
        
        [Fact]
        public static void EqualsArgument()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.Equals<Person>(p => p.Age == 32));
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo(new Person { Age = 32 });

            m.ToMatcher().Should().Be(n.ToMatcher());
        }
    }
}