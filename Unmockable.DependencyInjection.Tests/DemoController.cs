using System.Net.Http;
using System.Threading.Tasks;

namespace Unmockable.DependencyInjection.Tests
{
    public class DemoController
    {
        private readonly IUnmockable<HttpClient> _client;

        public DemoController(IUnmockable<HttpClient> client) => _client = client;

        public async Task<string> Do()
        {
            var result = await _client.Execute(x => x.GetAsync("https://google.com"));
            return await result.Content.ReadAsStringAsync();
        }
    }
}