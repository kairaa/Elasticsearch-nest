using System.Collections.Immutable;
using Elasticsearch.Api.Dtos;
using Elasticsearch.Api.Models.ECommerceModel;
using Elasticsearch.Api.Repositories;

namespace Elasticsearch.Api.Services
{
    public class ECommerceService : IECommerceService
    {
        private readonly IECommerceRepository _repository;

        public ECommerceService(IECommerceRepository repository)
        {
            _repository = repository;
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> CompoundQueryExampleOneAsync(string cityName, double taxfulTotalPrice, string categoryName, string menufacturer)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> CompoundQueryExampleTwoAsync(string customerFullName)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> FuzzyQueryAsync(string customerName)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<ImmutableList<ECommerce>>> GetAllByFirstName(string customerFirstName)
        {
            var result = await _repository.TermQueryAsync(customerFirstName);
            return ResponseDto<ImmutableList<ECommerce>>.Success(result, System.Net.HttpStatusCode.Created);
        }

        public async Task<ResponseDto<ImmutableList<ECommerce>>> GetAllByFirstNameList(List<string> customerFirstNameList)
        {
            var result = await _repository.TermsQueryAsync(customerFirstNameList);
            return ResponseDto<ImmutableList<ECommerce>>.Success(result, System.Net.HttpStatusCode.Created);
        }

        public async Task<ResponseDto<ImmutableList<ECommerce>>> GetAllByFullName(string customerFullName)
        {
            var result = await _repository.PrefixQueryAsync(customerFullName);
            return ResponseDto<ImmutableList<ECommerce>>.Success(result, System.Net.HttpStatusCode.Created);
        }

        public async Task<ResponseDto<ImmutableList<ECommerce>>> GetAllByPriceRange(double fromPrice, double toPrice)
        {
            var result = await _repository.RangeQueryAsync(fromPrice, toPrice);
            return ResponseDto<ImmutableList<ECommerce>>.Success(result, System.Net.HttpStatusCode.Created);
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> MatchAllQueryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> MatchBoolPrefixFullTextAsync(string customerFullName)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> MatchPhraseFullTextAsync(string customerFullName)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> MatchPhrasePrefixFullTextAsync(string customerFullName)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> MatchQueryFullTextAsync(string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> MultiMatchQueryFullTextAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> PaginationQueryAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ImmutableList<ECommerce>>> WildCardQueryAsync(string customerFullName)
        {
            throw new NotImplementedException();
        }
    }
}
