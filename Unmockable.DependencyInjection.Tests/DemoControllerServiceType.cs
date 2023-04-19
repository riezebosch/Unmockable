using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Unmockable.DependencyInjection.Tests
{
    public class DemoControllerServiceType
    {
        private readonly IUnmockable<HttpMessageInvoker> _client;

        public DemoControllerServiceType(IUnmockable<HttpMessageInvoker> client) => _client = client;

        public async Task<string> Do()
        {
            var result = await _client.Execute(x => x.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://google.com"), CancellationToken.None));
            return await result.Content.ReadAsStringAsync();
        }
    }
}