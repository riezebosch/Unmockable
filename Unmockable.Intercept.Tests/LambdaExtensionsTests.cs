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
             m.Invoking(x => x.ToMatcher())
                 .Should()
                 .Throw<NotSupportedExpressionException>()
                 .WithMessage(m.ToString());
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
        public static void NullDoesNotEqualValue()
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
        public static void CollectionDoNotMatchIgnore()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Foo(new[] {1, 2, 3});
            Expression<Func<SomeUnmockableObject, int>> n = y => y.Foo(Arg.Ignore<IEnumerable<int>>());

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }

        [Fact]
        public static void IgnoreArgument()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.Ignore<int>());
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo(3);

            m.ToMatcher().Should().Be(n.ToMatcher());
        }
        
        [Fact]
        public static void IgnoreArgumentString()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(Arg.Ignore<int>());
            m.ToMatcher()
                .ToString()
                .Should()
                .Contain("<ignore>");
        }

        [Fact]
        public static void EqualsArgument()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(3, Arg.Where<Person>(p => p.Age == 32));
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo(3, new Person {Age = 32});

            m.ToMatcher().Should().Be(n.ToMatcher());
        }
        
        [Fact]
        public static void EqualsArgumentDoesNotEqualIgnoreArgument()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(3, Arg.Where<Person>(p => p.Age == 32));
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo(3, Arg.Ignore<Person>());

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }

        [Fact]
        public static void EqualsArgumentString()
        {
            Expression<Func<SomeUnmockableObject, int>> m = y => y.Foo(3, Arg.Where<Person>(p => p.Age == 32));
            m.ToMatcher()
                .ToString()
                .Should()
                .Contain("p => (p.Age == 32)");
        }

        [Fact]
        public static void UniquePerType()
        {
            Expression<Func<int>> m = () => int.Parse("a");
            Expression<Func<double>> n = () => double.Parse("a");

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }
        
        [Fact]
        public static void Property()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Dummy;
            Expression<Func<SomeUnmockableObject, double>> n = x => x.Other;

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }
        
        [Fact]
        public static void PropertyNotEqualsMethod()
        {
            Expression<Func<SomeUnmockableObject, int>> m = x => x.Dummy;
            Expression<Func<SomeUnmockableObject, int>> n = x => x.Foo();

            m.ToMatcher().Should().NotBe(n.ToMatcher());
        }
    }
}