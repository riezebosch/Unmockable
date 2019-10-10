using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Unmockable.Tests
{
    public static partial class InterceptTests
    {
        public static class Throws
        {
            [Fact]
            public static void FuncThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock
                    .Setup(m => m.Foo())
                    .Throws<FileNotFoundException>();

                var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
                sut.Invoking(x => x.Execute(m => m.Foo()))
                    .Should()
                    .Throw<FileNotFoundException>();

                mock.Verify();
            }

            [Fact]
            public static void ActionThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Bar(5))
                    .Throws<FileNotFoundException>();

                var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
                sut.Invoking(x => x.Execute(m => m.Bar(5)))
                    .Should()
                    .Throw<FileNotFoundException>();

                mock.Verify();
            }
            
            [Fact]
            public static void ActionThrowsThenThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Bar(5))
                    .Throws<FileNotFoundException>()
                    .ThenThrows<DirectoryNotFoundException>();

                var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
                sut.Invoking(x => x.Execute(m => m.Bar(5)))
                    .Should()
                    .Throw<FileNotFoundException>();

                sut.Invoking(x => x.Execute(m => m.Bar(5)))
                    .Should()
                    .Throw<DirectoryNotFoundException>();

                mock.Verify();
            }

            [Fact]
            public static async Task FuncAsyncThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(x => x.FooAsync())
                    .Throws<FileNotFoundException>();

                var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
                await sut.Invoking(x => x.Execute(m => m.FooAsync()))
                    .Should()
                    .ThrowAsync<FileNotFoundException>();

                mock.Verify();
            }
            
            [Fact]
            public static async Task FuncAsyncThrowsThenThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(x => x.FooAsync())
                    .Throws<FileNotFoundException>()
                    .ThenThrows<DirectoryNotFoundException>();

                var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
                await sut.Invoking(x => x.Execute(m => m.FooAsync()))
                    .Should()
                    .ThrowAsync<FileNotFoundException>();
                await sut.Invoking(x => x.Execute(m => m.FooAsync()))
                    .Should()
                    .ThrowAsync<DirectoryNotFoundException>();

                mock.Verify();
            }
            
            [Fact]
            public static async Task ActionAsyncThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(x => x.BarAsync())
                    .Throws<FileNotFoundException>();

                var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
                await sut.Invoking(x => x.Execute(m => m.BarAsync()))
                    .Should()
                    .ThrowAsync<FileNotFoundException>();

                mock.Verify();
            }

            [Fact]
            public static void ThenThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.Foo())
                    .Returns(5)
                    .ThenThrows<FileNotFoundException>();

                var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
                sut.Execute(m => m.Foo());
                
                sut.Invoking(x => x.Execute(m => m.Foo()))
                    .Should()
                    .Throw<FileNotFoundException>();
                
                mock.Verify();
            }
            
            [Fact]
            public static async Task AsyncThenThrows()
            {
                var mock = new Intercept<SomeUnmockableObject>();
                mock.Setup(m => m.FooAsync())
                    .Returns(5)
                    .ThenThrows<FileNotFoundException>();

                var sut = mock.As<IUnmockable<SomeUnmockableObject>>();
                await sut.Execute(m => m.FooAsync());
                await sut.Invoking(x => x.Execute(m => m.FooAsync()))
                    .Should()
                    .ThrowAsync<FileNotFoundException>();
                
                mock.Verify();
            }
        }
    }
}