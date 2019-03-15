using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests
{
    public static class InterceptTests
    {
        public static class Setup
        {
            [Fact]
            public static void Func()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Foo()).Returns(5)
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
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(x => x.Bar(5));

                mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(x => x.Bar(5));

                mock.Verify();
            }
            
            [Fact]
            public static async Task FuncAsync()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(x => x.FooAsync())
                    .Returns(7);

                var result = await mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(x => x.FooAsync());

                result
                    .Should()
                    .Be(7);

                mock.Verify();
            }
            
            [Fact]
            public static async Task ActionAsync()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.BarAsync());

                await mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.BarAsync());

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
            public static void NoSetup()
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
            public static void NoSetupEnumerable()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                var items = new[] {1, 2, 3, 4};

                var ex = Assert.Throws<NoSetupException>(() => mock
                    .As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(x => x.Foo(items)));
                ex.Message.Should().Contain("Foo([1, 2, 3, 4])");
            }

            [Fact]
            public static void SetupChain()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock
                    .Setup(m => m.Foo(5)).Returns(2)
                    .Setup(y => y.Foo(4)).Throws<NotSupportedException>()
                    .Setup(x => x.Bar(5))
                    .Setup(x => x.Foo(3)).Returns(1).Then(4).Then(6).ThenThrows<NotImplementedException>().Then(6).ThenThrows<FileNotFoundException>();

                mock
                    .Setup(x => x.Bar(3))
                    .Setup(x => x.Bar(3)).Throws<FileNotFoundException>()
                    .Setup(x => x.Bar(3));

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
        }

        public static class Result
        {
            [Fact]
            public static void NoResult()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Foo());

                var ex = Assert.Throws<NoResultConfiguredException>(() => mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.Foo()));

                ex.Message
                    .Should()
                    .Contain("m => m.Foo()");
            }

            
            [Fact]
            public static async Task NoResultAsync()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.FooAsync());

                var ex = await Assert.ThrowsAsync<NoResultConfiguredException>(() => mock
                    .As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.FooAsync()));

                ex.Message
                    .Should()
                    .Contain("m => m.FooAsync()");
            }

            [Fact]
            public static void Then()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Foo())
                    .Returns(5)
                    .Then(6);

                mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.Foo())
                    .Should()
                    .Be(5);
                    
                mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.Foo())
                    .Should()
                    .Be(6);
                
                mock.Verify();
            }
            
            [Fact]
            public static void AsyncThen()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.FooAsync())
                    .Returns(5)
                    .Then(6);

                mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.FooAsync())
                    .Result
                    .Should()
                    .Be(5);
                    
                mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.FooAsync())
                    .Result
                    .Should()
                    .Be(6);
                
                mock.Verify();
            }
        }
        
        public static class Throws
        {
            [Fact]
            public static void FuncThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Foo()).Throws<FileNotFoundException>();

                Assert.Throws<FileNotFoundException>(() => mock
                    .As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.Foo()));

                mock.Verify();
            }

            [Fact]
            public static void ActionThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Bar(5))
                    .Throws<FileNotFoundException>();

                Assert.Throws<FileNotFoundException>(() => mock
                    .As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.Bar(5)));

                mock.Verify();
            }

            [Fact]
            public static async Task FuncAsyncThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(x => x.FooAsync())
                    .Throws<FileNotFoundException>();

                await Assert.ThrowsAsync<FileNotFoundException>(() => mock
                    .As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.FooAsync()));

                mock.Verify();
            }
            
            [Fact]
            public static async Task ActionAsyncThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(x => x.BarAsync())
                    .Throws<FileNotFoundException>();

                await Assert.ThrowsAsync<FileNotFoundException>(() => mock
                    .As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.BarAsync()));

                mock.Verify();
            }

            [Fact]
            public static void ThenThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Foo())
                    .Returns(5)
                    .ThenThrows<FileNotFoundException>();

                mock.As<IUnmockable<SomeUnmockableObject>>().Execute(m => m.Foo());
                Assert.Throws<FileNotFoundException>(() => mock.As<IUnmockable<SomeUnmockableObject>>().Execute(m => m.Foo()));
                
                mock.Verify();
            }
            
            [Fact]
            public static async Task AsyncThenThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.FooAsync())
                    .Returns(5)
                    .ThenThrows<FileNotFoundException>();

                await mock.As<IUnmockable<SomeUnmockableObject>>().Execute(m => m.FooAsync());
                await Assert.ThrowsAsync<FileNotFoundException>(() => mock.As<IUnmockable<SomeUnmockableObject>>().Execute(m => m.FooAsync()));
                
                mock.Verify();
            }
        }
        
        public static class Verify
        {
            [Fact]
            public static void Executed()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock
                    .Setup(m => m.Foo())
                    .Returns(5);
                
                mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(x => x.Foo());

                mock.Verify();
            }

            [Fact]
            public static void NotExecuted()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Foo());

                Assert.Throws<NotExecutedException>(() => mock.Verify());
            }
        }
    }
}