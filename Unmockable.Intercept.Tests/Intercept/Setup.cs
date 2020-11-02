using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;
using Xunit.Sdk;

namespace Unmockable.Tests.Intercept
{
    public static class Setup
    {
        [Fact]
        public static void Func()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo()).Returns(5)
                .Setup(m => m.Foo(5)).Returns(6);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo())
                .Should()
                .Be(5);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo(5))
                .Should()
                .Be(6);

            mock.Verify();
        }

        [Fact]
        public static void Action()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.Bar(5));

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Bar(5));

            mock.Verify();
        }
            
        [Fact]
        public static async Task FuncAsync()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.FooAsync())
                .Returns(7);

            var result = await mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.FooAsync());

            result
                .Should()
                .Be(7);

            mock.Verify();
        }

        [Fact]
        public static async Task FuncAsyncFuncAsync()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(x => x.FooAsync(Arg.Ignore<int>()))
                .Returns(4)
                .Setup(x => x.FooAsync(1))
                .Returns(3);
                
            var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
            await sut.Execute(r => r.FooAsync(1));
            await sut.Execute(r => r.FooAsync(2));
                
            mock.Verify();
        }

        [Fact]
        public static async Task ActionAsync()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.BarAsync());

            await mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(m => m.BarAsync());

            mock.Verify();
        }

        [Fact]
        public static void Property()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Dummy)
                .Returns(4);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(m => m.Dummy);

            mock.Verify();
        }
            
        [Fact]
        public static void IgnoreArgument()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo(Arg.Ignore<int>()))
                .Returns(5);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo(13))
                .Should()
                .Be(5);

            mock.Verify();
        }
        
        [Fact]
        public static void With()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo(Arg.With<int>(x => x.Should().Be(3, "that's what it's always been"))))
                .Returns(5);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo(3))
                .Should()
                .Be(5);

            mock.Verify();
        }
        
        [Fact]
        public static void WithException()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo(Arg.With<int>(x => x.Should().Be(3, ""))))
                .Returns(5);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Invoking(x => x.Execute(m => m.Foo(13)))
                .Should()
                .Throw<XunitException>();
        }

        [Fact]
        public static void NullArgument()
        {
            var mock = Interceptor
                .For<SomeUnmockableObject>()
                .Setup(m => m.Foo(3, null))
                .Returns(5);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo(3, null))
                .Should()
                .Be(5);

            mock.Verify();
        }
            
        [Fact]
        public static void NoSetup()
        {
            Interceptor
                .For<SomeUnmockableObject>()
                .As<IUnmockable<SomeUnmockableObject>>()
                .Invoking(x => x.Execute(m => m.Foo()))
                .Should()
                .Throw<SetupNotFoundException>()
                .WithMessage("SomeUnmockableObject.Foo()");
        }

        [Fact]
        public static void NoSetupEnumerable()
        {
            Interceptor
                .For<SomeUnmockableObject>()
                .As<IUnmockable<SomeUnmockableObject>>()
                .Invoking(x => x.Execute(m => m.Foo(new[] {1, 2, 3, 4})))
                .Should()
                .Throw<SetupNotFoundException>()
                .WithMessage("SomeUnmockableObject.Foo([1, 2, 3, 4])");
        }
            
        [Fact]
        public static void NoSetupNull()
        {
            Interceptor
                .For<SomeUnmockableObject>()
                .As<IUnmockable<SomeUnmockableObject>>()
                .Invoking(x => x.Execute(m => m.Foo(3, null)))
                .Should()
                .Throw<SetupNotFoundException>()
                .WithMessage("SomeUnmockableObject.Foo(3, null)");
        }
            
        [Fact]
        public static void NoSetupProperty()
        {
            Interceptor
                .For<SomeUnmockableObject>()
                .As<IUnmockable<SomeUnmockableObject>>()
                .Invoking(x => x.Execute(m => m.Dummy))
                .Should()
                .Throw<SetupNotFoundException>()
                .WithMessage("SomeUnmockableObject.Dummy");
        }

        [Fact]
        public static void SetupChain()
        {
            var mock = Interceptor.For<SomeUnmockableObject>();
            mock
                .Setup(m => m.Foo(5)).Returns(2)
                .Setup(y => y.Foo(4)).Throws<NotSupportedException>()
                .Setup(x => x.Bar(5))
                .Setup(x => x.Foo(3)).Returns(1).Then(4).Then(6).ThenThrows<NotImplementedException>().Then(6).ThenThrows<FileNotFoundException>();

            mock
                .Setup(x => x.Bar(3))
                .Setup(x => x.Bar(3)).Throws<FileNotFoundException>()
                .Setup(x => x.Bar(3));

            var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
            sut.Execute(r => r.Foo(5))
                .Should()
                .Be(2);
                
            sut.Execute(q => q.Foo(3))
                .Should()
                .Be(1);
        }
    }
}