using Catalog.core.Repositories;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Add this using directive

namespace Catalog.Infrastructure
{
    public static class Startup
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(options =>
                configuration.GetSection("DatabaseSettings").Bind(options));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            // Register repositories
            return services
                .AddScoped<IBrandRepository, BrandRepository>()
                .AddScoped<ITypeRepository, TypeRepository>()
                .AddScoped<IProductRepository, ProductRepository>();
        }

        public static async Task AddDatabaseInitializer(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            // Retrieve the options
            var dbSettings = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseSettings>>();
            // Run the seeder which is asynchronous
            await DatabaseSeeder.SeedAsync(dbSettings);
        }
    }
}
