﻿namespace Elasticsearch.Api.Dtos
{
    public record ProductDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? Feature)
    {
    }
}
