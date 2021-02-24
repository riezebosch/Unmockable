using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests.Matchers
{
    public static class Where
    {
        [Fact]
        public static void Matches() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(3, Arg.Where<Person>(p => p.Age == 32)))
                .Returns(5)
                .Execute(x => x.Foo(3, new Person {Age = 32}))
                .Should()
                .Be(5);

        [Fact]
        public static void NotMatches() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(3, Arg.Where<Person>(p => p.Age == 32)))
                .Returns(5)
                .Invoking(x => x.Execute(y => y.Foo(3, new Person())))
                .Should()
                .Throw<SetupNotFoundException>()
                .WithMessage("Foo(3, Unmockable.Tests.Person)");

        [Fact]
        public static void Verify() =>
            Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Foo(3, Arg.Where<Person>(p => p.Age == 32)))
                .Returns(5)
                .Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("Foo(3, p => (p.Age == 32)): 5");
    }
}