using AutoMapper;
using Catalog.Application.Mapper;
using Catalog.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Catalog.Queries.Dtos
{
    public class BrandResponseDto : IMapFrom<ProductBrand>
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductBrand , BrandResponseDto>();
        }
    }
}
