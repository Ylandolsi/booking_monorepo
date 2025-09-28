using System.Net.Http.Json;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions.Authentication;
using IntegrationsTests.Abstractions.Base;

namespace IntegrationsTests.Tests.Users;

public class LanguageTests : AuthenticationTestBase
{
    public LanguageTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAllLanguages_ShouldReturnLanguages_ForAnyUser()
    {
        //UserData userData = await CreateUserAndLogin(); 

        var response = await ActClient.GetAsync(UsersEndpoints.GetAllLanguages);

        response.EnsureSuccessStatusCode();
        var languages = await response.Content.ReadFromJsonAsync<List<Language>>();

        Assert.NotNull(languages);
        Assert.NotEmpty(languages);
    }

    [Fact]
    public async Task GetUserLanguages_ShouldReturnUserLanguages_WhenUserIsAuthenticated()
    {
        var loginResponse = await CreateUserAndLogin();

        var response =
            await ActClient.GetAsync(UsersEndpoints.GetUserLanguages.Replace("{userSlug}", loginResponse.UserSlug));

        response.EnsureSuccessStatusCode();
        var languages = await response.Content.ReadFromJsonAsync<List<Language>>();
        Assert.NotNull(languages);
    }

    [Fact]
    public async Task GetOtherUserLanguages_ShouldReturnUserLanguages()
    {
        var loginResponse = await CreateUserAndLogin();
        var otherUserrData = await CreateUserAndLogin();

        var response =
            await ActClient.GetAsync(UsersEndpoints.GetUserLanguages.Replace("{userSlug}", loginResponse.UserSlug));

        response.EnsureSuccessStatusCode();
        var languages = await response.Content.ReadFromJsonAsync<List<Language>>();
        Assert.NotNull(languages);
    }

    [Fact]
    public async Task UpdateUserLanguages_ShouldUpdateLanguages_WhenUserIsAuthenticated()
    {
        var loginResponse = await CreateUserAndLogin();

        var languagesResponse = await ActClient.GetAsync(UsersEndpoints.GetAllLanguages);
        languagesResponse.EnsureSuccessStatusCode();

        var allLanguages = await languagesResponse.Content.ReadFromJsonAsync<List<Language>>();

        Assert.NotNull(allLanguages);
        Assert.NotEmpty(allLanguages);


        var updatePayload = new { LanguageIds = new List<int> { allLanguages[0].Id, allLanguages[1].Id } };
        var response = await ActClient.PutAsJsonAsync(UsersEndpoints.UpdateUserLanguages, updatePayload);

        response.EnsureSuccessStatusCode();

        // Verify the user now has these languages
        var userLanguagesResponse =
            await ActClient.GetAsync(UsersEndpoints.GetUserLanguages.Replace("{userSlug}", loginResponse.UserSlug));

        userLanguagesResponse.EnsureSuccessStatusCode();
        var userLanguages = await userLanguagesResponse.Content.ReadFromJsonAsync<List<Language>>();
        Assert.NotNull(userLanguages);
        Assert.Equal(2, userLanguages.Count);
    }
}