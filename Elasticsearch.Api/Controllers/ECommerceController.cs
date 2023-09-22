using Elasticsearch.Api.Repositories;
using Elasticsearch.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommerceController : BaseController
    {
        private readonly IECommerceService _service;
        private readonly IECommerceRepository _repository;

        public ECommerceController(IECommerceService service, IECommerceRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpGet("{customerFirstName}")]
        public async Task<IActionResult> GetByFirstName(string customerFirstName)
        {
            return CreateActionResult(await _service.GetAllByFirstName(customerFirstName));
        }

        [HttpPost]
        public async Task<IActionResult> GetByFirstNameList(List<string> customerFirstNameList)
        {
            return CreateActionResult(await _service.GetAllByFirstNameList(customerFirstNameList));
        }
    
        [HttpGet("{customerFullName}")]
        public async Task<IActionResult> GetByFullName(string customerFullName)
        {
            return CreateActionResult(await _service.GetAllByFullName(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> GetByPriceRange(double fromPrice, double toPrice)
        {
            return CreateActionResult(await _service.GetAllByPriceRange(fromPrice, toPrice));
        }

        [HttpGet]
        public async Task<IActionResult> MatchAll()
        {
            return Ok(await _repository.MatchAllQueryAsync());
        }

        [HttpGet]
        public async Task<IActionResult> PaginationQuery(int page = 1, int pageSize = 3)
        {
            return Ok(await _repository.PaginationQueryAsync(page, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> WildCardQuery(string customerFullName)
        {
            return Ok(await _repository.WildCardQueryAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> FuzzyQuery(string customerName)
        {
            return Ok(await _repository.FuzzyQueryAsync(customerName));
        }

        [HttpGet]
        public async Task<IActionResult> MatchQueryFullText(string category)
        {
            return Ok(await _repository.MatchQueryFullTextAsync(category));
        }

        [HttpGet]
        public async Task<IActionResult> MatchBoolPrefixQueryFullText(string customerFullName)
        {
            return Ok(await _repository.MatchBoolPrefixFullTextAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> MatchPhraseQueryFullText(string customerFullName)
        {
            return Ok(await _repository.MatchPhraseFullTextAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> MatchPhrasePrefixQueryFullText(string customerFullName)
        {
            return Ok(await _repository.MatchPhrasePrefixFullTextAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExampleOne(string cityName, double taxfulTotalPrice, string categoryName, string menufacturer)
        {
            return Ok(await _repository.CompoundQueryExampleOneAsync(cityName, taxfulTotalPrice, categoryName, menufacturer));
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExampleTwo(string customerFullName)
        {
            return Ok(await _repository.CompoundQueryExampleTwoAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> MultiMatchQueryFullText(string name)
        {
            return Ok(await _repository.MultiMatchQueryFullTextAsync(name));
        }
    }
}
