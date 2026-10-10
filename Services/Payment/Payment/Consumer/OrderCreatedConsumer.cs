using EventBus.Messages.Events;
using MassTransit;

namespace Payment.Consumer
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IPublishEndpoint publishEndpoint, ILogger<OrderCreatedConsumer> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("OrderCreatedEvent received: {Message}", message);
            await Task.Delay(1000); // Simulate payment processing delay
            if (message.TotalPrice > 0)
            {
                var completeEvent = new PaymentCompletedEvent
                {
                    OrderId = message.Id,
                   CorrelationId = context.CorrelationId!.Value
                };
                await _publishEndpoint.Publish(completeEvent);
                _logger.LogInformation("Payment successful for Order ID: {OrderId}", message.Id);
            }
            else
            {
                var failedEvent = new PaymentFailedEvent
                {
                    OrderId = message.Id,
                    CorrelationId = context.CorrelationId!.Value,
                    Reason = "Invalid total price"
                };
                await _publishEndpoint.Publish(failedEvent);
                _logger.LogWarning("Payment failed for Order ID: {OrderId} due to invalid total price", message.Id);
                // Publish a PaymentFailedEvent or update order status in the database
            }
        }
    }
}
