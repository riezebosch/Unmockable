using System;
using System.Threading.Tasks;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests
{
    public static class InterceptTests
    {
        [Fact]
        public static void ExecuteTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo()).Returns(5);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo())
                .Should()
                .Be(5);
            
            mock.Verify();
        }

        [Fact]
        public static void IgnoreArgument()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo(Arg.Ignore<int>()))
                .Returns(5);

            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo(13))
                .Should()
                .Be(5);
            
            mock.Verify();
        }

        [Fact]
        public static void ExecuteActionTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(x => x.Bar(5));
            
            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Bar(5));
            
            mock.Verify();
        }
        
        [Fact]
        public static void NoSetupTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            
            var ex = Assert.Throws<NoSetupException>(() => mock
                .As<IUnmockable<SomeUnmockableObject>>()
                .Execute(m => m.Foo()));
            
            ex.Message
                .Should()
                .Contain("Foo()");
        }

        [Fact]
        public static void NoSetupResultTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo());
            
            var ex = Assert.Throws<NoResultConfiguredException>(() => mock.
                As<IUnmockable<SomeUnmockableObject>>()
                .Execute(m => m.Foo()));
            
            ex.Message
                .Should()
                .Contain("m => m.Foo()");
        }

        [Fact]
        public static async Task ResultAsync()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(x => x.FooAsync())
                .Returns(7);
            
            var result = await mock.As<IUnmockable<SomeUnmockableObject>>().Execute(x => x.FooAsync());
            result
                .Should()
                .Be(7);
            
            mock.Verify();
        }
        
        [Fact]
        public static async Task NoSetupResultAsyncTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.
                Setup(m => m.FooAsync());

            var ex = await Assert.ThrowsAsync<NoResultConfiguredException>(() => mock
                .As<IUnmockable<SomeUnmockableObject>>()
                .Execute(m => m.FooAsync()));
            
            ex.Message
                .Should()
                .Contain("m => m.FooAsync()");
        }

        [Fact]
        public static void NoSetupExceptionIncludesArgumentValues()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            var items = new[] {1, 2, 3, 4};
            
            var ex = Assert.Throws<NoSetupException>(() => mock
                .As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo(items)));
            ex.Message.Should().Contain("Foo([1, 2, 3, 4])");
        }

        [Fact]
        public static async Task NonGenericAsyncTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.BarAsync());

            await mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(m => m.BarAsync());
            
            mock.Verify();
        }

        [Fact]
        public static void SetupThrows()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo()).Throws<NotImplementedException>();

            Assert.Throws<NotImplementedException>(() => mock
                    .As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.Foo()));
            
            mock.Verify();
        }
        
        [Fact]
        public static void SetupActionThrows()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Bar(5))
                .Throws<NotImplementedException>();

            Assert.Throws<NotImplementedException>(() => mock
                    .As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.Bar(5)));
            
            mock.Verify();
        }
        
        [Fact]
        public static async Task SetupThrowsAsync()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(x => x.BarAsync())
                .Throws<NotImplementedException>();
            
            await Assert.ThrowsAsync<NotImplementedException>(() => mock
                .As<IUnmockable<SomeUnmockableObject>>()
                .Execute(m => m.BarAsync()));
            
            mock.Verify();
        }

        [Fact]
        public static void SetupChainTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock
                .Setup(m => m.Foo(5)).Returns(2)
                .Setup(y => y.Foo(4)).Throws<NotSupportedException>()
                .Setup(x => x.Bar(5))
                .Setup(x => x.Foo(3)).Returns(1);

            mock
                .As<IUnmockable<SomeUnmockableObject>>()
                .Execute(r => r.Foo(5))
                .Should()
                .Be(2);
            mock
                .As<IUnmockable<SomeUnmockableObject>>()
                .Execute(q => q.Foo(3))
                .Should()
                .Be(1);
        }

        [Fact]
        public static void VerifyTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo()).Returns(5);
            mock.As<IUnmockable<SomeUnmockableObject>>()
                .Execute(x => x.Foo());

            mock.Verify();
        }
        
        [Fact]
        public static void VerifyNotExecutedTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo());

            Assert.Throws<NotExecutedException>(() => mock.Verify());
        }

        [Fact]
        public static void StaticMock()
        {
            var mock = new Intercept<int>();
            mock.Setup(() => int.Parse("3"))
                .Returns(4);

            mock.As<IStatic>()
                .Execute(() => int.Parse("3"))
                .Should()
                .Be(4);
        }
    }

    static class ObjectExtensions
    {
        public static T As<T>(this object item) 
        {
            return (T)item;
        }
    }
}