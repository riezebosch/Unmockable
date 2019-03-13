using System;
using System.Threading.Tasks;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests
{
    public static class InterceptTests
    {
        public static class InstanceMock
        {
            [Fact]
            public static void SetupTest()
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

                var ex = Assert.Throws<NoResultConfiguredException>(() => mock.As<IUnmockable<SomeUnmockableObject>>()
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

                var result = await mock.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(x => x.FooAsync());
                
                result
                    .Should()
                    .Be(7);

                mock.Verify();
            }

            [Fact]
            public static async Task NoSetupResultAsyncTest()
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
                    .Setup(x => x.Bar(3))
                    .Setup(x => x.Bar(3)).Throws<NotImplementedException>()
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
        }

        public static class StaticMock
        {
            [Fact]
            public static void SetupTest()
            {
                var mock = new Intercept();
                mock.Setup(() => int.Parse("3")).Returns(4)
                    .Setup(() => int.Parse("4")).Returns(5);

                mock.As<IStatic>()
                    .Execute(() => int.Parse("3"))
                    .Should()
                    .Be(4);
                
                mock.As<IStatic>()
                    .Execute(() => int.Parse("4"))
                    .Should()
                    .Be(5);

                mock.Verify();
            }

            [Fact]
            public static void ExecuteProperty()
            {
                var mock = new Intercept();
                mock.Setup(() => DateTime.Today)
                    .Returns(new DateTime(2009, 9, 11));

                mock.As<IStatic>()
                    .Execute(() => DateTime.Today)
                    .Should()
                    .Be(new DateTime(2009, 9, 11));
            }
            
            [Fact]
            public static void Throws()
            {
                var mock = new Intercept();
                mock.Setup(() => int.Parse("3"))
                    .Throws<NotImplementedException>();

                Assert.Throws<NotImplementedException>(() => mock
                    .As<IStatic>()
                    .Execute(() => int.Parse("3")));
            }
            
            [Fact]
            public static void Verify()
            {
                var mock = new Intercept();
                mock.Setup(() => int.Parse("3")).Throws<NotImplementedException>()
                    .Setup(() => double.Parse("4")).Returns(4);

                var ex = Assert.Throws<NotExecutedException>(() => mock.Verify());

                ex.Message
                    .Should()
                    .Contain(@"Parse(""3"")");
                
                ex.Message
                    .Should()
                    .Contain(@"Parse(""4"")");
            }

            [Fact]
            public static void SetupChaining()
            {
                var mock = new Intercept();
                mock.Setup(() => int.Parse("3")).Throws<NotImplementedException>()
                    .Setup(() => double.Parse("4")).Returns(4)
                    .Setup(() => int.Parse("2")).Returns(3);

                mock.Setup(() => SomeUnmockableObject.ThrowStatic())
                    .Setup(() => SomeUnmockableObject.ThrowStatic()).Throws<NotImplementedException>()
                    .Setup(() => SomeUnmockableObject.ThrowStatic())
                    .Setup(() => int.Parse("3"));
            }

            [Fact]
            public static void Action()
            {
                var mock = new Intercept();
                mock.Setup(() => Console.WriteLine("hello"));

                mock.As<IStatic>()
                    .Execute(() => Console.WriteLine("hello"));
                
                mock.Verify();
            }

            [Fact]
            public static void NoResultThrows()
            {
                var mock = new Intercept();
                mock.Setup(() => int.Parse("3"));
                
                Assert.Throws<NoResultConfiguredException>(() => 
                    mock.As<IStatic>().Execute(() => int.Parse("3")));
            }
        }
    }
}