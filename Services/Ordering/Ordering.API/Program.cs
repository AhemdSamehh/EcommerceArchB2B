using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Application.Dispatcher;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplicationService();
            // Register Outbox and RabbitMQ services
            builder.Services.AddHostedService<OutboxMessageDispatcher>();
            builder.Services.ConfigureRabbitMQServices(builder.Configuration);
            var app = builder.Build();


            //Migration
            app.MigrateDatabase<OrderContext>((context, services) =>
            {
                var logger = services.GetRequiredService<ILogger<OrderContextSeed>>();

                OrderContextSeed.SeedAsync(context, logger)
                    .GetAwaiter()
                    .GetResult();
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
