using AutoMapper;
using Catalog.Application.Mapper;
using Catalog.core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Catalog.Queries.Dtos
{
    public class ProductDto : IMapFrom<Product>
    {

        public string Id { get; init; }
        public string Name { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public string ImageFile { get; init; }
        public ProductBrand Brand { get; init; }
        public ProductType Type { get; init; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>();
            
        }
    }
}
