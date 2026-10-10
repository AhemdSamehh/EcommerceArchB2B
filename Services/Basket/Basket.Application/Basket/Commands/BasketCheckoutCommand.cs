using AutoMapper;
using Basket.Application.Basket.Commands.Dtos;
using Basket.Application.Basket.Queries;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Mediator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Basket.Commands
{

	public record BasketCheckoutCommand : IRequest<Unit>
	{
        public BasketCheckoutDto BasketCheckoutDto { get; set; } = null!;
    }

	public class BasketCheckoutCommandHandler : IRequestHandler<BasketCheckoutCommand , Unit>
	{
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketCheckoutCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly MediatR.IMediator _mediator;

        public BasketCheckoutCommandHandler(IPublishEndpoint publishEndpoint , ILogger<BasketCheckoutCommandHandler> logger
            , IMapper mapper , MediatR.IMediator mediator)
        {
            _publishEndpoint=publishEndpoint;
            _logger=logger;
            _mapper=mapper;
            _mediator=mediator;
        }
        public async Task<Unit> Handle(BasketCheckoutCommand request, CancellationToken cancellationToken)
		{
			var basketCheckout = _mapper.Map<BasketCheckoutEvent>(request.BasketCheckoutDto);
            var basketResponse = await _mediator.Send(new GetBasketByUserNameQuery { UserName = basketCheckout.UserName! }, cancellationToken);
            if (basketResponse is null || !basketResponse.Items.Any())
            {
                throw new InvalidOperationException("Basket not found or empty");
            }
            basketCheckout.TotalPrice = basketResponse.Items.Sum(x => x.Price * x.Quantity);

            await _publishEndpoint.Publish(basketCheckout, cancellationToken);
            _logger.LogInformation("Basket checkout event published for user: {UserName}", request.BasketCheckoutDto.UserName);

            await _mediator.Send(
               new DeleteBasketByUserNameCommand { UserName = basketCheckout.UserName! },
               cancellationToken);
            _logger.LogInformation("Basket deleted for user: {UserName}", request.BasketCheckoutDto.UserName);
            return Unit.Value;
        }
	}
}
