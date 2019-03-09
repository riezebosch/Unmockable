using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests
{
    public class LambdaExtensionsTests
    {
        public class ToKey
        {
            [Fact]
            public void MatchExpressionByMethodName()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo();
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo();

                m.ToMatcher().Should().Be(n.ToMatcher());
            }

            [Fact]
            public void IncludeVariables()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(4);

                m.ToMatcher().Should().NotBe(n.ToMatcher());
            }

            [Fact]
            public void IncludeCapturedOuterVariables()
            {
                const int i = 3;
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(i);

                m.ToMatcher().Should().Be(n.ToMatcher());
            }

            [Fact]
            public void IncludeResultFromMethodCall()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(new Func<int>(() => 3)());

                m.ToMatcher().Should().Be(n.ToMatcher());
            }

            [Fact]
            public void OnlyMethodCallsSupported()
            {
                Expression<Func<int>> m = () => 3;
                var ex = Assert.Throws<NotInstanceMethodCallException>(() => m.ToMatcher());

                ex.Message.Should().Contain(m.ToString());
            }

            [Fact]
            public void IgnoreNulls()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3, null);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(3, null);

                m.ToMatcher().Should().Be(n.ToMatcher());
            }
            
            [Fact]
            public void IgnoreNullsDoesNotInterfereWithOtherMethods()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3, null);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(4, null);

                m.ToMatcher().Should().NotBe(n.ToMatcher());
            }

            [Fact]
            public void UnwrapCollections()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(new[] { 1, 2, 3 });
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(new List<int>{ 1, 2, 3});

                m.ToMatcher().Should().Be(n.ToMatcher());
            }
            
            [Fact]
            public void UnwrapNestedCollections()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(new[] { new[] { 1, 2 }, new[] { 3, 4} });
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(new[] { new[] { 1, 2 }, new[] { 3, 4} });

                m.ToMatcher().Should().Be(n.ToMatcher());
            }
        }
    }
}