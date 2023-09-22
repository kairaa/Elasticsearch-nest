using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.TermVectors;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.Api.Configurations;
using Elasticsearch.Api.Models;
using Elasticsearch.Api.Models.ECommerceModel;

namespace Elasticsearch.Api.Repositories
{
    public class ECommerceRepository : IECommerceRepository
    {
        private readonly ElasticsearchClient _client;

        public ECommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<ImmutableList<ECommerce>> TermQueryAsync(string customerFirstName)
        {
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
            //.Query(q => q.Term
            //    (t => t.Field("customer_first_name.keyword").Value(customerFirstName))));

            //daha guvenli
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
            //.Query(q => q.Term
            //    (t => t.CustomerFirstName.Suffix("keyword"), customerFirstName)));

            //baska bir yontem
            var termQuery = new TermQuery("customer_first_name.keyword")
            {
                Value = customerFirstName,
                CaseInsensitive = true
            };
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                                      .Query(termQuery));


            if (!result.IsSuccess())
                return null;

            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;
            }
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermsQueryAsync(List<string> customerFirstNameList)
        {
            var terms = new List<FieldValue>();

            customerFirstNameList.ForEach(c => terms.Add(c));

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
            //.Size(100)
            .Query(q => q
            .Terms(t => t
            .Field(f => f.CustomerFirstName
            .Suffix("keyword"))
            .Terms(new TermsQueryField(terms.AsReadOnly())))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PrefixQueryAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
            //.Size(100)
            .Query(q => q
            .Prefix(p => p
            .Field(f => f.CustomerFullName
            .Suffix("keyword")).Value(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> RangeQueryAsync(double fromPrice, double toPrice)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
            .Query(q => q
            .Range(r => r
            .NumberRange(n => n
            .Field(f => f.TaxfulTotalPrice)
            .Gte(fromPrice)
            .Lte(toPrice)))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchAllQueryAsync()
        {
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            //    .Size(100)
            //    .Query(q => q.MatchAll()));
            var result = await _client.SearchAsync<ECommerce>(s =>
                s.Index(Indexes.ECommerceIndex).Size(1000).Query(q => q.Match(m => m.Field(f => f.CustomerFullName).Query("shaw"))));


            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PaginationQueryAsync(int page, int pageSize)
        {
            // page=1, pageSize=10 =>  1-10
            // page=2 , pageSize=10=> 11-20
            // page=3, pageSize=10 => 21-30
            var pageFrom = (page - 1) * pageSize;

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                .Size(pageSize).From(pageFrom)
                .Query(q => q.MatchAll()));


            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> WildCardQueryAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)

                .Query(q => q.Wildcard(w =>
                    w.Field(f => f.CustomerFullName
                            .Suffix("keyword"))
                                .Wildcard(customerFullName))));


            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> FuzzyQueryAsync(string customerName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                .Query(q => q.Fuzzy(fu =>
                    fu.Field(f => f.CustomerFirstName.Suffix("keyword")).Value(customerName)
                        .Fuzziness(new Fuzziness(2))))
                            .Sort(sort => sort
                                .Field(f => f.TaxfulTotalPrice, new FieldSort() { Order = SortOrder.Desc })));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }


        public async Task<ImmutableList<ECommerce>> MatchQueryFullTextAsync(string categoryName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                .Size(1000).Query(q => q
                    .Match(m => m
                        .Field(f => f.Category)
                        .Query(categoryName).Operator(Operator.And))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> MultiMatchQueryFullTextAsync(string name)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                .Size(1000).Query(q => q
                    .MultiMatch(mm =>
                        mm.Fields(new Field("customer_first_name")
                            .And(new Field("customer_last_name"))
                            .And(new Field("customer_full_name")))
                            .Query(name))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchBoolPrefixFullTextAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                .Size(1000).Query(q => q
                    .MatchBoolPrefix(m => m
                        .Field(f => f.CustomerFullName)
                        .Query(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }


        public async Task<ImmutableList<ECommerce>> MatchPhraseFullTextAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                .Size(1000).Query(q => q
                    .MatchPhrase(m => m
                        .Field(f => f.CustomerFullName)
                        .Query(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();

        }

        public async Task<ImmutableList<ECommerce>> MatchPhrasePrefixFullTextAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                .Size(1000).Query(q => q
                    .MatchPhrasePrefix(m => m
                        .Field(f => f.CustomerFullName)
                        .Query(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }


        public async Task<ImmutableList<ECommerce>> CompoundQueryExampleOneAsync(string cityName, double taxfulTotalPrice, string categoryName, string menufacturer)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                .Size(1000).Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Term(t => t
                                .Field("geoip.city_name")
                                .Value(cityName)))
                        .MustNot(mn => mn
                            .Range(r => r
                                .NumberRange(nr => nr
                                    .Field(f => f.TaxfulTotalPrice)
                                    .Lte(taxfulTotalPrice))))
                        .Should(s => s.Term(t => t
                            .Field(f => f.Category.Suffix("keyword"))
                            .Value(categoryName)))
                        .Filter(f => f
                            .Term(t => t
                                .Field("manufacturer.keyword")
                                .Value(menufacturer))))


                ));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();

        }



        public async Task<ImmutableList<ECommerce>> CompoundQueryExampleTwoAsync(string customerFullName)
        {
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            //	.Size(1000).Query(q =>q.MatchPhrasePrefix(m=>m.Field(f=>f.CustomerFullName).Query(customerFullName))));

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(Indexes.ECommerceIndex)
                .Size(1000).Query(q => q
                    .Bool(b => b
                        .Should(m => m
                            .Match(m => m
                                .Field(f => f.CustomerFullName)
                                .Query(customerFullName))
                            .Prefix(p => p
                                .Field(f => f.CustomerFullName.Suffix("keyword"))
                                .Value(customerFullName))))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();

        }

    }
}
