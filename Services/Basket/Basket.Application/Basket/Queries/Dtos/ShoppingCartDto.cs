using AutoMapper;
using Basket.Application.Mapper;
using Basket.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Queries.Dtos
{
    public class ShoppingCartDto : IMapFrom<ShoppingCart>
    {
        public string UserName { get; set; }
        public List<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();
        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ShoppingCart, ShoppingCartDto>();

        }
    }
}
