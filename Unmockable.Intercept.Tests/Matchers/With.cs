using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Unmockable.Matchers;
using Xunit;
using Xunit.Sdk;

namespace Unmockable.Tests.Matchers
{
    public static class With
    {
        [Fact]
        public static void Execute()
        {
            var args = new Stack<int>();
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.With<int>(x => args.Push(x)));
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(3);
            
            m.ToMatcher().Should().Be(n.ToMatcher());
            args.Should()
                .BeEquivalentTo(3);
        }
        
        [Fact]
        public static void Action()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.With<int>(x => x.Should().Be(3, "")));
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(3);
            
            m.ToMatcher().Should().Be(n.ToMatcher());
        }
        
        [Fact]
        public static void Exception()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.With<int>(x => x.Should().Be(3, "")));
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(4);
            
            m.ToMatcher().Invoking(x => x.Equals(n.ToMatcher()))
                .Should()
                .Throw<XunitException>();
        }
        
        [Fact]
        public static void String()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.With<int>(x => x.Should().Be(3, "")));
            m.ToMatcher()
                .ToString()
                .Should()
                .Contain("x => x.Should().Be(");
        }
    }
}