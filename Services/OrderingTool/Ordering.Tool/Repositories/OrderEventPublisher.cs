using EventBus.Messages.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Tool.Repositories
{
    public sealed class OrderEventPublisher : IOrderEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderEventPublisher(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishOrderCreatedAsync(
            int orderId,
            decimal totalPrice,
            CancellationToken cancellationToken)
        {
            var message = new OrderCreatedEvent
            {
                Id = orderId,
                TotalPrice = totalPrice
            };

            await _publishEndpoint.Publish(
                message,
                cancellationToken);
        }

     
    }
}
