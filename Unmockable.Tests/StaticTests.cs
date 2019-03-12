using System;
using FluentAssertions;
using Xunit;

namespace Unmockable.Tests
{
    public static class StaticTests
    {
        [Fact]
        public static void WrapStaticMethod()
        {
            IStatic wrap = new Static();
            wrap
                .Execute(() => int.Parse("3"))
                .Should()
                .Be(3);
        }
        
        [Fact]
        public static void WrapStaticAction()
        {
            IStatic wrap = new Static();
            Assert.Throws<NotImplementedException>(() => wrap.Execute(() => SomeUnmockableObject.ThrowStatic()));
        }
        
        [Fact]
        public static void WrapStaticProperty()
        {
            IStatic wrap = new Static();
            wrap
                .Execute(() => DateTime.Today)
                .Should()
                .Be(DateTime.Today);
        }
    }
}