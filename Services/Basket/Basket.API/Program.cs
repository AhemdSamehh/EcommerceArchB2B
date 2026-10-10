
using Basket.Application;
using Basket.Application.GrpcService;
using Basket.Application.Mapper;
using Basket.Application.Settings;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Settings;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace Basket.API
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
            builder.Services.AddApplicationServices();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<DiscountGrpcService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            //Options pattern
            builder.Services.Configure<CacheSettings>(
                builder.Configuration.GetSection("CacheSettings"));
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["CacheSettings:ConnectionString"];
            });

            builder.Services.Configure<GrpcSettings>(
       builder.Configuration.GetSection("GrpcSettings"));
            // Register the wrapper that depends on the generated client
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    (sp, cfg) =>
    {
        var grpcSetting = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
        cfg.Address = new Uri(grpcSetting.DiscountUrl);
    });


            //Add mass Transit 

            builder.Services.ConfigurationRabbitMQServices(builder.Configuration);

            var app = builder.Build();

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
