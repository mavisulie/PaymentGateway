using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Domain.Entities;

/// <summary>
/// Payment context db.
/// </summary>
public class PaymentContext : DbContext
{
    public PaymentContext()
    {
    }

    public PaymentContext(DbContextOptions<PaymentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Payment> Payments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "PaymentGatewayDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasIndex(e => e.PaymentId);
        });
    }
}

