using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return services
                //.AddValidatorsFromAssembly(assembly)
                //.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(assembly);
                });
        }

        public static void ConfigurationRabbitMQServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, mqConfig) =>
                {
                    var hostAddress = configuration["EventBusSettings:HostAddress"];

                    if (string.IsNullOrWhiteSpace(hostAddress))
                        throw new InvalidOperationException("RabbitMQ HostAddress is not configured.");

                    mqConfig.Host(new Uri(hostAddress));

                });
            });

        }
    }
}
