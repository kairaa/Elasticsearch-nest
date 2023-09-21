using Elasticsearch.Api.Models;

namespace Elasticsearch.Api.Dtos
{
    //record ile dto'yu nesneye cevirirken prop'un degismesini onleriz
    public record ProductCreateDto(string Name, decimal Price, int Stock, ProductFeatureDto Feature)
    {
        public Product CreateProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature
                {
                    Width = Feature.Width,
                    Height = Feature.Height,
                    Color = (ProductColor)int.Parse(Feature.Color)
                }
            };
        }
    }
}
