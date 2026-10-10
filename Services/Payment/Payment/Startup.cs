using EventBus.Messages.Common;
using MassTransit;
using Payment.Consumer;

namespace Payment
{
    public static class Startup
    {
        public static IServiceCollection ConfigureRabbitMQServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<OrderCreatedConsumer>();
                config.UsingRabbitMq((context, mqConfig) =>
                {
                    mqConfig.Host(configuration["EventBusSettings:HostAddress"]);
                    mqConfig.ReceiveEndpoint(EventBusConstant.OrderCreatedQueue, c =>
                    {
                        c.ConfigureConsumer<OrderCreatedConsumer>(context);
                    });

                });
            });

            return services;
        }

    }
}
