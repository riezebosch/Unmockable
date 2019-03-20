using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Unmockable.DependencyInjection.Tests
{
    public static class ServiceCollectionExtensionTests
    {
        [Fact]
        public static async Task AddUnmockables_ServiceResolved()
        {
            var provider = new ServiceCollection()
                .AddScoped<DemoController>()
                .AddSingleton(new HttpClient())
                .AddUnmockables()
                .BuildServiceProvider();

            var controller = provider.GetService<DemoController>();
            await Assert.ThrowsAsync<HttpRequestException>(() => controller.Do());
        }
    }
}