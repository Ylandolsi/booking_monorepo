using Booking.Common;
using Booking.Common.Domain.DomainEvent;
using Booking.Common.Domain.Entity;
using Booking.Modules.Mentorships.Domain.Entities;
using Booking.Modules.Mentorships.Domain.Entities.Availabilities;
using Booking.Modules.Mentorships.Domain.Entities.Days;
using Booking.Modules.Mentorships.Domain.Entities.Mentors;
using Booking.Modules.Mentorships.Domain.Entities.MentorshipRelationships;
using Booking.Modules.Mentorships.Domain.Entities.Payments;
using Booking.Modules.Mentorships.Domain.Entities.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

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
    public DbSet<Wallet> Wallets { get; set;  }
    public DbSet<Escrow> Escrows { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    // Outbox messages
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

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
        await ConvertDomainEventsToOutboxMessages();

        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    private async Task ConvertDomainEventsToOutboxMessages()
    {
        var domainEvents = new List<IDomainEvent>();

        // Domain events from all entities that implement IEntity
        var entityEntries = ChangeTracker
            .Entries()
            .Where(entry => entry.Entity is IEntity)
            .Select(entry => entry.Entity as IEntity)
            .Where(entity => entity != null)
            .ToList();

        foreach (var entity in entityEntries)
        {
            domainEvents.AddRange(entity.DomainEvents);
            entity.ClearDomainEvents();
        }

        if (!domainEvents.Any())
        {
            return;
        }


        var outboxMessages = domainEvents.Select(domainEvent => new OutboxMessage(
            domainEvent.GetType().FullName!,
            JsonConvert.SerializeObject(
                domainEvent,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }),
            DateTime.UtcNow
        )).ToList();

        await OutboxMessages.AddRangeAsync(outboxMessages);
    }
}