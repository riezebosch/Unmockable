using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;

namespace Unmockable.Tests
{
    public class WrapTests
    {
        [Fact]
        public void ExecuteOnRealItem()
        {
            var wrap = new SomeUnmockableObject().Wrap();
            wrap.Execute(x => x.Foo()).Should().Be(3);
        }

        [Fact]
        public void ExecuteAction()
        {
            var wrap = new SomeUnmockableObject().Wrap();
            Assert.Throws<NotImplementedException>(() => wrap.Execute(x => x.Throw()));
        }

        [Fact]
        public void CachingUnique()
        {
            const int i = 6;
            var wrap = new SomeUnmockableObject().Wrap();
            wrap.Execute(x => x.Foo(5)).Should().Be(5);
            wrap.Execute(x => x.Foo(3)).Should().Be(3);
            wrap.Execute(x => x.Foo(new Func<int>(() => 4)())).Should().Be(4);
            wrap.Execute(x => x.Foo(i)).Should().Be(6);
        }

        [Fact]
        public void PerfTest()
        {
            var wrap = new Wrap<SomeUnmockableObject>(new SomeUnmockableObject());

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < 100000; i++)
            {
                wrap.Execute(m => m.Foo());
            }

            sw.Elapsed.TotalSeconds.Should().BeLessThan(1);
        }

        [Fact]
        public void WrapObject()
        {
            new object().Wrap().Should().BeOfType<Wrap<object>>();
        }
    }
}