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
            IUnmockable<SomeUnmockableObject> mock = new Wrap<SomeUnmockableObject>(new SomeUnmockableObject());
            mock.Execute(x => x.Foo()).Should().Be(3);
        }

        [Fact]
        public void CachingUnique()
        {
            const int i = 6;
            var real = new Wrap<SomeUnmockableObject>(new SomeUnmockableObject());
            real.Execute(x => x.Foo(5)).Should().Be(5);
            real.Execute(x => x.Foo(3)).Should().Be(3);
            real.Execute(x => x.Foo(new Func<int>(() => 4)())).Should().Be(4);
            real.Execute(x => x.Foo(i)).Should().Be(6);
        }

        [Fact]
        public void PerfTest()
        {
            var real = new Wrap<SomeUnmockableObject>(new SomeUnmockableObject());

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < 100000; i++)
            {
                real.Execute(m => m.Foo());
            }

            sw.Elapsed.TotalSeconds.Should().BeLessThan(1);
        }
    }
}