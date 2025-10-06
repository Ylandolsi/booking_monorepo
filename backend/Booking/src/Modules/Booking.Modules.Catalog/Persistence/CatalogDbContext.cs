using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Domain.Entities.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Booking.Modules.Catalog.Persistence;

public sealed class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    // Entities
    public DbSet<Store> Stores { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<SessionProduct> SessionProducts { get; set; }
    public DbSet<SessionAvailability> SessionAvailabilities { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Escrow> Escrows { get; set; }
    public DbSet<Payout> Payouts { get; set; }
    public DbSet<Day> Days { get; set; }
    public DbSet<StoreVisit> StoreVisits { get; set; }
    public DbSet<StoreDailyStats> StoreDailyStats { get; set; }
    public DbSet<ProductDailyStats> ProductDailyStats { get; set; }
    public DbSet<AdminNotification> AdminNotifications { get; set; } = null!;
    // Todo add customer entity 

    public DbSet<BookedSession> BookedSessions { get; set; }

    public DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schemas.Catalog);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // to avoid the warning about DateTime && GUID for seeding data  
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>()
            .HaveConversion<string>();
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}