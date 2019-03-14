using System.Net.Http;

namespace Unmockable.DependencyInjection.Tests
{
    public class DemoController
    {
        public DemoController(IUnmockable<HttpClient> _)
        {
        }
    }
}