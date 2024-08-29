using Nest;
using WebApp_Challenge.Models;

namespace WebApp_Challenge.Elasticsearch
{
    public class PermissionElasticsearchService
    {
        private readonly IElasticClient _elasticClient;

        public PermissionElasticsearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task IndexPermissionAsync(Permission permission)
        {
            var response = await _elasticClient.IndexDocumentAsync(permission);
            if (!response.IsValid)
            {
                // Manejar errores
            }
        }

    }
}
