using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Tool;
using Ordering.Tool.Repositories;

IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((config, services) =>
    {
        ConfigureServices(services, config.Configuration);
    })
    .Build();

await host.RunAsync();

static IServiceCollection ConfigureServices(
    IServiceCollection services,
    IConfiguration config)
{
    services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            // RabbitMQ configuration
        });
    });

    services.AddTransient<
        IOrderEventPublisher,
        OrderEventPublisher>();

    services.AddHostedService<App>();

    return services;
}