using Microsoft.EntityFrameworkCore;
using dotnet_microservices_boilerplate.OrderService.Domain.Entities;

namespace dotnet_microservices_boilerplate.OrderService.Infrastructure.Data;

public sealed class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Status).IsRequired();
            entity.HasMany(o => o.Items)
                  .WithOne()
                  .HasForeignKey("OrderId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("order_items");
            entity.HasKey(i => i.Id);
            entity.Property<Guid>("OrderId");
            entity.Property(i => i.ProductName);
            entity.Property(i => i.Quantity);
            entity.Property(i => i.UnitPrice);
        });
    }
}
