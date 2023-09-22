using Elasticsearch.Api.Repositories;

namespace Elasticsearch.Api.Extensions
{
    public static class InfrastructureExtension
    {
        public static void AddInfrastructureRegistiration(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IECommerceRepository, ECommerceRepository>();
        }
    }
}
