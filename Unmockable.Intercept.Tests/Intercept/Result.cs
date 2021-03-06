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
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo());

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Invoking(x => x.Execute(m => m.Foo()))
                .Should()
                .Throw<UninitializedException>()
                .WithMessage("Foo()");
        }
            
        [Fact]
        public static async Task NoResultAsync()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.FooAsync());

            await mock.As<IUnmockable<SomeUnmockableObject>>()
                .Invoking(x => x.Execute(m => m.FooAsync()))
                .Should()
                .ThrowAsync<UninitializedException>()
                .WithMessage("FooAsync()");
        }

        [Fact]
        public static void Then()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo())
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
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo())
                .Returns(5)
                .Then(6);

            var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
            sut.Execute(m => m.Foo());
            sut.Execute(m => m.Foo());
            sut.Invoking(x => x.Execute(m => m.Foo()))
                .Should()
                .Throw<OutOfResultsException>()
                .WithMessage("Foo()");
        }
            
        [Fact]
        public static void AsyncThen()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.FooAsync())
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
            var mock = Interceptor
                .For<SomeUnmockableObject>()
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
