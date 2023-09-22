﻿using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace Elasticsearch.Api.Extensions
{
    public static class ElasticsearchExtension
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration.GetSection("Elastic")["Url"]!;
            var username = configuration.GetSection("Elastic")["Username"]!;
            var password = configuration.GetSection("Elastic")["Password"]!;

            var settings = new ElasticsearchClientSettings
                (new Uri(url))
                .Authentication(new BasicAuthentication(username, password));

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);
        }
    }
}
