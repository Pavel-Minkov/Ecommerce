using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CorrelationId);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.OccurredOn).IsRequired();
                entity.Property(e => e.ProcessedOn).IsRequired(false);
            });
            modelBuilder.Entity<Order>().Property(o => o.Status).HasConversion<string>();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "Pesho";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "Pesho";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
