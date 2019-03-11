using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests
{
    public class WrapTests
    {
        [Fact]
        public static void ExecuteOnRealItem()
        {
            var wrap = new SomeUnmockableObject().Wrap();
            wrap.Execute(x => x.Foo()).Should().Be(3);
        }

        [Fact]
        public static async Task ExecuteAsync()
        {
            var wrap = new SomeUnmockableObject().Wrap();
            var result = await wrap.Execute(x => x.FooAsync());
            result.Should().Be(9);
        }

        [Fact]
        public static void ExecuteAction()
        {
            var wrap = new SomeUnmockableObject().Wrap();
            Assert.Throws<NotImplementedException>(() => wrap.Execute(x => x.Bar()));
        }

        [Fact]
        public static void CachingUnique()
        {
            const int i = 6;
            var wrap = new SomeUnmockableObject().Wrap();
            wrap.Execute(x => x.Foo(5)).Should().Be(5);
            wrap.Execute(x => x.Foo(3)).Should().Be(3);
            wrap.Execute(x => x.Foo(new Func<int>(() => 4)())).Should().Be(4);
            wrap.Execute(x => x.Foo(i)).Should().Be(6);

            for (var j = 0; j < 200; j++)
            {
                wrap.Execute(x => x.Foo(j)).Should().Be(j);
            }
        }

        [Fact]
        public static void PerfTest()
        {
            var wrap = new Wrap<SomeUnmockableObject>(new SomeUnmockableObject());

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < 100000; i++)
            {
                wrap.Execute(m => m.Foo(i));
            }

            sw.Elapsed.TotalSeconds.Should().BeLessThan(3);
        }

        [Fact]
        public static void NotAnInstanceMethodCall()
        {
            var wrap = new SomeUnmockableObject().Wrap();
            var ex = Assert.Throws<NotInstanceMethodCallException>(() => wrap.Execute(x => 3));
            ex.Message.Should().Contain("x => 3");
        }

        [Fact]
        public static void WrapObject()
        {
            new object().Wrap().Should().BeOfType<Wrap<object>>();
        }
    }
}