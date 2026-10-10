using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Already present, but required for ConfigurationBuilder

namespace Ordering.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OutboxMessage> outboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OutboxMessage>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasIndex(x => x.CorrelationId); //for faster lookups
                builder.Property(x => x.Type).IsRequired();
                builder.Property(x => x.Content).IsRequired();
                builder.Property(x => x.OccurredOn).IsRequired();
                builder.Property(x => x.ProcessedOn).IsRequired(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalPrice).HasPrecision(18, 2);
            });
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 1) Try environment variable first (recommended for CI / CI agents)
                var conn = Environment.GetEnvironmentVariable("OnixConfigDb");
                if (!string.IsNullOrWhiteSpace(conn))
                {
                    optionsBuilder.UseSqlServer(conn);
                    return;
                }

                // 2) Fallback: load appsettings.json from the assembly location (optional)
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory();
                var config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .Build();

                conn = config.GetConnectionString("OnixConfigDb");
                if (!string.IsNullOrWhiteSpace(conn))
                {
                    optionsBuilder.UseSqlServer(conn);
                }
                // If still not configured, leave unconfigured so caller fails early with clear message.
            }
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "Rahul"; //TODO: Replace with User
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "Rahul";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
