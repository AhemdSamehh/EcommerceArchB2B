using Basket.Application.Mapper;
using Basket.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Queries.Dtos
{
    public class ShoppingCartItemDto : IMapFrom<ShoppingCartItem>
    {
        public int Quantity { get; init; }
        public string ImageFile { get; init; }
        public decimal Price { get; init; }
        public string ProductId { get; init; }
        public string ProductName { get; init; }


        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<ShoppingCartItem, ShoppingCartItemDto>();
        }
    }
}
