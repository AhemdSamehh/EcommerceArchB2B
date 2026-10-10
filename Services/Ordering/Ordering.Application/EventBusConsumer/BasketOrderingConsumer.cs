using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Ordering.Commands;
using Ordering.Application.Ordering.Commands.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly ILogger<BasketOrderingConsumer> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public BasketOrderingConsumer(
            ILogger<BasketOrderingConsumer> logger,
            IMapper mapper,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            using var scope = _logger.BeginScope(
       "Consuming Basket Checkout Event for {CorrelationId}",
       context.Message.CorrelationId);

     
            var createOrderDto = _mapper.Map<CreateOrderDto>(context.Message);
            createOrderDto.CorrelationId = context.Message.CorrelationId;
            var command = new CreateOrderCommand { CreateOrderDto = createOrderDto };

            await _mediator.Send(command);

            _logger.LogInformation(
                "Received BasketCheckoutEventB2B for User: {UserName}, Total Price: {TotalPrice}",
                context.Message.UserName,
                context.Message.TotalPrice);
        }
    }
}
