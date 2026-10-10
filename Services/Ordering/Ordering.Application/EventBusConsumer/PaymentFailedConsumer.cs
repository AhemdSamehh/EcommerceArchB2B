using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.EventBusConsumer
{
    public class PaymentFailedConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PaymentFailedConsumer> _logger;

        public PaymentFailedConsumer(IOrderRepository orderRepository, ILogger<PaymentFailedConsumer> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
           var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
            if (order == null)
            {
                _logger.LogError($"Order with id {context.Message.OrderId} not found.");
                return;
            }
            order.Status = OrderStatus.Failed;
            await _orderRepository.UpdateAsync(order);
            _logger.LogInformation($"Order with id {context.Message.OrderId} marked as Payment Failed.");
        }
    }
}
