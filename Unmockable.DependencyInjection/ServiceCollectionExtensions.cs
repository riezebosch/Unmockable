using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Unmockable
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnmockables(this IServiceCollection collection)
        {
            collection
                .Select(x => x.ImplementationType ?? x.ServiceType)
                .Select(x => new ServiceDescriptor(
                    typeof(IUnmockable<>).MakeGenericType(x),
                    typeof(Wrap<>).MakeGenericType(x), 
                    ServiceLifetime.Transient))
                .ToList()
                .ForEach(collection.Add);
            
            return collection;
        }
    }
}