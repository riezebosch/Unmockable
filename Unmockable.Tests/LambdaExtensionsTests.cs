using System;
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

                m.ToKey().Should().Be(n.ToKey());
            }

            [Fact]
            public void IncludeVariables()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(4);

                m.ToKey().Should().NotBe(n.ToKey());
            }

            [Fact]
            public void IncludeCapturedOuterVariables()
            {
                const int i = 3;
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(i);

                m.ToKey().Should().Be(n.ToKey());
            }

            [Fact]
            public void IncludeResultFromMethodCall()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(new Func<int>(() => 3)());

                m.ToKey().Should().Be(n.ToKey());
            }

            [Fact]
            public void OnlyMethodCallsSupported()
            {
                Expression<Func<int>> m = () => 3;
                var ex = Assert.Throws<NotInstanceMethodCallException>(() => m.ToKey());

                ex.Message.Should().Contain(m.ToString());
            }

            [Fact]
            public void IgnoreNulls()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3, null);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(3, null);

                m.ToKey().Should().Be(n.ToKey());
            }
            
            [Fact]
            public void IgnoreNullsDoesNotInterfereWithOtherMethods()
            {
                Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(3, null);
                Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(4, null);

                m.ToKey().Should().NotBe(n.ToKey());
            }
        }
    }
}