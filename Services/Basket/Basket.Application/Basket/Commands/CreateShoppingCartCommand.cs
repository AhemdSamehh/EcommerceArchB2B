using AutoMapper;
using Basket.Application.Basket.Commands.Dtos;
using Basket.Application.GrpcService;
using Basket.Core.Repositories;
using Basket.Infrastructure.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Commands
{

	public record CreateShoppingCartCommand : IRequest<CreateShoppingCartDto>
	{
        public CreateShoppingCartDto  CreateShoppingCart { get; set; }
    }

	public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand , CreateShoppingCartDto>
	{
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly DiscountGrpcService _discountGrpcService;

        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository , IMapper mapper , DiscountGrpcService discountGrpcService)
        {
            _basketRepository=basketRepository;
            _mapper=mapper;
            _discountGrpcService=discountGrpcService;
        }
        public async Task<CreateShoppingCartDto> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
		{
            // Apply discount to each item in the shopping cart

            foreach (var item in request.CreateShoppingCart.Items)
            {
                var discount = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= discount.Amount;
            }
            var shoppingCart = _mapper.Map<ShoppingCart>(request.CreateShoppingCart);

            var createdShoppingCart = await _basketRepository.UpdateBasket(shoppingCart);

            var res =  _mapper.Map<CreateShoppingCartDto>(createdShoppingCart);

            return res; 
        }
    }
}
