using EventBus.Messages.Common;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.EventBusConsumer;
using Ordering.Application.PipeLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

             services
                .AddValidatorsFromAssembly(assembly)
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(assembly);
                });

            services.AddAutoMapper(typeof(Startup));

            return services;
            
        }

        public static IServiceCollection ConfigureRabbitMQServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketOrderingConsumer>();
                config.AddConsumer<PaymentCompletedConsumer>();
                config.AddConsumer<PaymentFailedConsumer>();
                config.UsingRabbitMq((context, mqConfig) =>
                {
                    var hostAddress = configuration["EventBusSettings:HostAddress"];

                    if (string.IsNullOrWhiteSpace(hostAddress))
                        throw new InvalidOperationException("RabbitMQ HostAddress is not configured.");

                    mqConfig.Host(new Uri(hostAddress));

                    mqConfig.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, endpoint =>
                    {
                        endpoint.ConfigureConsumer<BasketOrderingConsumer>(context);
                    });

                    mqConfig.ReceiveEndpoint(EventBusConstant.PaymentCompletedQueue, endpoint =>
                    {
                        endpoint.ConfigureConsumer<PaymentCompletedConsumer>(context);
                    });

                    mqConfig.ReceiveEndpoint(EventBusConstant.PaymentFailedQueue, endpoint =>
                    {
                        endpoint.ConfigureConsumer<PaymentFailedConsumer>(context);
                    });
                });
            });

            return services;
        }

    }
}
