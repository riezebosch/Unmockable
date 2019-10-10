using System;
using System.Linq.Expressions;
using FluentAssertions;
using Xunit;

namespace Unmockable.Tests.LambdaExtensions
{
    public static class Property
    {
        [Fact]
        public static void EqualsProperty()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Dummy;
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Dummy;

            m.ToMatcher().Should().Be(n.ToMatcher());
        }
        
        [Fact]
        public static void NotEqualsOtherProperty()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Dummy;
            Expression<Func<SomeUnmockableObject, double>> n = x => x.Other;

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }
        
        [Fact]
        public static void NotEqualsMethod()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Dummy;
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo();

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }
    }
}