using Booking.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Modules.Users.Presistence.Seed.Users;

internal static class SeedData
{
    public static async Task Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expertise>().HasData(LanguageExpertiseSeeder.Expertises());
        modelBuilder.Entity<Language>().HasData(LanguageExpertiseSeeder.Languages());

    }

}