using Elasticsearch.Api.Models.ECommerceModel;
using System.Collections.Immutable;

namespace Elasticsearch.Api.Repositories
{
    public interface IECommerceRepository
    {
        Task<ImmutableList<ECommerce>> TermQueryAsync(string customerFirstName);
        Task<ImmutableList<ECommerce>> TermsQueryAsync(List<string> customerFirstNameList);
        Task<ImmutableList<ECommerce>> PrefixQueryAsync(string customerFullName);
        Task<ImmutableList<ECommerce>> RangeQueryAsync(double fromPrice, double toPrice);
        Task<ImmutableList<ECommerce>> MatchAllQueryAsync();
        Task<ImmutableList<ECommerce>> PaginationQueryAsync(int page, int pageSize);
        Task<ImmutableList<ECommerce>> WildCardQueryAsync(string customerFullName);
        Task<ImmutableList<ECommerce>> FuzzyQueryAsync(string customerName);
        Task<ImmutableList<ECommerce>> MatchQueryFullTextAsync(string categoryName);
        Task<ImmutableList<ECommerce>> MultiMatchQueryFullTextAsync(string name);
        Task<ImmutableList<ECommerce>> MatchBoolPrefixFullTextAsync(string customerFullName);
        Task<ImmutableList<ECommerce>> MatchPhraseFullTextAsync(string customerFullName);
        Task<ImmutableList<ECommerce>> MatchPhrasePrefixFullTextAsync(string customerFullName);
        Task<ImmutableList<ECommerce>> CompoundQueryExampleOneAsync(string cityName, double taxfulTotalPrice, string categoryName, string menufacturer);
        Task<ImmutableList<ECommerce>> CompoundQueryExampleTwoAsync(string customerFullName);
    }
}
