using EventBus.Messages.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Tool.Services
{
    public class EventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishOrderCreatedAsync(
            int orderId,
            decimal totalPrice)
        {
            var message = new OrderCreatedEvent
            {
                Id = orderId,
                TotalPrice = totalPrice
            };

            await _publishEndpoint.Publish(message);
        }
    }
}
