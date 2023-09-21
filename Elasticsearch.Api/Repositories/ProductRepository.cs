using System.Collections.Immutable;
using Elasticsearch.Api.Configurations;
using Elasticsearch.Api.Dtos;
using Elasticsearch.Api.Models;
using Nest;

namespace Elasticsearch.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ElasticClient _client;

        public ProductRepository(ElasticClient client)
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

            if (!product.IsValid)
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

            if (!response.IsValid)
            {
                return null;
            }

            //id comes from elasticsearch
            product.Id = response.Id;
            return product;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateDto)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(updateDto.Id,
                x => x.Index(Indexes.ProductsIndex).Doc(updateDto));

            return response.IsValid;
        }
    }
}
