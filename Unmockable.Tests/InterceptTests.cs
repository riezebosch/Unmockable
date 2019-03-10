using System;
using System.Threading.Tasks;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests
{
    public class InterceptTests
    {
        [Fact]
        public static void ExecuteTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo()).Returns(5);

            mock
                .Execute(x => x.Foo())
                .Should()
                .Be(5);
            mock.Verify();
        }

        [Fact]
        public static void ExecuteActionTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(x => x.Bar());
            
            mock.Execute(x => x.Bar());
            mock
                .Verify();
        }
        
        [Fact]
        public static void NoSetupTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            
            var ex = Assert.Throws<NoSetupException>(() => mock.Execute(m => m.Foo()));
            ex
                .Message
                .Should()
                .Contain("Foo()");
        }

        [Fact]
        public static void NoSetupResultTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo());
            
            var ex = Assert.Throws<NoResultConfiguredException>(() => mock.Execute(m => m.Foo()));
            ex
                .Message
                .Should()
                .Contain("m => m.Foo()");
        }

        [Fact]
        public static async Task ResultAsync()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock
                .Setup(x => x.FooAsync())
                .Returns(7);
            
            var result = await mock.Execute(x => x.FooAsync());
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

            var ex = await Assert.ThrowsAsync<NoResultConfiguredException>(() => mock.Execute(m => m.FooAsync()));
            ex
                .Message
                .Should()
                .Contain("m => m.FooAsync()");
        }

        [Fact]
        public static void NoSetupExceptionIncludesArgumentValues()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            var items = new[] {1, 2, 3, 4};
            
            var ex = Assert.Throws<NoSetupException>(() => mock.Execute(x => x.Foo(items)));
            ex.Message.Should().Contain("Foo([1, 2, 3, 4])");
        }

        [Fact]
        public static async Task NonGenericAsyncTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.
                Setup(m => m.BarAsync());


            await mock.Execute(m => m.BarAsync());
        }

        [Fact]
        public static void SetupThrows()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo()).Throws<NotImplementedException>();

            Assert.Throws<NotImplementedException>(() => mock.Execute(m => m.Foo()));
            mock.Verify();
        }
        
        [Fact]
        public static void SetupActionThrows()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Bar()).Throws<NotImplementedException>();

            Assert.Throws<NotImplementedException>(() => mock.Execute(m => m.Bar()));
            mock.Verify();
        }
        
        [Fact]
        public static async Task SetupThrowsAsync()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock
                .Setup(x => x.BarAsync())
                .Throws<NotImplementedException>();
            
            await Assert.ThrowsAsync<NotImplementedException>(() => mock.Execute(m => m.BarAsync()));
            mock.Verify();
        }

        [Fact]
        public static void SetupChainTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock
                .Setup(m => m.Foo(5)).Returns(2)
                .Setup(y => y.Foo(4)).Throws<NotSupportedException>()
                .Setup(x => x.Bar())
                .Setup(x => x.Foo(3)).Returns(1);

            mock.Execute(r => r.Foo(5)).Should().Be(2);
            mock.Execute(q => q.Foo(3)).Should().Be(1);
        }

        [Fact]
        public static void VerifyTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo()).Returns(5);
            mock.Execute(x => x.Foo());

            mock.Verify();
        }
        
        [Fact]
        public static void VerifyNotExecutedTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo());

            Assert.Throws<NotExecutedException>(() => mock.Verify());
        }
    }
}