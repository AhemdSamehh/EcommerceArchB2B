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
    public class TypesResponseDto : IMapFrom<ProductType>
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductType, TypesResponseDto>();
        }
    }
}
