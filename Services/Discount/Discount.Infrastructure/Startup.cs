using Discount.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
            // or if using ConnectionStrings:DefaultConnection
            services.Configure<DatabaseSettings>(opts =>
            {
                opts.ConnectionString = configuration.GetConnectionString("DefaultConnection")!;
            });
            return services;
        }

  
    }
}
