using System;
using System.Collections.Generic;
using FluentAssertions;
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
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup( y => y.Foo(Arg.With<int>(x => args.Push(x))))
                .Returns(3)
                .Execute( y => y.Foo(3))
                .Should()
                .Be(3);
            
            args.Should()
                .BeEquivalentTo(3);
        }
        
        [Fact]
        public static void Action() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup( y => y.Foo(Arg.With<int>(x => x.Should().Be(3, ""))))
                .Returns(3)
                .Execute( y => y.Foo(3))
                .Should()
                .Be(3);

        [Fact]
        public static void Exception()=>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup( y => y.Foo(Arg.With<int>(x => x.Should().NotBe(3, ""))))
                .Returns(3)
                .Invoking(x => x.Execute( y => y.Foo(3)))
                .Should()
                .Throw<XunitException>();
        
        [Fact]
        public static void Null() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(3, Arg.With<Person>(y => y.Should().BeNull("message"))))
                .Returns(5)
                .Execute(x => x.Foo(3, null))
                .Should()
                .Be(5);

        [Fact]
        public static void Collection() => 
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(Arg.With<IEnumerable<int>>(y => y.Should().BeEquivalentTo(new[] { 1, 2, 3 }, ""))))
                .Returns(5)
                .Execute(x => x.Foo(new[] { 1, 2, 3}))
                .Should()
                .Be(5);

        [Fact]
        public static void Other() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(3, Arg.With<Person>(y => y.Should().BeNull("message"))))
                .Returns(5)
                .Invoking(x => x.Execute(y => y.Foo(3, Arg.Ignore<Person>())))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Arg.With only supports*");
        
        [Fact]
        public static void String() => 
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(y => y.Foo(Arg.With<int>(x => x.Should().Be(3, ""))))
                .ToString()
                .Should()
                .Contain("x => x.Should().Be(");
    }
}