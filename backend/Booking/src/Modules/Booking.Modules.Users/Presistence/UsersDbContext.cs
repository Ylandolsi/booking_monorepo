using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Domain.JoinTables;
using Booking.Modules.Users.Presistence.Seed.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>()
            .HaveConversion<string>();
    }

    // protected override void Up(MigrationBuilder migrationBuilder)
    // {
    // benefit : migration depends only on database schemas ( dont get affected when the model get changed (adding new require property wont break the migration ))
    //     migrationBuilder.SqlFile("InsertLanguages.sql", true);
    //     migrationBuilder.SqlFile("InsertExpertise.sql", true);
    // }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}