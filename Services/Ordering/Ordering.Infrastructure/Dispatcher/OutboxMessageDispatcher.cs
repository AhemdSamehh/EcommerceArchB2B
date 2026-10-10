using EventBus.Messages.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Dispatcher
{
    public class OutboxMessageDispatcher : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxMessageDispatcher> _logger;

        public OutboxMessageDispatcher(IServiceProvider serviceProvider , ILogger<OutboxMessageDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
                var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                var pendingMessage = await dbContext.outboxMessages
                    .Where(m => m.ProcessedOn == null)
                    .OrderBy(m => m.OccurredOn).Take(20).ToListAsync();

                foreach (var message in pendingMessage)
                {
                    try
                    {
                        var messageType = Type.GetType(message.Type);
                        var deserializedMessage = JsonConvert.DeserializeObject<OrderCreatedEvent>(message.Content);
                        await publishEndpoint.Publish(deserializedMessage);
                        message.ProcessedOn = DateTime.UtcNow;
                        _logger.LogInformation("Successfully dispatched outbox message with Id {MessageId}", message.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to dispatch outbox message with Id {MessageId}", message.Id);
                        message.Error = ex.Message;
                    }
                }
                await dbContext.SaveChangesAsync();
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
