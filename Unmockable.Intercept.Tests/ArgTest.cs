using Unmockable.Exceptions;
using Xunit;

namespace Unmockable.Tests
{
    public static class ArgTest
    {
        [Fact]
        public static void IgnoreShouldNotBeExecuted()
        {
            Assert.Throws<PlaceholderException>(() => Arg.Ignore<int>());
        }
        
        [Fact]
        public static void WhereShouldNotBeExecuted()
        {
            Assert.Throws<PlaceholderException>(() => Arg.Where<int>(x => x == 3));
        }
    }
}