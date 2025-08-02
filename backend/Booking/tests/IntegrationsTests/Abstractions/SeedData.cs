using Booking.Modules.Users.Presistence;
using Booking.Modules.Users.Presistence.Seed.Users;
using Microsoft.EntityFrameworkCore;

namespace IntegrationsTests.Abstractions;

internal static class SeedData
{
    public static async Task Initialize(UsersDbContext context)
    {
        try
        {
            if (!await context.Languages.AnyAsync())
            {
                await context.Languages.AddRangeAsync(LanguageExpertiseSeeder.Languages());
                Console.WriteLine($"Added languages");
            }

            if (!await context.Expertises.AnyAsync())
            {
                await context.Expertises.AddRangeAsync(LanguageExpertiseSeeder.Expertises());
                Console.WriteLine($"Added expertises");
            }

            await context.SaveChangesAsync();
            Console.WriteLine("Seed data saved successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
}