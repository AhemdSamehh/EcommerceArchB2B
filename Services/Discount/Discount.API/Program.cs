
using Discount.API.Services;
using Discount.Application;
using Discount.Application.Mapper;
using Discount.Core.Repositories;
using Discount.Infrastructure;
using Discount.Infrastructure.Repositories;
using Discount.Infrastructure.Settings;

namespace Discount.API
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
            builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
            // builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));
            builder.Services.AddGrpc();
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            builder.Services.AddApplicationServices();

            var app = builder.Build();
            app.MigrateDatabase();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();                     // add this
            app.UseAuthorization();
            app.MapGrpcService<DiscountService>();

            app.MapControllers();

            app.Run();
        }
    }
}
