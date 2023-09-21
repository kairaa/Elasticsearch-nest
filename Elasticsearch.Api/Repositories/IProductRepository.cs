using System.Collections.Immutable;
using Elasticsearch.Api.Dtos;
using Elasticsearch.Api.Models;
using Nest;

namespace Elasticsearch.Api.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> SaveAsync(Product product);
        Task<ImmutableList<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(ProductUpdateDto updateDto);
        Task<DeleteResponse> DeleteAsync(string id);
    }
}
