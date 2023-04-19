using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Unmockable.DependencyInjection.Tests
{
    public static class ServiceCollectionExtensionTests
    {
        [Fact]
        public static async Task AddUnmockables_ServiceTypeResolved()
        {
            // Arrange
            var provider = new ServiceCollection()
                .AddScoped<DemoControllerServiceType>()
                .AddSingleton<HttpMessageInvoker>(new HttpClient())
                .AddUnmockables()
                .BuildServiceProvider();

            // Act
            var controller = provider.GetRequiredService<DemoControllerServiceType>();
            var result = await controller.Do();
            
            // Assert
            result.Should().Contain("<!doctype html>");
        }

        [Fact]
        public static async Task AddUnmockables_ServiceResolved()
        {
            // Arrange
            var provider = new ServiceCollection()
                .AddScoped<DemoController>()
                .AddSingleton(new HttpClient())
                .AddUnmockables()
                .BuildServiceProvider();
            
            // Act
            var controller = provider.GetRequiredService<DemoController>();
            var result = await controller.Do();
            
            // Assert
            result.Should().Contain("<!doctype html>");
        }
    }
}