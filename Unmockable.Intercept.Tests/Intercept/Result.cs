using System.Threading.Tasks;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests.Intercept
{
    public static class Result
    {
        [Fact]
        public static void NoResult()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo());

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Invoking(x => x.Execute(m => m.Foo()))
                .Should()
                .Throw<NoResultsSetupException>()
                .WithMessage("m => m.Foo()");
        }
            
        [Fact]
        public static async Task NoResultAsync()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.FooAsync());

            await mock.As<IUnmockable<SomeUnmockableObject>>()
                .Invoking(x => x.Execute(m => m.FooAsync()))
                .Should()
                .ThrowAsync<NoResultsSetupException>()
                .WithMessage("m => m.FooAsync()");
        }

        [Fact]
        public static void Then()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo())
                .Returns(5)
                .Then(6);

            var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
            sut.Execute(m => m.Foo())
                .Should()
                .Be(5);
                    
            sut.Execute(m => m.Foo())
                .Should()
                .Be(6);
                
            mock.Verify();
        }
            
        [Fact]
        public static void MoreThen()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo())
                .Returns(5)
                .Then(6);

            var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
            sut.Execute(m => m.Foo());
            sut.Execute(m => m.Foo());
            sut.Invoking(x => x.Execute(m => m.Foo()))
                .Should()
                .Throw<OutOfResultsException>()
                .WithMessage("m => m.Foo()");
        }
            
        [Fact]
        public static void AsyncThen()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.FooAsync())
                .Returns(5)
                .Then(6);

            var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
            sut.Execute(m => m.FooAsync())
                .Result
                .Should()
                .Be(5);
                    
            sut.Execute(m => m.FooAsync())
                .Result
                .Should()
                .Be(6);
                
            mock.Verify();
        }
            
        [Fact]
        public static void MoreInvocationsOnSingleResult()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock
                .Setup(m => m.Foo())
                .Returns(3);

            var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
            sut.Execute(m => m.Foo())
                .Should().Be(3);

            sut.Execute(m => m.Foo())
                .Should().Be(3);
        }
    }
}
