using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.Api.Dtos;
using Elasticsearch.Api.Models;
using Elasticsearch.Api.Repositories;

namespace Elasticsearch.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Exceptions handled for this method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var response = await _repository.DeleteAsync(id);
            
            if(!response.IsValidResponse && response.Result == Result.NotFound)
            {
                _logger.LogError($"could not find an item with id {id}");
                return ResponseDto<bool>.Fail($"could not find an item with id {id}", System.Net.HttpStatusCode.NotFound);
            }

            if (!response.IsValidResponse)
            {
                //if has an error, assign it to exception variable
                response.TryGetOriginalException(out Exception? exception);
                _logger.LogError(exception, response.ElasticsearchServerError.Error.ToString());
                return ResponseDto<bool>.Fail($"something went wrong on {nameof(DeleteAsync)}", System.Net.HttpStatusCode.InternalServerError);
            }


            return ResponseDto<bool>.Success(true, System.Net.HttpStatusCode.NoContent);

        }

        public async Task<ResponseDto<ImmutableList<ProductDto>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();
            var dtos = response.Select(a => a.CreateDto()).ToImmutableList();
            return ResponseDto<ImmutableList<ProductDto>>.Success(dtos, System.Net.HttpStatusCode.Created);
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var response = await _repository.GetByIdAsync(id);

            if (response is null)
            {
                return ResponseDto<ProductDto>.Fail($"product with id:{id} couldn't found", System.Net.HttpStatusCode.InternalServerError);
            }

            return ResponseDto<ProductDto>.Success(response.CreateDto(), System.Net.HttpStatusCode.Created);
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto dto)
        {
            var product = dto.CreateProduct();
            var response = await _repository.SaveAsync(product);

            if (response is null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "something went wrong" }, System.Net.HttpStatusCode.InternalServerError);
            }
            return ResponseDto<ProductDto>.Success(response.CreateDto(), System.Net.HttpStatusCode.Created);
        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateDto)
        {
            var response = await _repository.UpdateAsync(updateDto);

            return response ? ResponseDto<bool>.Success(response, System.Net.HttpStatusCode.NoContent)
                : ResponseDto<bool>.Fail($"something went wrong on {nameof(UpdateAsync)}", System.Net.HttpStatusCode.Created);
        }
    }
}
