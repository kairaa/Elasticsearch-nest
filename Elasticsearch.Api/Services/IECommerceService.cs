using Elasticsearch.Api.Dtos;
using Elasticsearch.Api.Models.ECommerceModel;
using System.Collections.Immutable;

namespace Elasticsearch.Api.Services
{
    public interface IECommerceService
    {
        Task<ResponseDto<ImmutableList<ECommerce>>> GetAllByFirstName(string customerFirstName);
        Task<ResponseDto<ImmutableList<ECommerce>>> GetAllByFirstNameList(List<string> customerFirstNameList);
        Task<ResponseDto<ImmutableList<ECommerce>>> GetAllByFullName(string customerFullName);
        Task<ResponseDto<ImmutableList<ECommerce>>> GetAllByPriceRange(double fromPrice, double toPrice);
        Task<ResponseDto<ImmutableList<ECommerce>>> MatchAllQueryAsync();
        Task<ResponseDto<ImmutableList<ECommerce>>> PaginationQueryAsync(int page, int pageSize);
        Task<ResponseDto<ImmutableList<ECommerce>>> WildCardQueryAsync(string customerFullName);
        Task<ResponseDto<ImmutableList<ECommerce>>> FuzzyQueryAsync(string customerName);
        Task<ResponseDto<ImmutableList<ECommerce>>> MatchQueryFullTextAsync(string categoryName);
        Task<ResponseDto<ImmutableList<ECommerce>>> MultiMatchQueryFullTextAsync(string name);
        Task<ResponseDto<ImmutableList<ECommerce>>> MatchBoolPrefixFullTextAsync(string customerFullName);
        Task<ResponseDto<ImmutableList<ECommerce>>> MatchPhraseFullTextAsync(string customerFullName);
        Task<ResponseDto<ImmutableList<ECommerce>>> MatchPhrasePrefixFullTextAsync(string customerFullName);
        Task<ResponseDto<ImmutableList<ECommerce>>> CompoundQueryExampleOneAsync(string cityName, double taxfulTotalPrice, string categoryName, string menufacturer);
        Task<ResponseDto<ImmutableList<ECommerce>>> CompoundQueryExampleTwoAsync(string customerFullName);
    }
}
