using System.Collections.Immutable;
using Elasticsearch.Api.Dtos;
using Elasticsearch.Api.Models;

namespace Elasticsearch.Api.Services
{
    public interface IProductService
    {
        Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto dto);
        Task<ResponseDto<ImmutableList<ProductDto>>> GetAllAsync();
        Task<ResponseDto<ProductDto>> GetByIdAsync(string id);
        Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateDto);
        Task<ResponseDto<bool>> DeleteAsync(string id);
    }
}
