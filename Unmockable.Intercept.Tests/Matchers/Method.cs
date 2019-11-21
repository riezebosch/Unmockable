using System;
using System.Linq.Expressions;
using FluentAssertions;
using Unmockable.Exceptions;
using Unmockable.Matchers;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Method
    {
        [Fact]
        public static void EqualsMethod()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo();
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo();

            m.ToMatcher().Should().Be(n.ToMatcher());
        }

        [Fact]
        public static void OnlyMethodCallsSupported()
        {
            Expression<Func<int>> m = () => 3;
            m.Invoking(x => x.ToMatcher())
                .Should()
                .Throw<NotSupportedExpressionException>()
                .WithMessage(m.ToString());
        }
        
        [Fact]
        public static void NotEqualsFromOtherType()
        {
            Expression<Func<int>> m = () => int.Parse("a");
            Expression<Func<double>> n = () => double.Parse("a");

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }

    }
}