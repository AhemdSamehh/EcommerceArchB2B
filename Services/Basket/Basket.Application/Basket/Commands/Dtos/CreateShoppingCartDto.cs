using Basket.Application.Mapper;
using Basket.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Commands.Dtos
{
    public class CreateShoppingCartDto : IMapFrom<ShoppingCart>
    {
        public string UserName { get; init; }
        public List<CreateShoppingCartItemDto> Items { get; init; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<CreateShoppingCartDto, ShoppingCart>().ReverseMap();
        }
    }
}
