using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Unmockable.DependencyInjection.Tests
{
    public static class ServiceCollectionExtensionTests
    {
        [Fact]
        public static void AddUnmockables_ServiceResolved()
        {
            var provider = new ServiceCollection()
                .AddScoped<DemoController>()
                .AddSingleton(new HttpClient())
                .AddUnmockables()
                .BuildServiceProvider();

            provider.GetService<DemoController>();
        }
    }
}