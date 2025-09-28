using System.Net.Http.Json;
using Booking.Modules.Users.Domain.Entities;
using Booking.Modules.Users.Features;
using IntegrationsTests.Abstractions.Authentication;
using IntegrationsTests.Abstractions.Base;

namespace IntegrationsTests.Tests.Users;

public class ExpertiseTests : AuthenticationTestBase
{
    public ExpertiseTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAllExpertises_ShouldReturnExpertises_WhenUserIsUnAuthenticated()
    {
        var response = await ActClient.GetAsync(UsersEndpoints.GetAllExpertises);

        response.EnsureSuccessStatusCode();
        var expertises = await response.Content.ReadFromJsonAsync<List<Expertise>>();
        Assert.NotNull(expertises);
        Assert.NotEmpty(expertises);
    }

    [Fact]
    public async Task GetUserExpertises_ShouldReturnUserExpertises_WhenUserIsAuthenticated()
    {
        var userData = await CreateUserAndLogin();

        var response =
            await ActClient.GetAsync(UsersEndpoints.GetUserExpertises.Replace("{userSlug}", userData.UserSlug));

        response.EnsureSuccessStatusCode();
        var expertises = await response.Content.ReadFromJsonAsync<List<Expertise>>();
        Assert.NotNull(expertises);
    }

    [Fact]
    public async Task GetOtherUserExpertises_ShouldReturnUserExpertises_WhenUserIsAuthenticated()
    {
        var userData = await CreateUserAndLogin();
        var useData2 = await CreateUserAndLogin();

        var response =
            await ActClient.GetAsync(UsersEndpoints.GetUserExpertises.Replace("{userSlug}", userData.UserSlug));

        response.EnsureSuccessStatusCode();
        var expertises = await response.Content.ReadFromJsonAsync<List<Expertise>>();
        Assert.NotNull(expertises);
    }

    [Fact]
    public async Task UpdateUserExpertise_ShouldUpdateExpertises_WhenUserIsAuthenticated()
    {
        var userData = await CreateUserAndLogin();


        // Get all available expertises
        var expertisesResponse = await ActClient.GetAsync(UsersEndpoints.GetAllExpertises);
        expertisesResponse.EnsureSuccessStatusCode();

        var allExpertises = await expertisesResponse.Content.ReadFromJsonAsync<List<Expertise>>();

        Assert.NotNull(allExpertises);
        Assert.NotEmpty(allExpertises);

        var updatePayload = new { ExpertiseIds = new List<int> { allExpertises[0].Id, allExpertises[1].Id } };

        var response = await ActClient.PutAsJsonAsync(UsersEndpoints.UpdateUserExpertise, updatePayload);

        response.EnsureSuccessStatusCode();

        var userExpertisesResponse =
            await ActClient.GetAsync(UsersEndpoints.GetUserExpertises.Replace("{userSlug}", userData.UserSlug));

        userExpertisesResponse.EnsureSuccessStatusCode();
        var userExpertises = await userExpertisesResponse.Content.ReadFromJsonAsync<List<Expertise>>();

        Assert.NotNull(userExpertises);
        Assert.Equal(2, userExpertises.Count);
    }
}