using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.Api.Configurations;
using Elasticsearch.Api.Dtos;
using Elasticsearch.Api.Models;

namespace Elasticsearch.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ElasticsearchClient _client;

        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id, x => x.Index(Indexes.ProductsIndex));
            return response;
        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var products = await _client.SearchAsync<Product>(
                s => s.Index(Indexes.ProductsIndex)
                .Query(q => q.MatchAll()));

            foreach (var hit in products.Hits)
            {
                //id comes null, to solve it get id from hit id
                hit.Source.Id = hit.Id;
            }

            return products.Documents.ToImmutableList();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var product = await _client.GetAsync<Product>(id, x => x.Index(Indexes.ProductsIndex));

            if (!product.IsSuccess())
            {
                return null;
            }

            product.Source.Id = product.Id;
            return product.Source;
        }

        public async Task<Product?> SaveAsync(Product product)
        {
            product.Created = DateTime.Now;

            var response = await _client.IndexAsync(product,
                a => a.Index(Indexes.ProductsIndex)
                .Id(Guid.NewGuid().ToString()));

            if (!response.IsSuccess())
            {
                return null;
            }

            //id comes from elasticsearch
            product.Id = response.Id;
            return product;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateDto)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>
                (Indexes.ProductsIndex, updateDto.Id, x => x.Doc(updateDto));

            return response.IsSuccess();
        }
    }
}
