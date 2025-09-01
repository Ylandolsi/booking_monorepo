using System.Net.Http.Json;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features;
using Booking.Modules.Users.Features.Utils;
using IntegrationsTests.Abstractions;
using IntegrationsTests.Abstractions.Authentication;
using IntegrationsTests.Abstractions.Base;

namespace IntegrationsTests.Tests.Users;

public class ExperienceTests : AuthenticationTestBase
{
    public ExperienceTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AddExperience_ShouldCreateExperience_WhenUserIsAuthenticated()
    {
        LoginResponse loginResponse = await CreateUserAndLogin();

        var experiencePayload = new
        {
            Title = "Software Developer",
            Company = "Test Company",
            StartDate = DateTime.UtcNow.AddYears(-2),
            EndDate = DateTime.UtcNow.AddMonths(-3),
            Description = "Developed web applications using .NET Core"
        };

        var response = await ActClient.PostAsJsonAsync(UsersEndpoints.AddExperience, experiencePayload);

        response.EnsureSuccessStatusCode();

    }

    [Fact]
    public async Task GetExperiences_ShouldReturnExperiences()
    {
        LoginResponse loginResponse = await CreateUserAndLogin();

        // Add an experience first
        var experiencePayload = new
        {
            Title = "Software Developer",
            Company = "Test Company",
            StartDate = DateTime.UtcNow.AddYears(-2),
            EndDate = DateTime.UtcNow.AddMonths(-3),
            Description = "Developed web applications using .NET Core"
        };
        await ActClient.PostAsJsonAsync(UsersEndpoints.AddExperience, experiencePayload);

        var otherUserData = await CreateUserAndLogin();

        // Act
        var response = await ActClient.GetAsync(UsersEndpoints.GetUserExperiences.Replace("{userSlug}", loginResponse.UserSlug));

        // Assert
        response.EnsureSuccessStatusCode();
        List<Experience>? experiences = await response.Content.ReadFromJsonAsync<List<Experience>>();
        
        Console.WriteLine(experiences);
        Assert.NotNull(experiences);
        Assert.NotEmpty(experiences);
    }

    [Fact]
    public async Task UpdateExperience_ShouldUpdateExperience_WhenUserIsAuthenticated()
    {
        LoginResponse loginResponse = await CreateUserAndLogin();

        var experiencePayload = new
        {
            Title = "Software Developer",
            Company = "Test Company",
            StartDate = DateTime.UtcNow.AddYears(-2),
            EndDate = DateTime.UtcNow.AddMonths(-3),
            Description = "Developed web applications using .NET Core"
        };

        var createResponse = await ActClient.PostAsJsonAsync(UsersEndpoints.AddExperience, experiencePayload);
        createResponse.EnsureSuccessStatusCode();

        var experienceId = await createResponse.Content.ReadFromJsonAsync<int>();

        // Update the experience
        var updatePayload = new
        {
            Title = "Senior Developer",
            Company = "Updated Company",
            StartDate = DateTime.UtcNow.AddYears(-3),
            EndDate = DateTime.UtcNow.AddMonths(-1),
            Description = "Led development of enterprise applications"
        };

        // Act
        var response = await ActClient.PutAsJsonAsync(
            UsersEndpoints.UpdateExperience.Replace("{experienceId}", experienceId.ToString()),
            updatePayload);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteExperience_ShouldRemoveExperience_WhenUserIsAuthenticated()
    {
        LoginResponse loginResponse = await CreateUserAndLogin();
        
        // Add an experience first
        var experiencePayload = new
        {
            Title = "Software Developer",
            Company = "Test Company",
            StartDate = DateTime.UtcNow.AddYears(-2),
            EndDate = DateTime.UtcNow.AddMonths(-3),
            Description = "Developed web applications using .NET Core"
        };
        
        var createResponse = await ActClient.PostAsJsonAsync(UsersEndpoints.AddExperience, experiencePayload);
        createResponse.EnsureSuccessStatusCode();

        var experienceId = await createResponse.Content.ReadFromJsonAsync<int>();


        // Act
        var response = await ActClient.DeleteAsync(
            UsersEndpoints.DeleteExperience.Replace("{experienceId}", experienceId.ToString()));

        // Assert
        response.EnsureSuccessStatusCode();

        // Verify it's deleted
        var getResponse = await ActClient.GetAsync(UsersEndpoints.GetUserExperiences.Replace("{userSlug}", loginResponse.UserSlug));
        getResponse.EnsureSuccessStatusCode();
        var experiences = await getResponse.Content.ReadFromJsonAsync<List<object>>();
        Assert.Empty(experiences);
    }
}