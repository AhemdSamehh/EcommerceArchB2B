using AutoMapper;
using MassTransit.Transports;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ordering.Application.Constants;
using Ordering.Application.Ordering.Commands.Dtos;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Ordering.Commands
{

	public record CreateOrderCommand : IRequest<int>
	{
        public CreateOrderDto CreateOrderDto { get; set; } = null!;	
    }

	public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand , int>
	{
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger , IMapper mapper)   
        {
            _orderRepository = orderRepository;
            _logger = logger;
            this._mapper=mapper;
        }
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
		{
            var orderEntity = _mapper.Map<Order>(request.CreateOrderDto);
            var createdOrder = await _orderRepository.AddAsync(orderEntity);
            var outboxMessage = new OutboxMessage
            {
                CorrelationId = request.CreateOrderDto.CorrelationId.ToString(),
                Type = OutboxMessageTypes.OrderCreated,
                OccurredOn = DateTime.UtcNow,
                Content = JsonConvert.SerializeObject(new
                {
                    createdOrder.Id,
                    createdOrder.UserName,
                    createdOrder.TotalPrice,
                    createdOrder.FirstName,
                    createdOrder.LastName,
                    createdOrder.AddressLine,
                    createdOrder.Country,
                    createdOrder.State,
                    createdOrder.ZipCode,
                    createdOrder.CardName,
                    createdOrder.CardNumber,
                    createdOrder.Expiration,
                    createdOrder.Cvv,
                    createdOrder.PaymentMethod,
                    createdOrder.Status
                })
            };
            await _orderRepository.AddOutboxMessageAsync(outboxMessage);
            _logger.LogInformation("Order with ID {OrderId} created and outbox message added with Correlation ID {CorrelationId}", createdOrder.Id, outboxMessage.CorrelationId);

            return createdOrder.Id;

        }
	}
}
