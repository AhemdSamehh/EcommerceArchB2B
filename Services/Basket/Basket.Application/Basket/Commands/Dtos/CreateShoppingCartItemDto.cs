using AutoMapper;
using Basket.Application.Mapper;
using Basket.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Commands.Dtos
{
    public class CreateShoppingCartItemDto : IMapFrom<ShoppingCartItem>
    {
        public int Quantity { get; init; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; init; }
        public string ProductName { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateShoppingCartItemDto, ShoppingCartItem>().ReverseMap();
        }
    }
}
