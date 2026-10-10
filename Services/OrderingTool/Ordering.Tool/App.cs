using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Tool.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Tool
{
    public sealed class App : BackgroundService
    {
        private readonly ILogger<App> _logger;
        private readonly IOrderEventPublisher _publisher;

        public App(
            ILogger<App> logger,
            IOrderEventPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation("Ordering Tool started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.Clear();

                Console.WriteLine("================================");
                Console.WriteLine("         ORDERING TOOL");
                Console.WriteLine("================================");
                Console.WriteLine();
                Console.WriteLine("1. Publish OrderCreatedEvent");
                Console.WriteLine("2. Exit");
                Console.WriteLine();
                Console.Write("Select: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await PublishOrderCreated(stoppingToken);
                        break;

                    case "2":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        await Task.Delay(1000, stoppingToken);
                        break;
                }
            }
        }

        private async Task PublishOrderCreated(
            CancellationToken cancellationToken)
        {
            Console.Write("Order ID: ");
            var orderId = Console.ReadLine();

            Console.Write("Total Price: ");
            var totalPriceInput = Console.ReadLine();

            if (!int.TryParse(orderId, out var parsedOrderId))
            {
                Console.WriteLine("Invalid Order ID.");
                return;
            }

            if (!decimal.TryParse(totalPriceInput, out var totalPrice))
            {
                Console.WriteLine("Invalid Total Price.");
                return;
            }

            await _publisher.PublishOrderCreatedAsync(
                parsedOrderId,
                totalPrice,
                cancellationToken);

            Console.WriteLine();
            Console.WriteLine("OrderCreatedEvent published successfully.");
            Console.WriteLine("Press ENTER to continue...");

            Console.ReadLine();
        }
    }
}
