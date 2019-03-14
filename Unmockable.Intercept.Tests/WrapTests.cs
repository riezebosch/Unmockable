using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Unmockable.Tests
{
    public static class WrapTests
    {
        [Fact]
        public static void Execute()
        {
            var wrap = new SomeUnmockableObject { Dummy = 3 }.Wrap();
            wrap.Execute(x => x.Foo()).Should().Be(3);
        }

        [Fact]
        public static async Task ExecuteAsync()
        {
            var wrap = new SomeUnmockableObject { Dummy =  9 }.Wrap();
            var result = await wrap.Execute(x => x.FooAsync());
            result.Should().Be(9);
        }

        [Fact]
        public static void ExecuteAction()
        {
            var wrap = new SomeUnmockableObject().Wrap();
            wrap.Execute(x => x.Bar(10));

            wrap
                .Execute(x => x.Dummy)
                .Should()
                .Be(10);
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
                wrap.As<IUnmockable<SomeUnmockableObject>>()
                    .Execute(m => m.Foo(i));
            }

            sw.Elapsed.TotalSeconds.Should().BeLessThan(5);
        }

        [Fact]
        public static void WrapObject()
        {
            new object().Wrap().Should().BeOfType<Wrap<object>>();
        }
    }
}