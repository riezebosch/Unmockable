using System.IO;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests.Intercept
{
    public static class Verify
    {
        [Fact]
        public static void Executed()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo())
                .Returns(5);
                
            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo());

            mock.Verify();
        }

        [Fact]
        public static void NotExecuted()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo())
                .Returns(3);

            mock.Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("m => m.Foo(): 3");
        }
            
        [Fact]
        public static void MultipleNotExecuted()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo())
                .Returns(3)
                .Then(4);

            mock.Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("m => m.Foo(): 3, 4");
        }
            
        [Fact]
        public static void MultipleNotAllExecuted()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo())
                .Returns(3)
                .Then(4);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo());

            mock.Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("m => m.Foo(): 4");
        }
            
        [Fact]
        public static void ExceptionNotExecuted()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo())
                .Throws<FileNotFoundException>();

            mock.Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("m => m.Foo(): FileNotFoundException");
        }
            
        [Fact]
        public static void ActionNotExecuted()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Bar(Arg.Ignore<int>()));

            mock.Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("m => m.Bar(Ignore()): void");
        }
            
        [Fact]
        public static void AsyncFuncNotExecuted()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.FooAsync(3))
                .Returns(5);

            mock.Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("m => m.FooAsync(3): 5");
        }
            
        [Fact]
        public static void AsyncActionNotExecuted()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.BarAsync());

            mock.Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("m => m.BarAsync(): Task");
        }
            
        [Fact]
        public static void PropertyNotExecuted()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Dummy)
                .Returns(4);

            mock.Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("m => m.Dummy: 4");
        }

        [Fact]
        public static void NoResultNotExecuted()
        {
            var mock = Interceptor.For<SomeUnmockableObject>();
            mock.Setup(m => m.Foo());

            mock.Invoking(x => x.Verify())
                .Should()
                .Throw<NotExecutedException>()
                .WithMessage("m => m.Foo(): no results setup");
        }
    }
}