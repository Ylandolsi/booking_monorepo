using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Availabilities;
using Booking.Modules.Mentorships.Domain.Entities.Days;
using Booking.Modules.Mentorships.Domain.Entities.Mentors;
using Booking.Modules.Mentorships.Domain.Entities.MentorshipRelationships;
using Booking.Modules.Mentorships.Domain.Entities.Payments;
using Booking.Modules.Mentorships.Domain.Entities.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Booking.Modules.Mentorships.Persistence;

public sealed class MentorshipsDbContext : DbContext
{
    public MentorshipsDbContext(DbContextOptions<MentorshipsDbContext> options) : base(options)
    {
    }

    // Entities
    public DbSet<Mentor> Mentors { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Day> Days { get; set; }
    public DbSet<MentorshipRelationship> MentorshipRelationships { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Availability> Availabilities { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Escrow> Escrows { get; set; }
    public DbSet<Payout> Payouts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schemas.Mentorships);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MentorshipsDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // to avoid the warning about DateTime && GUID for seeding data  
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

   
}