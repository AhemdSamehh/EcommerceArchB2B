using AutoMapper;
using Basket.Application.Basket.Queries.Dtos;
using Basket.Core.Repositories;
using Basket.Infrastructure.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Queries
{

	public record GetBasketByUserNameQuery : IRequest<ShoppingCartDto>
	{
        public string UserName { get; set; } = null!;
    }

	public class GetBasketByUserNameQueryHandler : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartDto>
	{
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBasketByUserNameQueryHandler> _logger;

        public GetBasketByUserNameQueryHandler(IBasketRepository basketRepository , IMapper mapper , ILogger<GetBasketByUserNameQueryHandler> logger)
        {
            _basketRepository=basketRepository;
            _mapper=mapper;
            _logger=logger;
        }
        public async Task<ShoppingCartDto> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
		{
            var basket = await _basketRepository.GetBasket(request.UserName);   
            if (basket == null)
            {
                return new ShoppingCartDto 
                { UserName = request.UserName,
                    Items = new List<ShoppingCartItemDto>()
                };
            }
            _logger.LogInformation("Fetched basket for {@UserName}", request.UserName);

            var basketDto = _mapper.Map<ShoppingCartDto>(basket);
            return basketDto;
        }
	}
}
