using Elasticsearch.Api.Repositories;
using Elasticsearch.Api.Services;

namespace Elasticsearch.Api.Extensions
{
    public static class BusinessExtension
    {
        public static void AddBusinessRegistiration(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IECommerceService, ECommerceService>();
        }
    }
}
