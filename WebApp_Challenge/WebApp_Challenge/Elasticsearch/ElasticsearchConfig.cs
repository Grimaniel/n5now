using Nest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace WebApp_Challenge.Elasticsearch
{
    public static class ElasticsearchConfig
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticsearchSettings:Uri"];
            var defaultIndex = configuration["ElasticsearchSettings:DefaultIndex"];

            var settings = new ConnectionSettings(new Uri(url))
                            .DefaultIndex(defaultIndex);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }

    }
}
