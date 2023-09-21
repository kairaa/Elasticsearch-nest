using Elasticsearch.Api.Models;

namespace Elasticsearch.Api.Dtos
{
    public record ProductFeatureDto(int Width, int Height, string Color)
    {
    }
}
