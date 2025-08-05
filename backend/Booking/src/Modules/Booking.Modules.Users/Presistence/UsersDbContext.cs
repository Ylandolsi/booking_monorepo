using Booking.Common;
using Booking.Common.Domain.DomainEvent;
using Booking.Common.Domain.Entity;
using Booking.Common.Domain.Events;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Domain.JoinTables;
using Booking.Modules.Users.Presistence.Seed.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Booking.Modules.Users.Presistence;

public sealed class UsersDbContext
    : IdentityDbContext<User, IdentityRole<int>, int>
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    // Users Modules : 
    public DbSet<User> Users { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Expertise> Expertises { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Experience> Experiences { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // join tables 
    public DbSet<UserLanguage> UserLanguages { get; set; }
    public DbSet<UserExpertise> UserExpertises { get; set; }
    public DbSet<MentorMentee> UserMentors { get; set; }


    // outbox messages
    public DbSet<OutboxMessage> OutboxMessages { get; set; }


    // 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // to avoid the warning about DateTime && GUID for seeding data  
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);


        modelBuilder.HasDefaultSchema(Schemas.Users);
        SeedData.Seed(modelBuilder);
    }
    // protected override void Up(MigrationBuilder migrationBuilder)
    // {
    // benefit : migration depends only on database schemas ( dont get affected when the model get changed (adding new require property wont break the migration ))
    //     migrationBuilder.SqlFile("InsertLanguages.sql", true);
    //     migrationBuilder.SqlFile("InsertExpertise.sql", true);
    // }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await ConvertDomainEventsToOutboxMessages();
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    private async Task ConvertDomainEventsToOutboxMessages()
    {
        var domainEvents = new List<IDomainEvent>();

        // domain events from all entities that implement IEntity (including User)
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


        // TODO : check this approach instead of jsonConvert : 
        // var typeInfo = new Dictionary<string, string>
        // {
        //     ["Type"] = domainEvent.GetType().FullName!
        // };
        // var serializedData = JsonConvert.SerializeObject(domainEvent);
        // var metadata = JsonConvert.SerializeObject(typeInfo);

        // Store type info separately
        // var outboxMessages = domainEvents.Select(domainEvent => new OutboxMessage(
        // domainEvent.GetType().AssemblyQualifiedName!,
        // JsonConvert.SerializeObject(domainEvent),
        // DateTime.UtcNow
        // )).ToList();

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