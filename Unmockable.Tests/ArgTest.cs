using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests
{
    public static class ArgTest
    {
        [Fact]
        public static void ArgIgnore()
        {
            Assert.Throws<PlaceholderException>(() => Arg.Ignore<int>());
        }
        
        [Fact]
        public static void ArgEquals()
        {
            Assert.Throws<PlaceholderException>(() => Arg.Equals<int>(x => x == 3));
        }
    }
}