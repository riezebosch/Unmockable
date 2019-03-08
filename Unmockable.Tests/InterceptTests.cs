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
        public void ExecuteTest()
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
        public void ExecuteActionTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(x => x.Bar());
            
            mock
                .Execute(x => x.Bar());
            mock
                .Verify();
        }
        
        [Fact]
        public void NoSetupTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            
            var ex = Assert.Throws<NoSetupException>(() => mock.Execute(m => m.Foo()));
            ex
                .Message
                .Should()
                .Contain("m => m.Foo()");
        }

        [Fact]
        public void NoSetupResultTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo());
            
            var ex = Assert.Throws<NoSetupResultException>(() => mock.Execute(m => m.Foo()));
            ex
                .Message
                .Should()
                .Contain("m => m.Foo()");
        }

        [Fact]
        public async Task ResultAsync()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock
                .Setup(x => x.Wait())
                .Returns(7);
            
            var result = await mock.Execute(x => x.Wait());
            result
                .Should()
                .Be(7);
            
            mock.Verify();
        }
        
        [Fact]
        public async Task NoSetupResultAsyncTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.
                Setup(m => m.Wait());
            
            var ex = await Assert.ThrowsAsync<NoSetupResultException>(() => mock.Execute(m => m.Wait()));
            ex
                .Message
                .Should()
                .Contain("m => m.Wait()");
        }

        [Fact]
        public void SetupThrows()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo()).Throws<NotImplementedException>();

            Assert.Throws<NotImplementedException>(() => mock.Execute(m => m.Foo()));
            mock.Verify();
        }
        
        [Fact]
        public void SetupActionThrows()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Bar()).Throws<NotImplementedException>();

            Assert.Throws<NotImplementedException>(() => mock.Execute(m => m.Bar()));
            mock.Verify();
        }
        
        [Fact]
        public async Task SetupThrowsAsync()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock
                .Setup(x => x.ThrowAsync())
                .Throws<NotImplementedException>();
            
            await Assert.ThrowsAsync<NotImplementedException>(() => mock.Execute(m => m.ThrowAsync()));
            mock.Verify();
        }

        [Fact]
        public void SetupChainTest()
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
        public void VerifyTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo()).Returns(5);
            mock.Execute(x => x.Foo());

            mock.Verify();
        }
        
        [Fact]
        public void VerifyNotExecutedTest()
        {
            var mock = new Intercept<SomeUnmockableObject>();
            mock.Setup(m => m.Foo());

            Assert.Throws<NotExecutedException>(() => mock.Verify());
        }
    }
}