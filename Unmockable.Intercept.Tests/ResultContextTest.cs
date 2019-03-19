using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Unmockable.Result;
using Unmockable.Setup;
using Xunit;

namespace Unmockable.Tests
{
    public static class ResultContextTest
    {
        [Fact]
        public static void NoNext()
        {
            var context = new ResultContext<int>();
            context
                .HasNext
                .Should()
                .BeFalse();
        }
        
        [Fact]
        public static void Value()
        {
            var context = new ResultContext<int>();
            context.Add(3);

            context
                .Next()
                .Should()
                .Be(3);
        }

        [Fact]
        public static void InfiniteLast()
        {
            var context = new ResultContext<int>();
            context.Add(3);

            context.Next();
            context
                .Next()
                .Should()
                .Be(3);
        }

        [Fact]
        public static void AddTwice()
        {
            var context = new ResultContext<int>();
            context.Add(3);
            context.Add(5);

            context.Next().Should().Be(3);
            context.Next().Should().Be(5);
        }

        [Fact]
        public static void AddThrows()
        {
            var context = new ResultContext<int>();
            context.Add<FileNotFoundException>();

            Assert.Throws<FileNotFoundException>(() => context.Next());
        }

        [Fact]
        public static void NothingNeverThrows()
        {
            var context = new ResultContext<Nothing>();
            context.Next();
            context.Next();
        }
        
        [Fact]
        public static void NothingThrows()
        {
            var context = new ResultContext<Nothing>();
            context.Add<FileNotFoundException>();
            
            Assert.Throws<FileNotFoundException>(() => context.Next());
        }
        
        [Fact]
        public static async Task TaskAlwaysCompleted()
        {
            var context = new ResultContext<Task>();
            await context.Next();
            await context.Next();
        }

        [Fact]
        public static void IsDoneIsFalseWhenNotExecuted()
        {
            var context = new ResultContext<int>();
            context.Add(3);

            context
                .IsDone
                .Should()
                .BeFalse();
        }
        
        [Fact]
        public static void IsDoneIsTrueWhenExecuted()
        {
            var context = new ResultContext<int>();
            context.Add(3);
            context.Next();
            
            context
                .IsDone
                .Should()
                .BeTrue();
        }
    }
}